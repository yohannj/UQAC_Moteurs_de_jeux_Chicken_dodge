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
        mMesh.Clear(); //Remove what we did for the last frame
        gameObjects.RemoveWhere(g => g == null); //Remove objects that no longer exist

        List<Vector3> childVertex = new List<Vector3>();
        List<Vector2> childUV = new List<Vector2>();

        //Add vertices and UV of all objects
        foreach (GameObject child in gameObjects)
        {
            Mesh childMesh = child.GetComponent<MeshFilter>().sharedMesh; //Get the Mesh of the child
            //Add vertices of the child mesh
            foreach (Vector3 v3 in childMesh.vertices)
            {
                childVertex.Add(child.transform.localPosition + v3);
            }
            //Add UVs of the child mesh
            foreach (Vector2 v2 in childMesh.uv)
            {
                childUV.Add(v2);
            }

        }

        mMesh.vertices = childVertex.ToArray(); //Update vertices of the mesh to display
        mMesh.uv = childUV.ToArray(); //Update UVs of the mesh to display

        List<int> childIndices = new List<int>();

        int offset = 0; //Vertices of a child mesh are added after vertices of previous child mesh. This offset allow one child to find its vertices
        foreach (GameObject child in gameObjects)
        {

            Mesh childMesh = child.GetComponent<MeshFilter>().sharedMesh; //Get the Mesh of the child
            //Add triangles of the child mesh, adding the offset to point the right vertices
            foreach (int i in childMesh.triangles)
            {
                childIndices.Add(i + 4 * offset);
            }
            ++offset; //Increment the offset for the next child

        }

        mMesh.triangles = childIndices.ToArray(); //Update triangles of the mesh to display
    }


    /**
     * Check child that were just added and put them in finalHash if their mesh had the time to be initialize. It avoid errors for trying to display an element that is not initialized yet.
     */
    public static void UpdateHashesOf(HashSet<GameObject> justAddedHash, HashSet<GameObject> finalHash)
    {
        foreach (GameObject go in justAddedHash)
        {
            if (go.GetComponent<MeshFilter>() != null)
            {
                finalHash.Add(go);
            }
        }

        justAddedHash.RemoveWhere(c => finalHash.Contains(c));
    }

    public static Sprite GetSpriteFromObject(GameObject toGetFrom)
    {
        Sprite s_toGetFrom = toGetFrom.GetComponent<Sprite>();

        if (s_toGetFrom == null)
            s_toGetFrom = toGetFrom.GetComponentInChildren<Sprite>();

        return s_toGetFrom;
    }
}
