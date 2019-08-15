using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

/// <summary>
/// This system uses reflection to have individual objects update themselves according to value.
/// This saves space in the UIController.
/// Currently has problems with reflection target objects.
/// 
/// Later abandoned in favor of enums because it is many magnitudes faster in terms of speed.
/// The drawback is that every dynamic text field has to be manually implemented in the enum.
/// </summary>
public class UITextUpdater : MonoBehaviour {
    //public Component source;
    //public string varName = string.Empty;

    private Text textComponent;
    public string prefix, suffix;
    public enum Type { None, Teacups, Gold};
    public Type type;

    private void Start()
    {
        textComponent = GetComponent<Text>();
        //UpdateTextWithStringValue();
        UpdateTextByEnum();
    }

    void UpdateTextWithStringValue()
    {
        //int newVar = 0;
        //newVar = (int)source.GetType().GetField(varName).GetValue(source);
        //textComponent.text = newVar.ToString();
        //Debug.Log("Variable value: " + newVar);
    }

    public void UpdateTextByEnum()
    {
        string middle = "";
        if (type == Type.Teacups)
        {
            middle = GameManager.instance.globalSaveData.teacupCount.ToString();
        }
        else
        if (type == Type.Gold)
        {
            middle = GameManager.instance.globalSaveData.money.ToString();
        }

        //combine the text bodies
        textComponent.text = prefix + middle + suffix;
    }
}
