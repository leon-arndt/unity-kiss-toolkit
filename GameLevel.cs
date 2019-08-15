using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scriptable object is the basis for the data of every game levels.
/// The game levels in Tea on Rails are not stored in separate scenes but in these data containers.
/// This is a safer method of handling content generation which is less prone to leaking data.
/// </summary>
[CreateAssetMenu(fileName = "New Level", menuName = "Level", order = 3)]
public class GameLevel : ScriptableObject {
    public int levelNumber;
    public string levelName;
    public float levelTime;
    public TrainStationData destinationStation;
    public MechanicVideoData mechanicVideo;

    public GameController.SpecialRule specialRule;

    [Header("Ingredients")]
    public GameObject[] slotMachinePrefabs;
    public IngredientData[] desiredIngredients;

    [Header("Weather")]
    public WeatherManager.Weather[] weather;

    [Header("Balancing Factors")]
    [Range(0f, 3f)]
    public float vehicleDamageMultiplier = 1f;

    [Range(0f, 3f)]
    public float breakChanceMultiplier = 1f;

    //does this level have random equipment breaking?
    public bool randomBreaking, forcedBreaking;

    //is the coal Machine available?
    public bool coalMachineEnabled;

    [Header("Dialogue")]
    public bool showTutorial;
    [TextArea(3, 5)]
    public string missionText;
    public DialogueSpeaker customer;
    public DialogueScene customerReaction;
}