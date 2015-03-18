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
        Vector2 other_center = (Vector2)other.position + (other_size / 2);
        Vector2 my_center = (Vector2)transform.position + (my_size / 2);
        float other_radius = calcDist(other_size.x / 2, other_size.y / 2);
        float my_radius = calcDist(my_size.x / 2, my_size.y / 2);
        float dist_centers = calcDist(other_center.x - my_center.x, other_center.y - my_center.y);

        if (dist_centers > other_radius + my_radius) //Circles not crossing
        {
            //Debug.Log(dist_centers + " | " + (other_radius + my_radius) + " | " + other_radius + " | " + my_radius + " | " + other.position + " | " + transform.position);
            return false;
        }
        
        //Rectangles TODO



        return true;
    }

    bool isThereAVectorCrossingWith(Transform other)
    {
        //TODO
        return true;
    }

    float calcDist(float dx, float dy)
    {
        return Mathf.Sqrt(dx * dx + dy * dy);
    }
}
