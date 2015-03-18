using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuadTreeManager : MonoBehaviour {

    QuadTree quadTree;
    bool init = false; // Instead, use: "void Start()"?

    public Rect bounds;
    public float zAxis;
    public int maxObjects, maxLevels;

    List<GameObject> potentialInsertions = new List<GameObject>();

    void Awake()
    {
        quadTree = new QuadTree(0, maxObjects, maxLevels, bounds, zAxis);
    }

	void Update ()
    {
        if (!init)
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            for (int i = 0; i < allObjects.Length; i++)
            {
                if (allObjects[i].GetComponent<Colliding>() != null)
                    quadTree.insert(allObjects[i]);
            }

            init = true;
        }

        for (int i = 0; i < potentialInsertions.Count; i++ )
        {
            GameObject toCheck = potentialInsertions[i];

            Vector3 position = toCheck.transform.position;

            if (quadTree.isInBound(toCheck))
            {
                quadTree.insert(toCheck);
                potentialInsertions.Remove(toCheck);
            }
        }

        quadTree.cleanup();
        quadTree.Draw();

        Debug.Log(quadTree.ToString());
	}

    public void AddObject(GameObject toAdd)
    {
        potentialInsertions.Add(toAdd);
    }
}
