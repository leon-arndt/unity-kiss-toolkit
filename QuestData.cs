using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// QuestData is used by the Station Guide to inform the player at certain times:
/// For example, to track progress for major milestones.
/// It inherits from SaveData and is employed by the global save data field of the Game Manager.
/// </summary>
[System.Serializable]
public class QuestData : SaveData {
    public bool tutorialComplete;
    public bool hatManUnlocked;
    public bool tierBUnlocked;
    public bool tierCUnlocked;
}
