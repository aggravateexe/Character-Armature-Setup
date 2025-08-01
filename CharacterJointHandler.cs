using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJointHandler
{
    public static void AddJoint(List<GameObject> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            GameObject start = path[i], end = path[i + 1];
            AddJoint(start, end);
        }
    }
    public static void AddJoint(GameObject start, GameObject end)
    {
        if (start == null || end == null)
        {
            Debug.Log("Can't joint together if one Joint is NULL.");
        }

        Rigidbody rbStart, rbEnd;
        rbStart = start.AcquireComponent<Rigidbody>();
        rbEnd = end.AcquireComponent<Rigidbody>();

        rbStart.useGravity = false;
        rbEnd.useGravity = false;
        rbStart.isKinematic = true;
        rbEnd.isKinematic = true;

        CharacterJoint jointEnd = end.AcquireComponent<CharacterJoint>();
        jointEnd.connectedBody = rbStart;
    }
}
