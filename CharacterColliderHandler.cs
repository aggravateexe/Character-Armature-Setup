using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColliderHandler : MonoBehaviour
{
    private class ColliderInfo
    {
        public float scale; //Scale of the bone since armature's have their own scaling.
        public float height; //Height of the collider
        public float capsuleColliderRadius;
        public Vector3 center, boxColliderSize;

        public ColliderInfo(GameObject start, GameObject end, float size)
        {
            this.scale = start.transform.lossyScale.x;
            this.height = Vector3.Distance(start.transform.position, end.transform.position) / this.scale;
            this.center = new Vector3(0, height / 2, 0);

            this.capsuleColliderRadius = size / this.scale;
            this.boxColliderSize = new Vector3(size/this.scale, height, size/this.scale);
        }
    }
    public enum ColliderType
    {
        None,
        Capsule,
        Box
    };

    public static void AddCollider(GameObject start, GameObject end, float size, ColliderType colType)
    {
        switch (colType)
        {
            case ColliderType.Capsule:
                AddCapsuleCollider(start, end, size);
                break;
            case ColliderType.Box:
                AddBoxCollider(start, end, size);
                break;
            default:
                break;
        }
    }

    public static void AddCollider(List<GameObject> points, float size, ColliderType colType)
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            AddCollider(points[i], points[i + 1], size, colType);
        }
    }
    
    public static void AddCapsuleCollider(GameObject start, GameObject end, float size)
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
        ColliderInfo colInfo = new ColliderInfo(start, end, size);

        capCol.center = colInfo.center;
        capCol.height = colInfo.height;
        capCol.radius = colInfo.capsuleColliderRadius;
    }
    public static void AddBoxCollider(GameObject start, GameObject end, float size)
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
        ColliderInfo colInfo = new ColliderInfo(start, end, size);

        boxCol.center = colInfo.center;
        boxCol.size =colInfo.boxColliderSize;
    }
}
