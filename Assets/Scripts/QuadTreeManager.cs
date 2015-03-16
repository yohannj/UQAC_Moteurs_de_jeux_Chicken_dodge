using UnityEngine;
using System.Collections;

public class QuadTreeManager : MonoBehaviour {

    QuadTree quadTree;

    public Rect bounds;
    public float zAxis;

    void Awake()
    {
        quadTree = new QuadTree(0, bounds, zAxis);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        quadTree.Draw();
	}
}
