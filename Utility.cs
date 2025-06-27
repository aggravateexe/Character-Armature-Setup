using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    /**
     * Add/Gets component to object.
     * 
     * Returns Component being added/fetched.
    */
    public static T AcquireComponent<T>(this GameObject obj) where T : Component
    {
        T comp = obj.GetComponent<T>();
        if (comp == null)
        {
            comp = obj.AddComponent<T>();
        }
        return comp;
    }
}
