using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MMesh : MonoBehaviour
{

    protected List<Sprite> sprites;

    [SerializeField]
    internal SpriteSheet mSpriteSheet;

    MeshFilter mMeshFilter;
    MeshRenderer mMeshRender;
    UnityEngine.Mesh mMesh;

    public void Awake()
    {
        sprites = new List<Sprite>();

        mMesh = new UnityEngine.Mesh();
        mMesh.MarkDynamic();

        mMeshFilter = gameObject.AddComponent<MeshFilter>();
        mMeshFilter.mesh = mMesh;

        mMeshRender = gameObject.AddComponent<MeshRenderer>();
        mMeshRender.castShadows = false;
        mMeshRender.useLightProbes = false;
        mMeshRender.receiveShadows = false;
    }

    // Use this for initialization
    void Start()
    {
        mMeshRender.material = new Material(mSpriteSheet.Shader);
        mMeshRender.material.mainTexture = mSpriteSheet.Texture;
    }

    // Update is called once per frame
    void Update()
    {
        mMesh.Clear();

        List<Sprite.Vertex> tmpVertex = new List<Sprite.Vertex>();
        List<int> tmpIndices = new List<int>();

        foreach (Sprite s in sprites)
        {
            if (s.UpdateMeshComp())
            {
                foreach (Sprite.Vertex v in s.mVertex)
                {
                    tmpVertex.Add(v);
                }
                foreach (int i in s.mIndices)
                {
                    tmpIndices.Add(i);
                }
            }
        }

        mMesh.vertices = tmpVertex.Select(v => v.position).ToArray();
        mMesh.uv = tmpVertex.Select(v => v.uv).ToArray();
        mMesh.triangles = tmpIndices.ToArray();
        Debug.Log("Mesh info:" + mMesh.vertices.Length + " , " + mMesh.uv.Length + " , " + mMesh.triangles.Length);
    }

    public void addSprite(Sprite s)
    {
        sprites.Add(s);
    }
}
