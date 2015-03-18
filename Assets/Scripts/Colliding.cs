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
            if (isPossibleCollision(child) && isNearOf(child) && isBoundingBoxTouchingWith(child) && isThereAVectorCrossingWith(child)) //4 level of collision detection
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

    bool isNearOf(Transform other)
    {
        //TODO use quadtree
        return false;
    }

    bool isBoundingBoxTouchingWith(Transform other)
    {
        //TODO
        return false;
    }

    bool isThereAVectorCrossingWith(Transform other)
    {
        //TODO
        return false;
    }
}
