using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Colliding
{
	enum FacingType { Up, Down, Left, Right };

	static readonly IDictionary<FacingType, string> FacingSpriteCode = new Dictionary<FacingType, string> {
		{FacingType.Up, "B"},
		{FacingType.Down, "F"},
		{FacingType.Left, "L"},
		{FacingType.Right, "R"},
	};

	[SerializeField]
	SpriteSheet mSpriteSheet;

	[SerializeField]
	string mPrefix;

	[SerializeField]
	Score mScore;

	Sprite mSprite;
	FacingType mFacing = FacingType.Down;
	bool mIsAttacking;
	bool mIsMoving;

	public void Start ()
	{
        canBeCollided = true;

		mSprite = gameObject.AddComponent<Sprite>();
        transform.parent.GetComponent<MeshRegrouper>().add_GO_to_display(gameObject);//GameObject.Find("Layers").GetComponent<HUDandPlayersMesh>().add_GO_to_display(gameObject);
		mSprite.mSpriteSheet = mSpriteSheet;
		mSprite.AnimationEndedEvent += delegate {
			mIsAttacking = false;
			mSprite.mFrameSkip = 2;
			UpdateSprite();
			mSprite.UpdateMesh();
		};
		UpdateSprite();

        mSprite.renderer.enabled = false;
	}
	
	public void Update ()
	{
		if ( mIsAttacking == false && Input.GetKeyDown( KeyCode.Space ) )
		{
			mIsAttacking = true;
			mSprite.mAnimationFrame = 1;
			mSprite.mFrameSkip = 1;

            tryKillChiken();
		}

		Vector2 delta = Vector2.zero;
		if ( Input.GetKey( KeyCode.UpArrow ) )
		{
            delta += TryGo(Vector2.up);
			mFacing = FacingType.Up;
		}
		if ( Input.GetKey( KeyCode.DownArrow ) )
		{
			delta += TryGo(-Vector2.up);
			mFacing = FacingType.Down;
		}
		if ( Input.GetKey( KeyCode.LeftArrow ) )
		{
			delta += TryGo(-Vector2.right);
			mFacing = FacingType.Left;
		}
		if ( Input.GetKey( KeyCode.RightArrow ) )
		{
			delta += TryGo(Vector2.right);
			mFacing = FacingType.Right;
		}

		mIsMoving = ( delta != Vector2.zero );

		transform.Translate( delta * 3.0f );

		UpdateSprite();
	}

	void UpdateSprite()
	{
		mSprite.mIsAnimated = mIsMoving || mIsAttacking;
		mSprite.mSpriteName = string.Format( "{0}{1}{2}{3}",
			mPrefix,
			mIsAttacking ? "A" : "M",
			FacingSpriteCode[mFacing],
			mSprite.mIsAnimated ? "" : "1"
		);
	}

    Vector2 TryGo(Vector2 direction)
    {
        Vector2 res = Vector2.zero;
        transform.Translate(direction * 3.0f); //translate
        bool is_colliding = IsCollidingWith(GameObject.Find("1_PlayGround")).Count > 0;
        if (!is_colliding)
        {
            res = direction;

            int nb_rupee_collided = 0;
            foreach (GameObject rupee in IsCollidingWith(GameObject.Find("3.1_Rupee")))
            {
                ++nb_rupee_collided;
                Destroy(rupee);
            }
            mScore.addScore(nb_rupee_collided);
        }
        transform.Translate(-direction * 3.0f); //remove translation
        return res;
    }

    void tryKillChiken()
    {
        foreach (GameObject chicken in IsCollidingWith(GameObject.Find("3_Chicken")))
        {
            Destroy(chicken);
        }
    }
}