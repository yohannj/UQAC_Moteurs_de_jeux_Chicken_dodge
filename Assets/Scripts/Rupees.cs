using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rupees : MonoBehaviour {

    [SerializeField]
    SpriteSheet mSpriteSheet;

    HashSet<GameObject> rupeesJustAdded;
    HashSet<GameObject> rupees;
    MeshFilter mMeshFilter;
    MeshRenderer mMeshRender;
    UnityEngine.Mesh mMesh;

    void Awake()
    {
        rupeesJustAdded = new HashSet<GameObject>();
        rupees = new HashSet<GameObject>();
        SpriteBatching.Initialize(ref mMesh, ref mMeshFilter, ref mMeshRender, gameObject, mSpriteSheet);
    }

	// Update is called once per frame
	void Update () {
        SpriteBatching.UpdateHashesOf(rupeesJustAdded, rupees);
        SpriteBatching.UpdateMesh(ref mMesh, ref rupees);
	}

    public void addRupee(Vector3 mTarget)
    {
        var newRupeeObj = new GameObject();
        rupeesJustAdded.Add(newRupeeObj);
        var newRupee = newRupeeObj.AddComponent<Rupee>();
        newRupeeObj.transform.parent = gameObject.transform.parent;
        newRupeeObj.transform.localPosition = mTarget + Vector3.back * -10.1f;
        newRupee.mSpriteSheet = mSpriteSheet;
    }

    
}
