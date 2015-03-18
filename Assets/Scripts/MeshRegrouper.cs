using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshRegrouper : MonoBehaviour
{

    [SerializeField]
    SpriteSheet mSpriteSheet;

    HashSet<GameObject> GOs;
    MeshFilter mMeshFilter;
    MeshRenderer mMeshRender;
    UnityEngine.Mesh mMesh;

    void Awake()
    {
        GOs = new HashSet<GameObject>();
        SpriteBatching.Initialize(ref mMesh, ref mMeshFilter, ref mMeshRender, gameObject, mSpriteSheet);
    }

    // Update is called once per frame
    void Update()
    {
        SpriteBatching.UpdateMesh(ref mMesh, ref GOs);
    }

    public void add_GO_to_display(GameObject go)
    {
        GOs.Add(go);
    }
}
