using UnityEngine;
using System.Collections;

public class Chicken : MonoBehaviour
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
		mPosition = transform.localPosition;
		mVelocity = ( mTarget - mPosition ).normalized * Random.Range( 2.0f, 5.0f );

		var newSprite = gameObject.AddComponent<Sprite>();
		newSprite.mSpriteSheet = mSpriteSheet;
		newSprite.mIsAnimated = true;
		newSprite.mSpriteName = "C" + ( mVelocity.x > 0 ? "R" : "L" );
        newSprite.renderer.enabled = false;
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
		var newRupeeObj = new GameObject();
        transform.parent.GetComponent<ChickenSpawner>().addRupee(newRupeeObj);
		var newRupee = newRupeeObj.AddComponent<Rupee>();
		newRupeeObj.transform.parent = gameObject.transform.parent;
		newRupeeObj.transform.localPosition = (Vector3) mTarget + Vector3.back * -10.1f;
		newRupee.mSpriteSheet = mSpriteSheet;
		mDropped = true;
	}
}
