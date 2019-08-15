using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class was written by Hellium and found on StackExchange. Free for use.
/// It extends the GetComponentsInChildren method to ignore grandchildren.
/// </summary>
public static class GameObjectExtensions
{
    public static T[] GetComponentsInDirectChildren<T>(this GameObject gameObject) where T : Component
    {
        List<T> components = new List<T>();
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            T component = gameObject.transform.GetChild(i).GetComponent<T>();
            if (component != null)
                components.Add(component);
        }

        return components.ToArray();
    }
}