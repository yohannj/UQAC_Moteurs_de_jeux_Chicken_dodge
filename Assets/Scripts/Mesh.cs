using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Mesh : MonoBehaviour
{

    protected List<Sprite> sprites;

    [SerializeField]
    protected SpriteSheet mSpriteSheet;

    protected MeshFilter mMeshFilter;
    protected MeshRenderer mMeshRender;
    protected UnityEngine.Mesh mMesh;

    // Use this for initialization
    protected void Start()
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

        mMeshRender.material = new Material(mSpriteSheet.Shader);
        mMeshRender.material.mainTexture = mSpriteSheet.Texture;
    }

    // Update is called once per frame
    protected void Update()
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
    }

    public void addSprite(Sprite s)
    {
        sprites.Add(s);
    }
}
