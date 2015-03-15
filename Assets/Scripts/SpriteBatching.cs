using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SpriteBatching {

	public static void Initialize(ref Mesh mMesh,
                                  ref MeshFilter mMeshFilter,
                                  ref MeshRenderer mMeshRender,
                                  GameObject go,
                                  SpriteSheet mSpriteSheet)
    {
        mMesh = new Mesh();
        mMesh.MarkDynamic();

        mMeshFilter = go.AddComponent<MeshFilter>();
        mMeshFilter.mesh = mMesh;

        mMeshRender = go.AddComponent<MeshRenderer>();
        mMeshRender.castShadows = false;
        mMeshRender.useLightProbes = false;
        mMeshRender.receiveShadows = false;

        mMeshRender.material = new Material(mSpriteSheet.Shader);
        mMeshRender.material.mainTexture = mSpriteSheet.Texture;
    }

    public static void UpdateMesh(ref Mesh mMesh,
                                  ref HashSet<GameObject> gameObjects)
    {
        mMesh.Clear();
        gameObjects.RemoveWhere(g => g == null);

        List<Vector3> childVertex = new List<Vector3>();
        List<Vector2> childUV = new List<Vector2>();
        foreach (GameObject child in gameObjects)
        {
            Mesh childMesh = child.GetComponent<MeshFilter>().sharedMesh;
            foreach (Vector3 v3 in childMesh.vertices)
            {
                childVertex.Add(child.transform.localPosition + v3);
            }
            foreach (Vector2 v2 in childMesh.uv)
            {
                childUV.Add(v2);
            }

        }

        mMesh.vertices = childVertex.ToArray();
        mMesh.uv = childUV.ToArray();

        List<int> childIndices = new List<int>();
        int index = 0;
        foreach (GameObject child in gameObjects)
        {

            Mesh childMesh = child.GetComponent<MeshFilter>().sharedMesh;
            foreach (int i in childMesh.triangles)
            {
                childIndices.Add(i + 4 * index);
            }
            ++index;

        }
        mMesh.triangles = childIndices.ToArray();
    }
}
