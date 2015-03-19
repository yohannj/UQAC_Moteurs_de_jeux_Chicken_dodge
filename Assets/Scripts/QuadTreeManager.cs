using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuadTreeManager : MonoBehaviour {

    QuadTree quadTree;
    bool init = false; // Instead, use: "void Start()"?

    public Rect bounds;
    public float zAxis;
    public int maxObjects, maxLevels;

	bool allInserted;

    List<GameObject> objects = new List<GameObject>();

    void Awake()
    {
        quadTree = new QuadTree(0, maxObjects, maxLevels, bounds, zAxis);
    }

	void Update ()
    {
		allInserted = false;
        /*if (!init)
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            for (int i = 0; i < allObjects.Length; i++)
            {
                if (allObjects[i].GetComponent<Colliding>() != null && quadTree.isInBound(allObjects[i]))
                    //quadTree.insert(allObjects[i]);
					objects.Insert (allObjects[i]);
            }

            init = true;
        }*/

		quadTree.clear ();

		//objects = FindObjectsOfType<GameObject>();

		//objects.RemoveAll(o => o == null);
		int added = 0;
		quadTree.initDebug();

		foreach(GameObject anObject in FindObjectsOfType<GameObject>()){
			if (anObject.GetComponent<Colliding>()) {
				quadTree.insert (anObject);
				++added;
			}
		}
		quadTree.shapeQuadTree();

		Debug.Log(added + " - " + quadTree.getNumObjects() + " - Debug: " + quadTree.readDebug());

        quadTree.Draw();

		allInserted = true;

        //Debug.Log(quadTree.ToString());
	}

    public void AddObject(GameObject toAdd)
    {
        objects.Add(toAdd);
    }

	public bool inSameRect(GameObject g1, GameObject g2)
	{
		return quadTree.inSameRect(g1, g2);
	}

	public bool isAllInserted() {
		return allInserted;
	}
}
