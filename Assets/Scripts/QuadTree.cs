﻿/* Implémentation du QuadTree inspiré du tutoriel
 * trouvé ici : http://gamedevelopment.tutsplus.com/tutorials/quick-tip-use-quadtrees-to-detect-likely-collisions-in-2d-space--gamedev-374
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class QuadTree {
    private int maxObjects;
    private int maxLevels;

    private int level;
    private List<Vector3> previousPositions;
    private List<GameObject> objects;
    private Rect bounds;
    private QuadTree[] nodes;

    private float zAxis;

    public QuadTree(int level, int maxObjects, int maxLevels, Rect bounds, float zAxis)
    {
        this.level = level;
        this.maxObjects = maxObjects;
        this.maxLevels = maxLevels;
        this.bounds = bounds;
        objects = new List<GameObject>();
        previousPositions = new List<Vector3>();
        nodes = new QuadTree[4];
        this.zAxis = zAxis;
    }

    public void clear()
    {
        objects.Clear();
        previousPositions.Clear();

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
        float subWidth = bounds.width / 2f;
        float subHeight = bounds.height / 2f;
        float x = bounds.x;
        float y = bounds.y;

        nodes[0] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x + subWidth, y, subWidth, subHeight), zAxis);
        nodes[1] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x, y, subWidth, subHeight), zAxis);
        nodes[2] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x, y - subHeight, subWidth, subHeight), zAxis);
        nodes[3] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x + subWidth, y - subHeight, subWidth, subHeight), zAxis);
    }

    private int getIndex(GameObject toCheck)
    {
        Sprite s_toCheck = SpriteBatching.GetSpriteFromObject(toCheck);
        if (s_toCheck == null) throw new Exception(toCheck.name + " et ses enfants n'ont pas de Sprite!");

        int index = -1;
        double verticalMidpoint = bounds.x + (bounds.width / 2f);
        double horizontalMidpoint = bounds.y - (bounds.height / 2f);

        bool topQuadrant = (toCheck.transform.position.y > horizontalMidpoint && toCheck.transform.position.y - s_toCheck.SpriteSize.y > horizontalMidpoint);
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
        previousPositions.Add(newObject.transform.position);

        if (objects.Count > maxObjects && level < maxLevels)
        {
            if (nodes[0] == null || nodes[1] == null || nodes[2] == null || nodes[3] == null) split();

            //foreach (GameObject anObject in objects)
            for (int i = 0; i < objects.Count; i++)
            {
                int index = getIndex(objects[i]);
                if (index != -1)
                {
                    nodes[index].insert(objects[i]);
                    objects.RemoveAt(i);
                    previousPositions.RemoveAt(i);
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

        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] == null)
            {
                objects.RemoveAt(i);
                previousPositions.RemoveAt(i);
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

    public int getLevel()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != null)
                return nodes[i].getLevel();
        }

        return level;
    }

    public int getNumObjects()
    {
        int toReturn = objects.Count;

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != null)
                toReturn += nodes[i].getNumObjects();
        }

        return toReturn;
    }

    public bool isInBound(GameObject toCheck)
    {
        Vector3 position = toCheck.transform.position;
        return ((position.x >= bounds.x && position.x <= bounds.x + bounds.width) &&
                (position.y <= bounds.y && position.y >= bounds.y - bounds.height));
    }

    private bool hasMoved(int index)
    {
        return previousPositions[index] != objects[index].transform.position;
    }

    public void update()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != null)
                nodes[i].update();
        }
    }

    public override string ToString()
    {
        return "Level = " + getLevel() + ". Number of objects = " + getNumObjects();
    }
}