/* Implémentation du QuadTree inspiré du tutoriel
 * trouvé ici : http://gamedevelopment.tutsplus.com/tutorials/quick-tip-use-quadtrees-to-detect-likely-collisions-in-2d-space--gamedev-374
 */

/*

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuadTree {
    private const int maxObjects = 10;
    private const int maxLevels = 5;
    private Rect bounds;

    private int level;
    private List<GameObject> objects;
    private QuadTree[] nodes;

    public QuadTree(int level, Rect bounds)
    {
        this.level = level;
        this.bounds = bounds;
        objects = new List<GameObject>();
        nodes = new QuadTree[4];
    }

    public void clear()
    {
        objects.Clear();

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != null)
            {
                nodes[i].clear();
                nodes[i] = null;
            }
        }
    }

    private void split()
    {
        int subWidth = (int)(bounds.width / 2);
        int subHeight = (int)(bounds.height / 2);
        int x = (int)bounds.x;
        int y = (int)bounds.y;

        nodes[0] = new QuadTree(level + 1, new Rect(x + subWidth, y, subWidth, subHeight));
        nodes[1] = new QuadTree(level + 1, new Rect(x, y, subWidth, subHeight));
        nodes[2] = new QuadTree(level + 1, new Rect(x, y + subHeight, subWidth, subHeight));
        nodes[3] = new QuadTree(level + 1, new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
    }

    private int getIndex(GameObject toCheck)
    {
        int index = -1;
        double verticalMidpoint = bounds.x + (bounds.width / 2);
        double horizontalMidpoint = bounds.y + (bounds.height / 2);

        bool topQuadrant = (pRect.y < horizontalMidpoint && pRect.y + pRect.height < horizontalMidpoint);
        bool bottomQuadrant = (pRect.y > horizontalMidpoint);

        if (pRect.x < verticalMidpoint && pRect.x + pRect.width < verticalMidpoint)
        {
            if (topQuadrant) index = 1;
            else if (bottomQuadrant) index = 2;
        }
        else if (pRect.x > verticalMidpoint)
        {
            if (topQuadrant) index = 0;
            else if (bottomQuadrant) index = 3;
        }

        return index;
    }

    public void insert(GameObject newObject)
    {
        if (nodes[0] != null)
        {
            int index = getIndex(newObject);

            if (index != -1)
            {
                nodes[index].insert(newObject);
                return;
            }
        }

        objects.Add(newObject);

        if (objects.Count > maxObjects && level < maxLevels)
        {
            if (nodes[0] == null) split();

            foreach (GameObject anObject in objects)
            {
                int index = getIndex(anObject);
                if (index != 1)
                {
                    objects.Remove(anObject);
                    nodes[index].insert(anObject);
                }
            }
        }
    }

    public List<GameObject> retrieve(List<GameObject> returnObjects, GameObject toCheck)
    {
        int index = getIndex(toCheck);

        if (index != -1 && nodes[0] != null)
            nodes[index].retrieve(returnObjects, toCheck);

        returnObjects.AddRange(objects);

        return returnObjects;
    }
}
*/