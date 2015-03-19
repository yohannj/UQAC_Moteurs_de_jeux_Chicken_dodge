using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class QuadTreeManager : MonoBehaviour {

    QuadTree quadTree;

    public Rect bounds;
    public float zAxis;
    public int maxObjects, maxLevels;


    HashSet<GameObject> objects = new HashSet<GameObject>();

    void Awake()
    {
        quadTree = new QuadTree(0, maxObjects, maxLevels, bounds, zAxis);
    }

	void Update ()
    {
/*        Stopwatch sw = new Stopwatch();*/
        

		quadTree.init ();
        
		foreach(GameObject anObject in objects){
			if (anObject.GetComponent<Colliding>()) {
				quadTree.insert (anObject);
			}
		}
        
        objects = new HashSet<GameObject>();
        /*sw.Start();*/
		quadTree.shapeQuadTree();
        /*sw.Stop();*/

        quadTree.Draw();

        
//         long nanoseconds = sw.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L));
//         UnityEngine.Debug.Log(nanoseconds);
	}

    public void AddObject(GameObject toAdd)
    {
        objects.Add(toAdd);
    }

	public bool inSameRect(GameObject g1, GameObject g2)
	{
		return quadTree.inSameRect(g1, g2);
	}
}
