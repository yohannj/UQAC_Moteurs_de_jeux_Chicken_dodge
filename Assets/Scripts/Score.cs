using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	[SerializeField]
	TextSprite mScoreSprite;

    [SerializeField]
    SpriteSheet mSpriteSheet;

	int _value = 0;
	public int Value
	{
		get {
			return _value;
		}

		set {
			_value = value;
			mScoreSprite.Text = Value.ToString();
		}
	}

	public void Start()
	{
		Value = Value;
        var face_GO = transform.FindChild("Face").gameObject;
        var face_newSprite = face_GO.AddComponent<Sprite>();
        face_newSprite.mSpriteSheet = mSpriteSheet;
        face_newSprite.mSpriteName = gameObject.name == "GreenScore" ? "GI" : "RI";
        face_newSprite.mIsAnimated = false;
        face_newSprite.mFrameSkip = 2;
        face_newSprite.transform.parent = transform;
        face_newSprite.transform.localPosition = face_GO.transform.position;
        face_newSprite.renderer.enabled = false;
        transform.parent.GetComponent<MeshRegrouper>().add_GO_to_display(face_GO);

        var rupee_GO = transform.FindChild("Rupee").gameObject;
        var rupee_newSprite = rupee_GO.AddComponent<Sprite>();
        rupee_newSprite.mSpriteSheet = mSpriteSheet;
        rupee_newSprite.mSpriteName = "G";
        rupee_newSprite.mIsAnimated = true;
        rupee_newSprite.mFrameSkip = 2;
        rupee_newSprite.mAnimWait = 90;
        rupee_newSprite.transform.parent = transform;
        rupee_newSprite.transform.localPosition = rupee_GO.transform.position;
        rupee_newSprite.renderer.enabled = false;
        transform.parent.GetComponent<MeshRegrouper>().add_GO_to_display(rupee_GO);
	}
}