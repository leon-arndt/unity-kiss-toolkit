using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The statistics system saves various misc information for fun :)
/// It inherits from SaveData and is employed by the global save data field of the Game Manager.
/// </summary>
[System.Serializable]
public class Statistics : SaveData {
    public int stepsTaken = 0;
    public float totalDamageTaken = 0f;
    public int totalPlantsPotted = 0;
}
