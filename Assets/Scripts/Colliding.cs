using System.Collections.Generic;
using UnityEngine;

public class Colliding : MonoBehaviour {

    protected bool canBeCollided = false;

    public bool CanBeCollided
    {
        get { return canBeCollided; }
    }

    public List<GameObject> IsCollidingWith(GameObject parent)
    {
        List<GameObject> res = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            if (isPossibleCollision(child)) //TODO: Complete
            {
                res.Add(child.gameObject);
            }
        }
        return res;
    }

    bool isPossibleCollision(Transform other)
    {
        return other.GetComponent<Colliding>().canBeCollided == true;
    }
}
