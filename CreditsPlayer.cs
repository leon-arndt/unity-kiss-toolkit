using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPlayer : MonoBehaviour {
    [SerializeField]
    uint playerID;

    RectTransform rt;
    float moveSpeed = 5f;

    [SerializeField]
    RectTransform canvasRT;

	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {

        float moveHorizontal = 0;
        float moveVertical = 0;


        if (playerID == 0) //Player 1
        {
            moveHorizontal = Input.GetAxis("P1Horizontal");
            moveVertical = Input.GetAxis("P1Vertical");
        }
        else //Player 2
        {
            moveHorizontal = Input.GetAxis("P2Horizontal");
            moveVertical = Input.GetAxis("P2Vertical");
        }

        //move
        rt.position += 5f * new Vector3(moveHorizontal, moveVertical, 0);


        //limit position to the bounds of the canvas
        float newX = Mathf.Min(canvasRT.sizeDelta.x, Mathf.Max(0, rt.position.x));
        float newY = Mathf.Min(canvasRT.sizeDelta.y, Mathf.Max(0, rt.position.y));
        rt.position = new Vector3(newX, newY, 0);
    }
}
