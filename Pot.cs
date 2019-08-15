using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour {
    //setting up singleton
    public static Pot Instance = null;

    public List<Ingredient> ingredientsInPot = new List<Ingredient>();
    public int desiredIngredientsInPot = 0;
    public int cookedIngredients = 0;

    public float cookTimeForCurrentIngredient;
    public float cookTimeLeftForCurrentIngredient;
    public int indexCurrentlyCooking;
    public bool isCooking = false;
    public bool isOpen = false;
    public bool isLocked = false; //locked when cooking

    private float clampSpinAmount = 90f;

    [SerializeField]
    private ParticleSystem psBubbles, psStartBubbles;

    [SerializeField]
    private PulldownLever pulldownLever;

    [SerializeField]
    private Transform clampLeft, clampRight;

    public Animator animator;

    private void Awake()
    {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    private void Start()
    {
        //vfx particle systems
        var emission = psBubbles.emission;
        emission.enabled = false;
    }
    private void Update()
    {
        if (isCooking)
        {
            if (cookTimeLeftForCurrentIngredient > 0)
            {
                cookTimeLeftForCurrentIngredient -= 0.1f * VehicleController.Instance.GetTemperature() * Time.deltaTime;
            }

            UIController.Instance.UpdateCookingBar(1 - (cookTimeLeftForCurrentIngredient / cookTimeForCurrentIngredient));


            //finished cooking current ingredient
            if (cookTimeLeftForCurrentIngredient <= 0)
            {
                FinishCooking();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        HandleTrigger(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleTrigger(other);
    }

    //handle both trigger cases
    void HandleTrigger(Collider other)
    {
        //check if the collider is an ingredient
        if (other.GetComponent<Ingredient>() != null)
        {
            //only allow ingredients if now cooking and if the pot is open
            if (isCooking == false && isOpen == true)
            {
                //ignore some ingredients TODO: add this in the above conditions (isCooking == false) so that child parteria cant be added to pot
                if (IsValidIngredient(other.GetComponent<Ingredient>()))
                {
                    ingredientsInPot.Add(other.GetComponent<Ingredient>());
                    Debug.Log("Pot received ingredient");
                    HandleNewIngredient(other.GetComponent<Ingredient>());
                }
                else
                {
                    //don't continue if parteria exception
                    return;
                }

                //if the player was carrying the object
                if (other.transform.parent != null)
                {
                    if (other.transform.parent.GetComponent<PlayerController>() != null)
                    {

                        //determine PlayerController
                        PlayerController player = other.transform.parent.GetComponent<PlayerController>();

                        //Player drops object automatically
                        player.DropObject();
                    }
                }

                //Clear from both players' nearby interactable list
                GameController.instance.player1.RemoveFromInteractableList(other.gameObject);
                GameController.instance.player2.RemoveFromInteractableList(other.gameObject);

                //disable the other ingredient and put it down beforehand
                other.GetComponent<Ingredient>().Pacify();
                other.gameObject.SetActive(false);

                //get rid of the ui hint if there is one
                other.GetComponent<Ingredient>().DeleteUIHint();
            }
        }
    }

    public List<Ingredient> GetIngredientsInPot()
    {
        return ingredientsInPot;
    }

    public bool IsValidIngredient(Ingredient ingredient)
    {
        if (ingredient is Parteria)
        {
            //don't cook child parteria
            if (!ingredient.GetComponent<Parteria>().isParent)
            {
                return false;
            }

            //don't cook unconnected parteria
            if (!ingredient.GetComponent<Parteria>().connected)
            {
                return false;
            }
        }
        return true;
    }

    public void HandleNewIngredient(Ingredient ingredient)
    {
        IngredientData[] desiredIngredients = GameController.instance.desiredIngredients;
        Debug.Log(desiredIngredients);
        for (int i = 0; i < desiredIngredients.Length; i++)
        {
            if (desiredIngredients[i].plantName == ingredient.ingredientData.plantName && GameController.instance.ingredientFound[i] == false)
            {
                EventManager.ThrowAddIngredientEvent();
                Debug.Log("Desired Ingredient identified in pot");
                desiredIngredientsInPot++;
                GameController.instance.ingredientFound[i] = true;

                //Start the cooking process
                StartCooking(ingredient, i);

                return; //don't check any more ingredients
            }
        }
    }

    public void CheckIfFoundAll()
    {
        if (desiredIngredientsInPot >= GameController.instance.desiredIngredients.Length)
        {
            Debug.Log("WE DID IT NASA!");
            GameController.instance.foundAllDesiredIngredients = true;
            GameController.instance.WinGame();
        }
    }

    public void StartCooking(Ingredient ingredient, int i)
    {
        //show the progress bar and start new cooktime
        indexCurrentlyCooking = i;
        cookTimeForCurrentIngredient = ingredient.ingredientData.cookTime;
        cookTimeLeftForCurrentIngredient = cookTimeForCurrentIngredient;
        isCooking = true;
        UIController.Instance.SetCookingScreenVisibility(true);

        //animations
        animator.SetTrigger("startCooking");

        //audio
        AudioManager.instance.PlayEvent("Play_pot_add");

        //vfx particle systems
        var emission = psBubbles.emission;
        emission.enabled = true;

        //startBubbles play once
        psStartBubbles.Clear();
        psStartBubbles.Play();

        //update stats
        GameManager.instance.globalSaveData.stats.totalPlantsPotted++;

        //let go of the pulldown lever and lock pot
        pulldownLever.SetPullDownState(false);
        isLocked = true;
        pulldownLever.interactable = false;
        pulldownLever.UpdateSignalLightColor(false);
        pulldownLever.MakePotLidColliderNonTrigger();

        //clamps
        StartCoroutine(SpinClamp(clampLeft, clampSpinAmount));
        StartCoroutine(SpinClamp(clampRight, clampSpinAmount));

    }

    private void FinishCooking()
    {
        EventManager.ThrowFinishedCookingEvent();

        cookedIngredients++;
        isCooking = false;
        UIController.Instance.UpdateMissionIngredientFound(indexCurrentlyCooking);
        UIController.Instance.SetCookingScreenVisibility(false);

        CheckIfFoundAll();
        //vfx particle systems
        var emission = psBubbles.emission;
        emission.enabled = false;

        //unlock the pot
        isLocked = false;
        pulldownLever.interactable = true;
        pulldownLever.UpdateSignalLightColor(true);

        //clamps
        StartCoroutine(SpinClamp(clampLeft, -clampSpinAmount));
        StartCoroutine(SpinClamp(clampRight, -clampSpinAmount));

        //audio
        AudioManager.instance.PlayEvent("Play_ui_select_rew");

        //animation
        animator.SetTrigger("finishCooking");
    }

    IEnumerator SpinClamp(Transform clampTransform, float amount)
    {
        float stepSize = 6f * Time.deltaTime;

        for (float f = 0f; f <= 1; f += stepSize)
        {
            float zRot = Mathf.SmoothStep(clampTransform.rotation.z, amount, f);
            clampTransform.Rotate(new Vector3(0, 0, stepSize * zRot));
            yield return null;
        }
    }
}