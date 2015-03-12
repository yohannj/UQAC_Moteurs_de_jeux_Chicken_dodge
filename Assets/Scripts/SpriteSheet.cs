using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteSheet : MonoBehaviour {

	[SerializeField]
	TextAsset mDescription;

	[SerializeField]
	Texture2D mTexture;

	[SerializeField]
	Shader mShader;

	public IDictionary<string, SpriteDescription> Sprites { get; private set; }
	public Shader Shader { get { return mShader; } }
	public Texture2D Texture { get { return mTexture; } }

    HashSet<GameObject> spritesGO;
    MeshFilter mMeshFilter;
    MeshRenderer mMeshRender;
    UnityEngine.Mesh mMesh;

	public void Awake()
	{
		Sprites = ParseDescription( mDescription.text, mTexture );
        initMesh();
	}

    void Update()
    {
        mMesh.Clear();
        List<Vector3> childVertex = new List<Vector3>();
        List<Vector2> childUV = new List<Vector2>();
        foreach (GameObject child in spritesGO)
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
        foreach (GameObject child in spritesGO)
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

	static IDictionary<string, SpriteDescription> ParseDescription( string rawDescription, Texture2D texture )
	{
		float width = texture.width;
		float height = texture.height;

		var description = (IDictionary<string, object>) MiniJSON.Json.Deserialize( rawDescription );
		var framesDescr = (IDictionary<string, object>) description["frames"];

		var sprites = new Dictionary<string, SpriteDescription>();

		foreach ( var item in framesDescr )
		{
			var itemDescr = (IDictionary<string, object>) item.Value;
			var frameDescr = (IDictionary<string, object>) itemDescr["frame"];
			var sourceDescr = (IDictionary<string, object>) itemDescr["spriteSourceSize"];
			var sizeDescr = (IDictionary<string, object>) itemDescr["sourceSize"];
			var frame = ExtractRect( frameDescr );
			var source = ExtractRect( sourceDescr );
			frame.xMin /= width;
			frame.yMin /= height;
			frame.xMax /= width;
			frame.yMax /= height;

			sprites[item.Key] = new SpriteDescription {
				mUV = AdjustUVOrientation( frame ),
				mSpriteSize = ExtractSize( sizeDescr ),
				mPixelOffset = new Vector2( source.xMin, source.yMin ),
				mPixelSize = new Vector2( source.width, source.height )
			};
		}

		return sprites;
	}

	static Vector2 ExtractSize( IDictionary<string, object> descr )
	{
		return new Vector2 {
			x = (float)(long) descr["w"],
			y = (float)(long) descr["h"]
		};
	}

	static Rect ExtractRect( IDictionary<string, object> descr )
	{
		return new Rect {
			xMin = (float)(long) descr["x"],
			yMin = (float)(long) descr["y"],
			width = (float)(long) descr["w"],
			height = (float)(long) descr["h"]
		};
	}

	static Rect AdjustUVOrientation( Rect frame )
	{
		return new Rect {
			xMin = frame.xMin,
			yMin = 1.0f - frame.yMax,
			width = frame.width,
			height = frame.height
		};
	}

    void initMesh()
    {
        spritesGO = new HashSet<GameObject>();
        mMesh = new UnityEngine.Mesh();
        mMesh.MarkDynamic();

        mMeshFilter = gameObject.AddComponent<MeshFilter>();
        mMeshFilter.mesh = mMesh;

        mMeshRender = gameObject.AddComponent<MeshRenderer>();
        mMeshRender.castShadows = false;
        mMeshRender.useLightProbes = false;
        mMeshRender.receiveShadows = false;

        mMeshRender.material = new Material(mShader);
        mMeshRender.material.mainTexture = mTexture;
    }

    public void addSpriteGO(GameObject spriteGO)
    {
        spritesGO.Add(spriteGO);
    }
}
