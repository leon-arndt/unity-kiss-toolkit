using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour {
    public static CreditsController Instance;
    public PlayerController player1, player2;

    [SerializeField]
    private Text creditsText, p1ScoreText, p2ScoreText;

    private float scrollSpeed = 0.6f;
    public static uint p1Score = 0;
    public static uint p2Score = 0;

    // Use this for initialization
    void Start () {
        //audio
        AudioManager.instance.StopAll();
        AudioManager.instance.PlayEvent("Play_music_credits");


        if (Instance == null)
        {
            Instance = this;
        }
    }
	
	// Update is called once per frame
	void Update () {
        creditsText.rectTransform.position += scrollSpeed * Vector3.up * Time.deltaTime;

        //Exit the credits
        if (Input.GetAxis("Cancel") > 0 || Input.GetAxis("Pause") > 0)
        {
            ReturnToMainMenu();
        }
    }

    public void ReturnToMainMenu()
    {
        AudioManager.instance.StopAll();
        GameManager.LoadMainMenuScene();
    }

    public void UpdatePlayerScore(int playerID)
    {
        if (playerID == 0)
        {
            p1Score++;
            p1ScoreText.text = "Score: " + p1Score.ToString();
        }
        else
        {
            p2Score++;
            p2ScoreText.text = "Score: " + p2Score.ToString();
        }
    }
}
