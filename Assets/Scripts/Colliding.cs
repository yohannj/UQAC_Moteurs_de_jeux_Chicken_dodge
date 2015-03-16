using UnityEngine;

public class Colliding : MonoBehaviour {

    protected bool canBeCollided = false;

    public bool CanBeCollided
    {
        get { return canBeCollided; }
    }
}
