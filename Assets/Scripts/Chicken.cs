using UnityEngine;
using System.Collections;

public class Chicken : Colliding
{
	[SerializeField]
	internal SpriteSheet mSpriteSheet;

	internal Vector2 mTarget;

	Vector2 mVelocity;
	Vector2 mPosition;
	float mDistance = 0.0f;
	bool mDropped;

	public void Start()
	{
        canBeCollided = true;

		mPosition = transform.localPosition;
		mVelocity = ( mTarget - mPosition ).normalized * Random.Range( 2.0f, 5.0f );

		var newSprite = gameObject.AddComponent<Sprite>();
		newSprite.mSpriteSheet = mSpriteSheet;
		newSprite.mIsAnimated = true;
		newSprite.mSpriteName = "C" + ( mVelocity.x > 0 ? "R" : "L" );
        newSprite.renderer.enabled = false;

        GameObject.Find("QuadTreeManager").GetComponent<QuadTreeManager>().AddObject(gameObject);
	}
	
	public void Update()
	{
		var targetDistanceSq = ( mTarget - mPosition ).sqrMagnitude;
		mPosition += mVelocity;
		var newTargetDistanceSq = ( mTarget - mPosition ).sqrMagnitude;
		if ( mDropped == false && newTargetDistanceSq > targetDistanceSq )
			Drop();

		transform.localPosition = mPosition;
		mDistance += mVelocity.magnitude;
		if ( mDistance > 1000.0f )
			GameObject.Destroy( gameObject );
	}

	public void Drop()
	{
        transform.parent.GetChild(0).GetComponent<Rupees>().addRupee((Vector3)mTarget);
        mDropped = true;
	}
}
