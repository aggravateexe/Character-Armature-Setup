using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;

public class CharacterArmature : MonoBehaviour
{
    /**
     * Node correlating to each point. Points will traverse upwards seperately from eachother.
    */
    private class Node
    {
        private GameObject start, end, current;
        public List<GameObject> path;

        public Node(GameObject start, GameObject end)
        {
            this.start = start;
            this.end = end;
            this.current = null;
            this.path = new List<GameObject>();
        }

        /**
         * Steps upward from parent to parent until we reach the end (top point)
         * 
         * returns a GameObject if we make a successful step, otherwise, return null when finished.
        */
        private GameObject Step()
        {
            if (current == end)
            {
                return null;
            }
            else if (current == null)
            {
                current = start;
            }
            else
            {
                current = current.transform.parent.gameObject;
            }

            return current;
        }
        /**
         * We traverse upwards from our given start to our end, filling up the path list as we go on.
        */
        public void Traverse(List<GameObject> points)
        {
            GameObject current;
            while ((current = Step()) != null)
            {
                path.Add(current);
                if (!points.Contains(current))
                {
                    points.Add(current);
                }
            }
        }
    }
    public CharacterColliderHandler.ColliderType colliderType;
    public float colliderSize = 1;

    public GameObject topPoint;
    public List<GameObject> endPoints;
    [HideInInspector]
    public List<GameObject> points;

    [ContextMenu("Build")]
    public void Build()
    {
        if (topPoint == null)
        {
            Debug.Log("Top Point is NULL, cannot build!");
            return;
        }
        else if (endPoints.Count < 1)
        {
            Debug.Log("There are no End Points to traverse with, cannot build!");
            return;
        }

        //Clearing points list for reusability.
        points.Clear();

        //Setting up nodes for each end point.
        List<Node> nodes = new List<Node>();
        foreach (GameObject endPoint in endPoints)
        {
            nodes.Add(new Node(endPoint, topPoint));
        }

        //We will now have all end points traverse upwards to the top point in order to get their respective paths.
        foreach (Node node in nodes)
        {
            node.Traverse(points);
        }

        //Applying scripts to points within a nodes path.
        foreach (Node node in nodes)
        {
            node.path.Reverse();
            CharacterColliderHandler.AddCollider(node.path, colliderSize, colliderType);
        }
    }
    [ContextMenu("Clean")]
    public void Clean()
    {
        if (points.Count < 1)
        {
            return;
        }
        foreach (GameObject point in points)
        {
            CleanPoint(point);
        }
    }
    public void CleanPoint(GameObject point)
    {
        Rigidbody rb = point.GetComponent<Rigidbody>();
        if (rb)
        {
            DestroyImmediate(rb);
        }
        CharacterJoint joint = point.GetComponent<CharacterJoint>();
        if (joint)
        {
            DestroyImmediate(joint);
        }
        CapsuleCollider capCol = point.GetComponent<CapsuleCollider>();
        if (capCol)
        {
            DestroyImmediate(capCol);
        }
        BoxCollider boxCol = point.GetComponent<BoxCollider>();
        if (boxCol)
        {
            DestroyImmediate(boxCol);
        }
    }
}
