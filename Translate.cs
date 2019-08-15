using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translate : MonoBehaviour
{
    [SerializeField]
    string stringID;

    // Use this for initialization
    void Start()
    {
        TranslateText();
    }

    public void TranslateText()
    {
        GetComponent<Text>().text = GameManager.instance.GetComponent<Translator>().GetTranslation(stringID);
    }
}
