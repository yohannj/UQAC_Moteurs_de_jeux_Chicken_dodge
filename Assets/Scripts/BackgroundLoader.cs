using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundLoader : Colliding
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



    void Awake()
    {
        canBeCollided = (gameObject.name == "1_PlayGround");
        myBackgrounds = new HashSet<GameObject>();
        SpriteBatching.Initialize(ref mMesh, ref mMeshFilter, ref mMeshRender, gameObject, mSpriteSheet);
    }

    public void Start()
    {
        float y = 0;
        foreach (var line in mDescription.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            float x = 0;
            foreach (var c in line)
            {
                if (EntryMap.ContainsKey(c))
                {
                    var entry = EntryMap[c];
                    var spriteGO = new GameObject();
                    myBackgrounds.Add(spriteGO);
                    spriteGO.AddComponent<DefaultCollider>();
                    spriteGO.name = "BackGround #"+myBackgrounds.Count;
                    if (transform.name == "1_PlayGround")
                    {
                        GameObject.Find("QuadTreeManager").GetComponent<QuadTreeManager>().AddObject(spriteGO);
                    }
                    var newSprite = spriteGO.AddComponent<Sprite>();
                    newSprite.mSpriteSheet = mSpriteSheet;
                    newSprite.mSpriteName = entry.spriteName;
                    newSprite.mIsAnimated = entry.isAnimated;
                    newSprite.mFrameSkip = entry.frameSkip;
                    newSprite.transform.parent = transform;
                    newSprite.transform.localPosition = new Vector3(x, y, y * 0.01f);
                    newSprite.renderer.enabled = false;
                }
                x += mScale;
            }
            y -= mScale;
        }
    }

    void Update()
    {
        SpriteBatching.UpdateMesh(ref mMesh, ref myBackgrounds);
    }
}
