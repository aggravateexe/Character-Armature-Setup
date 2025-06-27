using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColliderHandler : MonoBehaviour
{
    private class ColliderInfo
    {
        public float lossyScale;
        public float height; //Height of the collider
        public float width; //width of the collider
        public float capsuleColliderRadius;
        public Vector3 center, boxColliderSize;

        public ColliderInfo(GameObject start, GameObject end, float size, Dictionary<GameObject, int> points)
        {
            this.lossyScale = start.transform.lossyScale.x;
            this.height = Vector3.Distance(start.transform.position, end.transform.position) / this.lossyScale;
            this.width = Math.Abs(start.transform.position.x - end.transform.position.x) / this.lossyScale;
            this.center = new Vector3(0, height / 2, 0);

            this.capsuleColliderRadius = size / this.lossyScale;
            this.boxColliderSize = new Vector3(capsuleColliderRadius, height, capsuleColliderRadius);
        }
    }
    public enum ColliderType
    {
        None,
        Capsule,
        Box
    };

    public static void AddCollider(GameObject start, GameObject end, float size, ColliderType colType, Dictionary<GameObject, int> points)
    {
        switch (colType)
        {
            case ColliderType.Capsule:
                AddCapsuleCollider(start, end, size, points);
                break;
            case ColliderType.Box:
                AddBoxCollider(start, end, size, points);
                break;
            default:
                break;
        }
        points[start]++;
    }

    public static void AddCollider(List<GameObject> path, float size, ColliderType colType, Dictionary<GameObject, int> points)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            GameObject start = path[i], end = path[i + 1];
            AddCollider(start, end, size, colType, points);
        }
    }
    
    public static void AddCapsuleCollider(GameObject start, GameObject end, float size, Dictionary<GameObject, int> points)
    {
        if (start == null || end == null)
        {
            Debug.Log("Can't add collider without two positions to calculate distance.");
        }
        BoxCollider boxCol = start.GetComponent<BoxCollider>();
        if (boxCol != null)
        {
            DestroyImmediate(boxCol);
        }

        CapsuleCollider capCol = start.AcquireComponent<CapsuleCollider>();
        ColliderInfo colInfo = new ColliderInfo(start, end, size, points);

        capCol.center = colInfo.center;
        capCol.height = colInfo.height;

        if (points[start] > 0)
        {
            capCol.radius += colInfo.width;
        }
        else
        {
            capCol.radius = colInfo.capsuleColliderRadius;
        }
    }
    public static void AddBoxCollider(GameObject start, GameObject end, float size, Dictionary<GameObject, int> points)
    {
        if (start == null || end == null)
        {
            Debug.Log("Can't add collider without two positions to calculate distance.");
        }
        CapsuleCollider capCol = start.GetComponent<CapsuleCollider>();
        if (capCol != null)
        {
            DestroyImmediate(capCol);
        }

        BoxCollider boxCol = start.AcquireComponent<BoxCollider>();
        ColliderInfo colInfo = new ColliderInfo(start, end, size, points);

        boxCol.center = colInfo.center;
        if (points[start] > 0)
        {
            boxCol.size = new Vector3(boxCol.size.x + colInfo.width, colInfo.height, boxCol.size.z + colInfo.width);
        }
        else
        {
            boxCol.size = colInfo.boxColliderSize;
        }
    }
}
