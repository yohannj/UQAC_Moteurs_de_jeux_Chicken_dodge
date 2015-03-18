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
        return true;
        //return other.GetComponent<Colliding>().canBeCollided == true;
    }

    bool isNearOf(Transform other)
    {
        //TODO use quadtree
        return true;
    }

    bool isBoundingBoxTouchingWith(Transform other)
    {
        Vector2 other_size = other.GetComponent<Sprite>().getSpriteSize();
        Vector2 my_size = GetComponent<Sprite>().getSpriteSize();

        //Circles
        Vector3 other_center = other.position;
        Vector3 my_center = transform.position;
        float other_radius = calcSquareDist(other_size.x / 2, other_size.y / 2);
        float my_radius = calcSquareDist(my_size.x / 2, my_size.y / 2);
        float dist_centers = calcSquareDist(other_center.x - my_center.x, other_center.y - my_center.y);

        if (dist_centers < other_radius + my_radius)
        {
            return true;
        }
        
        //Rectangles TODO


        return false;
    }

    bool isThereAVectorCrossingWith(Transform other)
    {
        //TODO
        return true;
    }

    float calcSquareDist(float dx, float dy)
    {
        return dx * dx + dy * dy;
    }
}
