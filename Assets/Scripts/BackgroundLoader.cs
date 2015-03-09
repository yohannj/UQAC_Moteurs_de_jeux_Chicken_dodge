using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundLoader : MonoBehaviour
{
	class EntryDetails
	{
		public string spriteName;
		public bool isAnimated;
		public int frameSkip;
	}

	static readonly IDictionary<char, EntryDetails> EntryMap = new Dictionary<char, EntryDetails> {
		{ 'T', new EntryDetails{ spriteName = "T1", isAnimated = false, frameSkip = 1 } },
		{ 'F', new EntryDetails{ spriteName = "F", isAnimated = true, frameSkip = 8 } },
		{ 'G', new EntryDetails{ spriteName = "P1", isAnimated = false, frameSkip = 1 } },
		{ '\\', new EntryDetails{ spriteName = "FE2", isAnimated = false, frameSkip = 1 } },
		{ '-', new EntryDetails{ spriteName = "FE1", isAnimated = false, frameSkip = 1 } },
		{ '|', new EntryDetails{ spriteName = "FE3", isAnimated = false, frameSkip = 1 } }
	};

	[SerializeField]
	TextAsset mDescription;

	[SerializeField]
	SpriteSheet mSpriteSheet;

	[SerializeField]
	float mScale = 16.0f;


    HashSet<GameObject> myBackgrounds;
    MeshFilter mMeshFilter;
    MeshRenderer mMeshRender;
    UnityEngine.Mesh mMesh;

	public void Start ()
	{
        myBackgrounds = new HashSet<GameObject>();
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


		float y = 0;
		foreach ( var line in mDescription.text.Split(new[]{'\n','\r'}, System.StringSplitOptions.RemoveEmptyEntries) )
		{
			float x = 0;
			foreach ( var c in line )
			{
				if ( EntryMap.ContainsKey( c ) )
				{
					var entry = EntryMap[c];
					var spriteGO = new GameObject();
                    myBackgrounds.Add(spriteGO);
					var newSprite = spriteGO.AddComponent<Sprite>();
					newSprite.mSpriteSheet = mSpriteSheet;
					newSprite.mSpriteName = entry.spriteName;
					newSprite.mIsAnimated = entry.isAnimated;
					newSprite.mFrameSkip = entry.frameSkip;
					newSprite.transform.parent = transform;
					newSprite.transform.localPosition = new Vector3( x, y, y * 0.01f );
                    newSprite.renderer.enabled = false;
				}
				x += mScale;
			}
			y -= mScale;
		}
	}

    void Update()
    {
        mMesh.Clear();
        List<Vector3> childVertex = new List<Vector3>();
        List<Vector2> childUV = new List<Vector2>();
        List<int> childIndices = new List<int>();
        foreach(GameObject child in myBackgrounds) {
            Mesh childMesh = child.GetComponent<Sprite>().mMesh;
            Debug.Log(child.transform.localPosition);
            foreach (Vector3 v3 in childMesh.vertices)
            {
                childVertex.Add(child.transform.localPosition + v3);
            }
            foreach (Vector2 v2 in childMesh.uv)
            {
                childUV.Add(v2);
            }
            foreach (int i in childMesh.triangles)
            {
                childIndices.Add(i);
            }
        }

        mMesh.vertices = childVertex.ToArray();
        mMesh.uv = childUV.ToArray();
        mMesh.triangles = childIndices.ToArray();
        Debug.Log("mMesh info: " + mMesh.vertices.Length + ", " + mMesh.uv.Length + ", " + mMesh.triangles.Length);
    }
}
