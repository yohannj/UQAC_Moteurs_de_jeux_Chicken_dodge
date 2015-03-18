using UnityEngine;
using System.Collections;

public class QuadTreeManager : MonoBehaviour {

    QuadTree quadTree;
    bool init = false; // Instead, use: "void Start()"?

    public Rect bounds;
    public float zAxis;
    public int maxObjects, maxLevels;

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

        quadTree.cleanup();
        quadTree.Draw();

        Debug.Log(quadTree.ToString());
	}

    public void AddObject(GameObject toAdd)
    {
        quadTree.insert(toAdd);
    }
}
