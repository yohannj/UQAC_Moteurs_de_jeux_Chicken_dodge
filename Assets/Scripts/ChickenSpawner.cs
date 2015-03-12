using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChickenSpawner : MonoBehaviour
{
    [SerializeField]
    SpriteSheet mSpriteSheet;

    [SerializeField]
    Rect mSourceArea;

    [SerializeField]
    Rect mTargetArea;

    [SerializeField]
    float mSpawnWaitFactor = 1.0f;

    float mSpawnDelay = 30.0f;

    HashSet<GameObject> chickensJustAdded;
    HashSet<GameObject> chickens;
    MeshFilter mMeshFilter;
    MeshRenderer mMeshRender;
    UnityEngine.Mesh mMesh;

    void Awake()
    {
        chickensJustAdded = new HashSet<GameObject>();
        chickens = new HashSet<GameObject>();
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

    public void Start()
    {
        Spawn();
    }

    public void Update()
    {
        var spawnDelay = (int)mSpawnDelay;
        if ((Time.frameCount % spawnDelay) == 0)
        {
            Spawn();
            mSpawnDelay = Mathf.Max(8.0f, mSpawnDelay * mSpawnWaitFactor);
        }
        UpdateChickensHashes();
        UpdateMesh();
    }

    void Spawn()
    {
        var newChickenObj = new GameObject();
        chickensJustAdded.Add(newChickenObj);
        var newChicken = newChickenObj.AddComponent<Chicken>();
        newChicken.mSpriteSheet = mSpriteSheet;
        newChickenObj.transform.parent = gameObject.transform;

        Vector2 pos = Vector2.zero;

        switch (Random.Range(0, 4))
        {
            case 0:
                pos = new Vector2(Random.Range(mSourceArea.xMin, mSourceArea.xMax), mSourceArea.yMin);
                break;
            case 1:
                pos = new Vector2(Random.Range(mSourceArea.xMin, mSourceArea.xMax), mSourceArea.yMax);
                break;
            case 2:
                pos = new Vector2(mSourceArea.xMin, Random.Range(mSourceArea.yMin, mSourceArea.yMax));
                break;
            case 3:
                pos = new Vector2(mSourceArea.xMax, Random.Range(mSourceArea.yMin, mSourceArea.yMax));
                break;
        }

        newChicken.transform.localPosition = pos;
        newChicken.mTarget = new Vector2(Random.Range(mTargetArea.xMin, mTargetArea.xMax), Random.Range(mTargetArea.yMin, mTargetArea.yMax));
    }

    void UpdateChickensHashes()
    {
        foreach (GameObject go in chickensJustAdded)
        {
            if (go.GetComponent<MeshFilter>() != null)
            {
                chickens.Add(go);
            }
        }

        chickensJustAdded.RemoveWhere(c => chickens.Contains(c));
    }

    void UpdateMesh()
    {
        mMesh.Clear();
        chickens.RemoveWhere(c => c == null);

        List<Vector3> childVertex = new List<Vector3>();
        List<Vector2> childUV = new List<Vector2>();
        foreach (GameObject child in chickens)
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
        foreach (GameObject child in chickens)
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

    public void addRupee(GameObject rupeeGO)
    {
        //TODO
    }

}
