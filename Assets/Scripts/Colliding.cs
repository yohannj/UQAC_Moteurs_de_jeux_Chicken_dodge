using System.Collections.Generic;
using UnityEngine;

public class Colliding : MonoBehaviour
{

    protected bool canBeCollided = false;

    public bool CanBeCollided
    {
        get { return canBeCollided; }
    }

    public List<GameObject> collidingWith(GameObject parent)
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

    public bool isCollidingWith(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            if (isPossibleCollision(child) && isNearOf(child) && isBoundingBoxTouchingWith(child) && isThereAVectorCrossingWith(child)) //4 level of collision detection
            {
                return true;
            }
        }
        return false;
    }

    bool isPossibleCollision(Transform other)
    {
        return other.GetComponent<Colliding>() != null && other.GetComponent<Colliding>().canBeCollided;
    }

    bool isNearOf(Transform other)
    {
        return true; //Enlever le commentaire pour une meilleure fluidité
        //return GameObject.Find("QuadTreeManager").GetComponent<QuadTreeManager>().inSameRect(gameObject, other.gameObject); //Commenter pour une meilleure fluidité
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
        Vector2? intersection_point = LineIntersectionPoint(p1, p2, p3, p4);
        if (intersection_point == null)
            return false;

        float min_x_1 = Mathf.Min(p1.x, p2.x);
        float min_x_2 = Mathf.Min(p3.x, p4.x);
        float max_x_1 = Mathf.Max(p1.x, p2.x);
        float max_x_2 = Mathf.Max(p3.x, p4.x);

        float min_y_1 = Mathf.Min(p1.y, p2.y);
        float min_y_2 = Mathf.Min(p3.y, p4.y);
        float max_y_1 = Mathf.Min(p1.y, p2.y);
        float max_y_2 = Mathf.Min(p3.y, p4.y);

        float x = ((Vector2)intersection_point).x;
        float y = ((Vector2)intersection_point).y;

        //Check if the intersection point is on both segment
        if(min_x_1 <= x && x <= max_x_1
            && min_x_2 <= x && x <= max_x_2
            && min_y_1 <= y && y <= max_y_1
            && min_y_2 <= y && y <= max_y_2)
        {
            return true;
        }

        return false;
    }

    //Source : mas, posted on http://www.wyrmtale.com/blog/2013/115/2d-line-intersection-in-c as seen on 2015-03-18 13:10
    //Determined the method is Cramer's rule and verified the code correctly follow it
    //Modified case delta == 0 to null instead of an exception (there are no intersection, as say Cramer's rule). Modified return type to Vector2? instead of Vector2 to be able to return null.
    Vector2? LineIntersectionPoint(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
    {
        // Get A,B,C of first line - points : ps1 to pe1
        float A1 = pe1.y - ps1.y;
        float B1 = ps1.x - pe1.x;
        float C1 = A1 * ps1.x + B1 * ps1.y;

        // Get A,B,C of second line - points : ps2 to pe2
        float A2 = pe2.y - ps2.y;
        float B2 = ps2.x - pe2.x;
        float C2 = A2 * ps2.x + B2 * ps2.y;

        // Get delta and check if the lines are parallel
        float delta = A1 * B2 - A2 * B1;
        if (delta == 0)
            return null;

        // now return the Vector2 intersection point
        return new Vector2(
            (B2 * C1 - B1 * C2) / delta,
            (A1 * C2 - A2 * C1) / delta
        );
    }
}
