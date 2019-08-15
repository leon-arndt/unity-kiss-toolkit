using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This simple UI component contains the Scene references for the UI Stats
/// </summary>
public class UIStats : MonoBehaviour {
    [SerializeField]
    private Text stepsText, plantsPottedText, teacupsCollectedText, totalDamageText;

    public void UpdateStatsText()
    {
        Statistics statistics = GameManager.instance.globalSaveData.stats;
        stepsText.text = statistics.stepsTaken.ToString() + " steps taken";
        plantsPottedText.text = statistics.totalPlantsPotted.ToString() + " plants potted";
        teacupsCollectedText.text = GameManager.instance.globalSaveData.teacupCount + " teacups collected";
        totalDamageText.text = Mathf.Round(statistics.totalDamageTaken) + " total damage to train";
    }
}
