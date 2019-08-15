using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cutscene", menuName = "Cutscene", order = 3)]
public class Cutscene : ScriptableObject {
    public CutsceneAction[] cutsceneActions;
}
