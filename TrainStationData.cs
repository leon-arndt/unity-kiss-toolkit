using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This scriptable objects holds the names for each of the different train station.
/// There is also a theme GameObject prefab which is instantiated when the train station is entered.
/// It is called Train Station Data because there were originally meant to be different customer stations.
/// </summary>
[CreateAssetMenu(fileName = "New Station", menuName = "Train Station", order = 3)]
public class TrainStationData : ScriptableObject {
    public string stationName;
    public GameObject customerHouse;
    public Vector3 housePosition;
    public Vector3 player1Position;
    public Vector3 player2Position;
    public float player1YRotation;
    public float player2YRotation;
    public float houseYRotation;
    public string customerSongName;
    public string weatherSoundName;

    //fog
    public Color fogColor;

    [Range(0f, 0.05f)]
    public float fogDensity;
}
