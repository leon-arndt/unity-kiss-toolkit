using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;// Required when using Event data.

public class UILanguageChanger : MonoBehaviour
{
    private Dropdown dropdown;

    // Use this for initialization
    void Start () {
        dropdown = GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(delegate
        {
            TaskOnClick();
        });
    }



    void TaskOnClick()
    {
        GameManager.instance.language = (GameManager.Language)dropdown.value;
    }
}
