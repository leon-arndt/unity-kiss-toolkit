using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //refactor

/// <summary>
/// This is the Quick Time Event component used for each QTE instance.
/// It combines logic and UI elements and refers to the QuickTimeEventManager.
/// There is a playerTransform reference to detect when the player walks away.
/// </summary>
public class QuickTimeEvent : MonoBehaviour {
    public List<KeyCode> requiredButtons = new List<KeyCode>();
    List<KeyCode> keyCodePool;
    int desiredButtonIndex = 0;
    float timePerKey = 2f;
    float timeForCurrentKey;
    public bool eventActive = false;

    public GameObject qteVisual;
    public Text debugText;
    public Image debugImage, reminderImage, buttonImage;
    public Animation correctAnimation;
    public Transform playerTransform;

    private System.Action actionWhenFinished;

    private void Start()
    {
        debugImage.type = Image.Type.Filled;
        debugImage.fillMethod = Image.FillMethod.Radial360;
        HideUI();
        buttonImage.sprite = null;
    }

    // Update is called once per frame
    void Update () {
        //only proceed if the event is active
        if (eventActive)
        {
            timeForCurrentKey -= Time.deltaTime;
            debugImage.fillAmount = timeForCurrentKey / timePerKey;

            //failed the QTE
            if (timeForCurrentKey < 0)
            {
                ExitQTE();
                return;
            }

            //exited the QTE by walking away
            if (Vector3.Distance(transform.position, playerTransform.position) > 3.5f)
            {
                ExitQTE();
            }

            KeyCode desiredButton = requiredButtons[desiredButtonIndex];

            bool validKey = Input.GetKeyDown(desiredButton);

            if (validKey)
            {
                Debug.Log("pressed the right button");

                //player animation
                playerTransform.GetComponent<PlayerController>().animator.SetTrigger("repairHit");

                //play the sound
                AudioManager.instance.PlayEvent("Play_repair");

                //show an animation
                correctAnimation.Play();

                if (desiredButtonIndex < requiredButtons.Count - 1)
                {
                    desiredButtonIndex++;
                    timeForCurrentKey = timePerKey;
                    Invoke("UpdateButton", correctAnimation.clip.length);
                }
                else
                {
                    WinQTE();
                }
            }
            else
            {
                //did the player hit a wrong button?
                foreach (KeyCode key in keyCodePool)
                {
                    //did the player press this button
                    if (Input.GetKeyDown(key))
                    {
                        ExitQTE();
                    }
                }
            }
        }
	}

    public void StartQTE(Transform callingTransform, System.Action actionWhenFinished = null)
    {
        //lock the player in place
        callingTransform.GetComponent<PlayerController>().enabled = false;

        //update the action
        this.actionWhenFinished = actionWhenFinished;

        //clear the required buttons. Can be left out to make repairing harder over time
        requiredButtons.Clear();

        //define population source
        if (callingTransform.GetComponent<PlayerController>().playerID  < Input.GetJoystickNames().Length)
        {
            if (callingTransform.GetComponent<PlayerController>().playerID == 1)
            {
                keyCodePool = QTEManager.Instance.validJoy2Buttons;
            }
            else
            {
                keyCodePool = QTEManager.Instance.validJoy1Buttons;
            }
        }
        else
        {
            if (callingTransform.GetComponent<PlayerController>().playerID == 1)
            {
                keyCodePool = QTEManager.Instance.validP1Keys;
            }
            else
            {
                keyCodePool = QTEManager.Instance.validP2Keys;
            }
        }
        
        //populate required buttons
        for (int i = 0; i < 3; i++)
        {
            requiredButtons.Add(keyCodePool[Random.Range(0, keyCodePool.Count)]);
        }

        //update the player transform
        playerTransform = callingTransform;

        //player animation
        playerTransform.GetComponent<PlayerController>().animator.SetBool("isRepairing", true);
        playerTransform.GetComponent<PlayerController>().SetHammerVisibility(true);

        timeForCurrentKey = timePerKey;
        eventActive = true;
        desiredButtonIndex = 0;

        ShowUI();
    }

    void ExitQTE()
    {
        eventActive = false;
        HideUI();

        //player animation
        playerTransform.GetComponent<PlayerController>().animator.SetBool("isRepairing", false);
        playerTransform.GetComponent<PlayerController>().SetHammerVisibility(false);

        //unlock the player
        playerTransform.GetComponent<PlayerController>().enabled = true;

        playerTransform = null;
    }

    void WinQTE()
    {
        Debug.Log("QTE Won");
        debugText.text = "";
        ExitQTE();

        //callback action
        if (actionWhenFinished != null)
        {
            actionWhenFinished.Invoke();
        }
    }

    private void ShowUI()
    {
        qteVisual.SetActive(true);
        UpdateButton();
    }

    public void SetReminderVisibility(bool visibility)
    {
        reminderImage.gameObject.SetActive(visibility);
    }

    private void HideUI()
    {
        debugText.text = "";
        qteVisual.SetActive(false);
    }

    private void UpdateButton()
    {
        string buttonText = TranslateKeyCodeToButtonText(requiredButtons[desiredButtonIndex].ToString());
        debugText.text = buttonText;
    }

    private string TranslateKeyCodeToButtonText(string keyCodeName)
    {
        //return string.Empty;
        buttonImage.sprite = QTEManager.Instance.transparent;
        
        if (keyCodeName.Contains("Button0"))
        {
            buttonImage.sprite = QTEManager.Instance.aButton;
            return "";
        }
        if (keyCodeName.Contains("Button1"))
        {
            buttonImage.sprite = QTEManager.Instance.bButton;
            return "";
        }
        if (keyCodeName.Contains("Button2"))
        {
            buttonImage.sprite = QTEManager.Instance.xButton;
            return "";
        }
        if (keyCodeName.Contains("Button3"))
        {
            buttonImage.sprite = QTEManager.Instance.yButton;
            return "";
        }
        
        if (keyCodeName.Contains("Up"))
        {
            return "↑";
        }
        
        if (keyCodeName.Contains("Left"))
        {
            return "←";
        }
        
        if (keyCodeName.Contains("Right"))
        {
            return "→";
        }
        
        if (keyCodeName.Contains("Down"))
        {
            return "↓";
        }
        
        //must be a keyboard (WASD) code
        return keyCodeName;
    }
}
