using System;
using UnityEngine;

public class Rupee : Colliding
{
	[SerializeField]
	internal SpriteSheet mSpriteSheet;

	public void Start()
	{
        canBeCollided = true;

		var newSprite = gameObject.AddComponent<Sprite>();
		newSprite.mSpriteSheet = mSpriteSheet;
		newSprite.mIsAnimated = true;
		newSprite.mAnimWait = 45;
		newSprite.mFrameSkip = 2;
		switch ( UnityEngine.Random.Range( 0, 3 ) )
		{
			case 0:
				newSprite.mSpriteName = "G";
				break;
			case 1:
				newSprite.mSpriteName = "B";
				break;
			case 2:
				newSprite.mSpriteName = "R";
				break;
		}
        newSprite.renderer.enabled = false;

        GameObject.Find("QuadTreeManager").GetComponent<QuadTreeManager>().AddObject(gameObject);
	}
}