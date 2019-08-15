using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public GameObject hat, hammer;
    public Transform hatParentTransform, neckRigTransform;
    private PlayerPickUp playerPickUp;
    private FacialAnimation facialAnimation;
    private Player player; //rewired player
    
    public float speed;
    public float maxSpeed;
    public int playerID;
    public bool holdingSomething;
    public float dizzyTime;
    public float burningTime;
    public bool slowedEffect = false;
    public bool slipperyEffect = false;
    public float frictionFactor = 0.85f;
    public bool isJumping;
    public bool longNeck;

    private Rigidbody rb;
    public const float STANDARD_TURN_SPEED = 13f;
    public float turnSpeed = STANDARD_TURN_SPEED;
    private float maxVelocityY = 2f;
    private float jumpForce = 400f;
    private float jumpTime = 1.5f;
    private float jumpTimeLeft = -0.01f;
    private float startMaxSpeed;
    private float tauntTimeLeft = -0.01f;
    private float distanceBetweenFootsteps = 100f;
    private float distanceUntilNextFootstep = 100f;

    //disabled time means the player can't move
    private float disabledTime = 0f;

    public List<GameObject> nearbyInteractableList = new List<GameObject>();

    public KeyCode[] actionKey, throwKey, jumpKey, falconTauntKey, pauseKey;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPickUp = GetComponent<PlayerPickUp>();
        facialAnimation = GetComponent<FacialAnimation>();

        startMaxSpeed = maxSpeed;

        SetHammerVisibility(false);

        if (gameObject.tag == "Player2")
        {
            playerID = 1;
        }
        else
        {
            playerID = 0;
        }

        player = ReInput.players.GetPlayer(playerID);
    }


    void Update()
    {
        #region _PAUSE
        if (CustomInputManager.GetKeyPressed(pauseKey))
        {
            GrayscaleEffect.Instance.FadeToAndFromGray(Time.timeScale == 1);
            //show menu
            if (UISettings.Instance != null)
            {
                UISettings.Instance.SetPauseScreenVisibility(Time.timeScale == 1);
            }

            Time.timeScale = 1 - Time.timeScale;
        }
        #endregion


        #region _MOVEMENT
        float moveHorizontal = 0;
        float moveVertical = 0;

        float lookHorizontal = 0;
        float lookVertical = 0;

        if (dizzyTime > 0)
        {
            dizzyTime -= Time.deltaTime;
            animator.SetFloat("dizzyTime", dizzyTime);
        }

        if (burningTime > 0)
        {
            burningTime -= Time.deltaTime;
            animator.SetFloat("burnTime", burningTime);
        }
        

        //Player can only move if not dizzy and not jumping (also when not carried)
        if (burningTime > 0.1)
        {
            moveHorizontal = 0.5f * Mathf.Sin(5f * burningTime);
            moveVertical = 0.5f * Mathf.Cos(5f * burningTime);
        }
        else
        if (disabledTime < 0.2f && !playerPickUp.isPickedUp)
        {
            if (playerID == 0) //Player 1
            {
                moveHorizontal = player.GetAxis("Move Horizontal");
                moveVertical = player.GetAxis("Move Vertical");

                lookHorizontal = Input.GetAxis("P1HorizontalLook");
                lookVertical = Input.GetAxis("P1VerticalLook");
            }
            else //Player 2
            {
                moveHorizontal = player.GetAxis("Move Horizontal");
                moveVertical = player.GetAxis("Move Vertical");

                lookHorizontal = Input.GetAxis("P2HorizontalLook");
                lookVertical = Input.GetAxis("P2VerticalLook");
            }
        }


        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 lookDirection = new Vector3(lookHorizontal, 0.0f, lookVertical);

        //normalize vector if it the magnitude is > 1 to prevent diagonal movement from being faster
        if (movement.magnitude > 1)
        {
            movement /= movement.magnitude;
        }

        if (slowedEffect)
        {
            movement *= 0.5f;
            maxSpeed = 0.5f * startMaxSpeed;
        }
        else
        {
            maxSpeed = startMaxSpeed;
        }

        //check if it is free in front with a raycast
        float distanceToCheck = holdingSomething ? 2f : 0.1f;

        var hitInFront = Physics.RaycastAll(transform.position, transform.forward, distanceToCheck * movement.magnitude);
        Debug.DrawRay(transform.position, transform.forward);

        //any hits with objects which are not pickups will slow down the player
        bool nonPickupHit = false;

        if (!holdingSomething)
        {
            foreach (var hit in hitInFront)
            {
                if (hit.transform.GetComponent<Ingredient>() == null)
                {
                    nonPickupHit = true;
                }
            }
        }

        if (hitInFront.Length <= 1 || !nonPickupHit)
        {
            float timeFactor = 60f * Time.deltaTime;
            rb.AddForce(timeFactor * movement * speed);

            //footstep sounds
            distanceUntilNextFootstep -= rb.velocity.magnitude;
            if (distanceUntilNextFootstep < 0)
            {
                //audio
                AudioManager.instance.PlayEvent("Play_footstep");
                distanceUntilNextFootstep = distanceBetweenFootsteps;
                GameManager.instance.globalSaveData.stats.stepsTaken++;
            }

        }

        //limit the velocity
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.normalized.x * maxSpeed, rb.velocity.y, rb.velocity.normalized.z * maxSpeed);
        }
        
        //look in the direction of movement over time
        if (movement.magnitude > 0.1f)
        {
            RotateTowardsTarget(transform.position + movement, turnSpeed);
        }

        //look direction override
        if (lookDirection.magnitude > 0.3f && movement.magnitude < 0.2f)
        {
            RotateTowardsTarget(transform.position + lookDirection, turnSpeed);
        }

        if (rb.velocity.magnitude > 4f)
        {
            if (Random.Range(0, 100) > 30)
            {
                ParticleManager.Instance.CreateWalkParticles(transform);
            }
        }

        //animation
        if (animator != null)
        {
            animator.SetFloat("walkSpeed", movement.magnitude);
            if (movement.magnitude > 0.1f)
            {
                animator.speed = 1.2f * (rb.velocity.magnitude / maxSpeed);
            }
            else
                animator.speed = 1f;
        }

        //turn upright if skewed
        float yRot = transform.eulerAngles.y;
        Quaternion desired = Quaternion.Euler(0f, yRot, 0f);
        transform.rotation = desired;

        //friction
        CheckIfOnIce(transform.position, 1);
        float frictionAdjust = frictionFactor;
        if (slipperyEffect) frictionAdjust += 0.3f;

        //apply friction and prevent flying up
        rb.velocity = new Vector3(frictionAdjust * rb.velocity.x, Mathf.Min(maxVelocityY, rb.velocity.y), frictionAdjust * rb.velocity.z);
        #endregion

        #region _INTERACTION
        if (player.GetButtonDown("Action") && dizzyTime < 0.1f)
        {
            if (holdingSomething) //drop old interactable
            {
                DropObject();
            }
            else
            if (nearbyInteractableList.Count > 0) //interactable nearby
            {
                //interact with objects
                for (int i = 0; i < nearbyInteractableList.Count; i++)
                {
                    //skip the element if it is null
                    if (nearbyInteractableList[i] != null)
                    {
                        if (nearbyInteractableList[i].GetComponent<Interactable>().interactable)
                        {
                            Interactable target = nearbyInteractableList[i].GetComponent<Interactable>();

                            //delete uihint
                            target.DeleteUIHint();
                            target.Interact(transform);

                            if (target is PickUp) {
                                //audio for pickups
                                AudioManager.instance.PlayEvent("Play_player_pickup");
                            }
                            break;
                        }
                    }
                }

                //pick up pickup objects
                if (nearbyInteractableList[0].GetComponent<PickUp>() != null)
                {
                    animator.SetTrigger("pickUp");
                    
                    //definitely a pickup object
                    holdingSomething = true;
                    IncreaseColliderSize();
                }
            }
        }
        #endregion

        #region _THROWING
        if (player.GetButtonDown("Throw") && dizzyTime < 0.1f)
        {
            Throw();
        }
        #endregion

        #region _JUMP
        if (player.GetButtonDown("Leap") && dizzyTime < 0.1f && jumpTimeLeft < 0)
        {
            Jump();
        }
        //jump physics
        if (jumpTimeLeft > 0)
        {
            jumpTimeLeft -= Time.deltaTime;

            if (jumpTimeLeft > 1f)
            {
                Vector3 jumpVector;
                float jumpFactor = jumpTimeLeft / jumpTime;
                jumpVector = jumpFactor * jumpForce * transform.forward;
                rb.AddForce(Time.timeScale * jumpVector);
                facialAnimation.FlickerMaterial(jumpTimeLeft);
            }
        }
        #endregion

        #region _TAUNTS
        //falcon salute
        if (player.GetButtonDown("FalconTaunt") && disabledTime < 0.1f && !holdingSomething)
        {
            //audio
            AudioManager.instance.PlayEvent("Play_player_taunt");

            tauntTimeLeft = 1f;
            animator.SetTrigger("falconTaunt");
        }
        if (tauntTimeLeft > 0)
        {
            //look towards world z-
            RotateTowardsTarget(new Vector3(0, 0, -100), 10f);
            tauntTimeLeft -= Time.deltaTime;
        }

        //dance
        if (player.GetButton("Dance") && disabledTime < 0.1f && !holdingSomething)
        {
            animator.SetBool("dancing", true);
        }

        if (player.GetButtonUp("Dance"))
        {
            animator.SetBool("dancing", false);
        }

        #endregion

        #region _IDLEBREAKUPS
        bool animatorIsBusy = !animator.GetCurrentAnimatorStateInfo(4).IsName("Empty State");

        if (disabledTime < 0.1f && movement.magnitude < 0.1f && !holdingSomething && !animatorIsBusy)
        {
            //if the player is not busy play an idle animation
            if (Random.Range(0, 1000) > 994 && !animatorIsBusy)
            {
                //StartCoroutine(PlayIdleBreakup());
                int targetIdle = Random.Range(1, 4);
                animator.SetInteger("playBreakupIdle", targetIdle);
            }
        }
        else if (animatorIsBusy && movement.magnitude > 0.5f || holdingSomething)
        {
            //breaking out of the idle when moving (and animator is busy with idle animation)
            animator.SetInteger("playBreakupIdle", 0);
        }
        else if (animator.GetCurrentAnimatorStateInfo(4).normalizedTime > 0.9f)
        {
            //manually reset the idle animation int after normal time transition
            animator.SetInteger("playBreakupIdle", 0);
        }
        #endregion

        //calculate disabled time
        disabledTime = dizzyTime + jumpTimeLeft + tauntTimeLeft;
    }

    //find new interactables nearby
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Interactable>() != null)
        {
            if (other.GetComponent<Interactable>().interactable)
            {
                HandleNewInteractable(other.gameObject);
            }
        }
    }

    void HandleNewInteractable(GameObject other)
    {
        //add to nearby interactables list
        if (!nearbyInteractableList.Contains(other.gameObject))
        {
            foreach (GameObject item in nearbyInteractableList)
            {
                if (item.GetComponent<Interactable>().uiHint != null)
                {
                    item.GetComponent<Interactable>().uiHint.DestroySelf();
                }
            }
            nearbyInteractableList.Clear();
            nearbyInteractableList.Add(other.gameObject);
        }

        //create a UI hint only if the first item in the nearbyList
        if (nearbyInteractableList.Count == 1)
        {
            if (UIHintManager.instance.ShouldCreateUIHint(other.GetComponent<Interactable>()))
            {
                UIHintManager.instance.CreateUIHint(transform, other.transform);
            }
        }
    }


    //clear old ones when leaving trigger
    void OnTriggerExit(Collider other)
    {
        if (nearbyInteractableList.Contains(other.gameObject))
        {
            nearbyInteractableList.Remove(other.gameObject);

            //turn off the uihint
            if (!CheckOtherPlayerHasInteractableNearby(other.gameObject))
            {
                other.GetComponent<Interactable>().DeleteUIHint();
            }
        }
    }

    public void RemoveFromInteractableList(GameObject go)
    {
        nearbyInteractableList.Remove(go);
    }

    public static void RemoveFromBothInteractableLists(GameObject go)
    {
        GameController.instance.player1.RemoveFromInteractableList(go);
        GameController.instance.player2.RemoveFromInteractableList(go);
    }

    public void AddDizzyTime(float timeToAdd)
    {
        if (dizzyTime < 0.1f && jumpTimeLeft < 0.1f)
        {
            Debug.Log("Added Dizzy Time");
            dizzyTime += timeToAdd;
            ParticleManager.Instance.CreateStunParticle(transform); //stun particles
            GetComponent<FacialAnimation>().ChangeFaceToHit(); //Facial animation
            ForceFeedback.Vibrate(player); //vibrate
            if (CreditsController.Instance != null)
            {
                CreditsController.Instance.UpdatePlayerScore(1 - playerID);
            }

            //drop the held object too
            DropObject();

            //drop self if held
            playerPickUp.ReverToState();

            //audio
            AudioManager.instance.PlayEvent("Play_dizzy");
        }
    }

    public void AddBurnTime(float timeToAdd)
    {
        if (burningTime < 0.1f && jumpTimeLeft < 0.1f)
        {
            Debug.Log("Added Burning Time");
            burningTime += timeToAdd;
            ParticleManager.Instance.CreateStunParticle(transform); //stun particles
            GetComponent<FacialAnimation>().ChangeFaceToHit(); //Facial animation
            ForceFeedback.Vibrate(player); //vibrate
            if (CreditsController.Instance != null)
            {
                CreditsController.Instance.UpdatePlayerScore(1 - playerID);
            }

            //drop the held object too
            DropObject();

            //drop self if held
            playerPickUp.ReverToState();

            //audio
            AudioManager.instance.PlayEvent("Play_dizzy");
        }
    }

    public void DropObject()
    {
        //audio
        AudioManager.instance.PlayEvent("Play_player_drop");

        animator.SetTrigger("putDown");

        PickUp[] interactables = GameObjectExtensions.GetComponentsInDirectChildren<PickUp>(gameObject);
        foreach (PickUp pickUp in interactables)
        {
            pickUp.PutDown();
        }

        holdingSomething = false;
        ResetColliderSize();
    }

    private void Throw()
    {
        if (holdingSomething)
        {
            //audio
            AudioManager.instance.PlayEvent("Play_player_throw");

            animator.SetTrigger("throw");

            List<PickUp> interactables = new List<PickUp>(GetComponentsInChildren<PickUp>());
            interactables.Remove(GetComponent<PlayerPickUp>()); //remove player pickup

            foreach (PickUp pickUp in interactables)
                pickUp.Throw();


            holdingSomething = false;
            ResetColliderSize();
        }
    }

    public void IncreaseColliderSize()
    {
        GetComponent<CapsuleCollider>().radius = 1;
    }

    public void ResetColliderSize()
    {
        GetComponent<CapsuleCollider>().radius = 0.5f;
    }

    public bool CheckOtherPlayerHasInteractableNearby(GameObject goToCheck)
    {
        //does not work, refactor.

        PlayerController otherPlayer;
        if (GameController.instance != null)
        {
            otherPlayer = (playerID == 0) ? GameController.instance.player2 : GameController.instance.player1;
        }
        else if (TrainStation.instance != null)
        {
            otherPlayer = (playerID == 0) ? TrainStation.instance.player2 : TrainStation.instance.player1;
        }
        else
        {
            otherPlayer = (playerID == 0) ? CreditsController.Instance.player2 : CreditsController.Instance.player1;
        }

        return otherPlayer.nearbyInteractableList.Contains(goToCheck);
    }

    private void Jump()
    {
        if (!playerPickUp.isPickedUp)
        {
            //audio
            AudioManager.instance.PlayEvent("Play_player_leap");

            animator.SetTrigger("jump");

            jumpTimeLeft = jumpTime;

            Throw();
        }
    }

    private void RotateTowardsTarget(Vector3 target, float speed)
    {
        Vector3 targetPos = target;
        Vector3 relativePos = targetPos - transform.position;

        Quaternion desiredRot = Quaternion.LookRotation(relativePos, Vector3.up);
        Quaternion newRot = Quaternion.Lerp(transform.rotation, desiredRot, speed * Time.deltaTime);
        transform.rotation = newRot;
    }

    public void SetHammerVisibility(bool visible)
    {
        hammer.SetActive(visible);
    }

    //figure out if on ice
    void CheckIfOnIce(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].GetComponent<Ice>())
            {
                slipperyEffect = true;
                return;
            }
            i++;
        }
        slipperyEffect = false;
    }

    public void UpdateBeingCarriedAnimation(bool value)
    {
        animator.SetBool("isGrabbed", value);
    }
}