using System; //Could cause problems with Unity Random functions
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator : MonoBehaviour
{
    public TextAsset csv;
    string[,] grid;
    string[] lines;

    // Use this for initialization
    void Start()
    {
        //CSVReader.DebugOutputGrid( CSVReader.SplitCsvGrid(csv.text) ); 
        //TranslateAllObjectsInScene();
        grid = CSVReader.SplitCsvGrid(csv.text);
        lines = csv.text.Split("\n"[0]);
    }

    public string GetTranslation(string stringID)
    {
        //Try to translate and catch exception if there is an error
        try {
            int lineIndex = CSVReader.GetStringIDLineIndex(grid, lines, stringID);
            //Debug.Log("The line index for " + stringID + " is thought to be" + lineIndex);
            string translatedText = CSVReader.GetTranslationForLineIndex(grid, lineIndex, (int)GameManager.instance.language);
            return translatedText;
        }
        catch (Exception e)
        {
            Debug.Log("Error: The correct translation line index could not be found");
            return stringID;
        }
    }

    //very unexpensive method, not currently used
    public void TranslateAllObjectsInScene()
    {
        Debug.Log("Trying to translate all objects in scene");

        Translate[] translateObjects = FindObjectsOfType<Translate>();
        foreach (Translate translateObject in translateObjects)
        {
            //Debug.Log("Tried translating an object");
            translateObject.TranslateText();
        }
    }
}
