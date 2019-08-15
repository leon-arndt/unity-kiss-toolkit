using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scriptable object is the basis for the data of every ingredient.
/// It contains fields for names, textures and plant stats.
/// </summary>
[CreateAssetMenu(fileName = "New IngredientData", menuName = "IngredientData", order = 3)]
public class IngredientData : ScriptableObject
{
    public Texture2D ingredientTexture;
    public string plantName;
    [TextArea(3, 10)]
    public string description;
    public string trait;
    public string flavor;
    public float moveSpeed = 3f;
    public float sleepTime = 1f;
    public float cookTime;
}
