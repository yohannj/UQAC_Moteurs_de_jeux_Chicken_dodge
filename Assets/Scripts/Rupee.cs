using System;
using UnityEngine;

public class Rupee : MonoBehaviour
{
	[SerializeField]
	internal SpriteSheet mSpriteSheet;

	public void Start()
	{
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
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Mesh>().addSprite(newSprite);
	}
}