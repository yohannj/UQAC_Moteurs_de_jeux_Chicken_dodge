using System.Collections.Generic;
using UnityEngine;

public class Colliding : MonoBehaviour
{

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
        return other.GetComponent<Colliding>() != null && other.GetComponent<Colliding>().canBeCollided;
    }

    bool isNearOf(Transform other)
    {
        //TODO use quad tree
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

        //Rectangles
        if (other.position.x < transform.position.x + my_size.x
            && transform.position.x < other.position.x + other_size.x
            && other.position.y < transform.position.y + my_size.y
            && transform.position.y < other.position.y + other_size.y) //Rectangles are crossing
        {
            return true;
        }



        return false;
    }

    bool isThereAVectorCrossingWith(Transform other)
    {
        Vector3[] other_vertices = other.GetComponent<Sprite>().mMesh.vertices;
        Vector3[] my_vertices = GetComponent<Sprite>().mMesh.vertices;
        int[] other_indices = other.GetComponent<Sprite>().mIndices;
        int[] my_indices = GetComponent<Sprite>().mIndices;

        for (int other_triangle_index = 0; other_triangle_index < other_indices.Length / 3; ++other_triangle_index)
        {
            //Cast Vector3 in Vector2. Contains p0, p1, p2 and p0 again. So that p0 -> p1 is a first segment, p1 -> p2 a second one, and p2 -> p0 the third one
            Vector2[] other_current_tri_points = { other_vertices[3 * other_triangle_index], other_vertices[3 * other_triangle_index + 1], other_vertices[3 * other_triangle_index + 2], other_vertices[3 * other_triangle_index] };
            for (int my_triangle_index = 0; my_triangle_index < my_indices.Length / 3; ++my_triangle_index)
            {
                //Cast Vector3 in Vector2. Contains p0, p1, p2 and p0 again. So that p0 -> p1 is a first segment, p1 -> p2 a second one, and p2 -> p0 the third one
                Vector2[] my_current_tri_points = { my_vertices[3 * my_triangle_index], my_vertices[3 * my_triangle_index + 1], my_vertices[3 * my_triangle_index + 2], my_vertices[3 * my_triangle_index] };

                //Check combination of all segments unless collision is found
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; i < 3; ++j)
                    {
                        if (segmentsIntersect(other_current_tri_points[i], other_current_tri_points[i + 1], my_current_tri_points[j], my_current_tri_points[j + 1]))
                        {
                            return true;
                        }
                    }
                }

            }
        }

        return false;
    }

    float calcDist(float dx, float dy)
    {
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    bool segmentsIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        //TODO
        return true;
    }
}
