/* Implémentation du QuadTree inspiré du tutoriel
 * trouvé ici : http://gamedevelopment.tutsplus.com/tutorials/quick-tip-use-quadtrees-to-detect-likely-collisions-in-2d-space--gamedev-374
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class QuadTree {
    private int maxObjects;
    private int maxLevels;
    private Rect bounds;
    private float zAxis;
    private int level;
    private List<GameObject> objects;
    private QuadTree[] nodes;

    public QuadTree(int level, int maxObjects, int maxLevels, Rect bounds, float zAxis)
    {
        this.level = level;
        this.maxObjects = maxObjects;
        this.maxLevels = maxLevels;
        this.bounds = bounds;
        objects = new List<GameObject>();
        nodes = new QuadTree[4];
        this.zAxis = zAxis;
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

        nodes[0] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x + subWidth, y, subWidth, subHeight), zAxis);
        nodes[1] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x, y, subWidth, subHeight), zAxis);
        nodes[2] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x, y + subHeight, subWidth, subHeight), zAxis);
        nodes[3] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x + subWidth, y + subHeight, subWidth, subHeight), zAxis);
    }

    private int getIndex(GameObject toCheck)
    {
        Sprite s_toCheck = SpriteBatching.GetSpriteFromObject(toCheck);
        if (s_toCheck == null) throw new Exception(toCheck.name + " et ses enfants n'ont pas de Sprite!");

        int index = -1;
        double verticalMidpoint = bounds.x + (bounds.width / 2);
        double horizontalMidpoint = bounds.y - (bounds.height / 2);

        bool topQuadrant = (toCheck.transform.position.y > horizontalMidpoint && toCheck.transform.position.y + s_toCheck.SpriteSize.y > horizontalMidpoint);
        bool bottomQuadrant = (toCheck.transform.position.y < horizontalMidpoint);

        if (toCheck.transform.position.x < verticalMidpoint && toCheck.transform.position.x + s_toCheck.SpriteSize.x < verticalMidpoint)
        {
            if (topQuadrant) index = 1;
            else if (bottomQuadrant) index = 2;
        }
        else if (toCheck.transform.position.x > verticalMidpoint)
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

            //foreach (GameObject anObject in objects)
            for (int i = 0; i < objects.Count; i++)
            {
                int index = getIndex(objects[i]);
                if (index != -1)
                {
                    GameObject toInsert = objects[i];
                    objects.Remove(objects[i]);
                    nodes[index].insert(toInsert);
                }
            }
        }
    }

    public bool isEmpty()
    {
        return (objects.Count <= 0);
    }

    public void cleanup()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != null)
            {
                nodes[i].cleanup();

                if (nodes[i].isEmpty())
                    nodes[i] = null;
            }
        }

        objects.RemoveAll(item => item == null);
    }

    public List<GameObject> retrieve(List<GameObject> returnObjects, GameObject toCheck)
    {
        int index = getIndex(toCheck);

        if (index != -1 && nodes[0] != null)
            nodes[index].retrieve(returnObjects, toCheck);

        returnObjects.AddRange(objects);

        return returnObjects;
    }

    public void Draw()
    {
        Vector3 corner1 = new Vector3(bounds.x, bounds.y, zAxis),
                corner2 = new Vector3(bounds.x + bounds.width, bounds.y, zAxis),
                corner3 = new Vector3(bounds.x + bounds.width, bounds.y - bounds.height, zAxis),
                corner4 = new Vector3(bounds.x, bounds.y - bounds.height, zAxis);

        Debug.DrawLine(corner1, corner2);
        Debug.DrawLine(corner2, corner3);
        Debug.DrawLine(corner3, corner4);
        Debug.DrawLine(corner4, corner1);

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != null)
                nodes[i].Draw();
        }
    }
}