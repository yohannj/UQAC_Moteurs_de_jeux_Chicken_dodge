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
    private HashSet<GameObject> all_objects;
    private HashSet<GameObject> objects;
    private Rect bounds;
    private QuadTree[] nodes;

    private float zAxis;

    public QuadTree(int level, int maxObjects, int maxLevels, Rect bounds, float zAxis)
    {
        this.level = level;
        this.maxObjects = maxObjects;
        this.maxLevels = maxLevels;
        this.bounds = bounds;
        all_objects = new HashSet<GameObject>();
        objects = new HashSet<GameObject>();
        nodes = new QuadTree[0];
        this.zAxis = zAxis;
    }

    public void init()
    {
        all_objects.RemoveWhere(o => o == null);
        objects = new HashSet<GameObject>(all_objects);

        nodes = new QuadTree[0];
    }

    private void split()
    {
        float subWidth = bounds.width / 2f;
        float subHeight = bounds.height / 2f;
        float x = bounds.x;
        float y = bounds.y;

        nodes = new QuadTree[4];
        nodes[0] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x + subWidth, y, subWidth, subHeight), zAxis);
        nodes[1] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x, y, subWidth, subHeight), zAxis);
        nodes[2] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x, y - subHeight, subWidth, subHeight), zAxis);
        nodes[3] = new QuadTree(level + 1, maxObjects, maxLevels, new Rect(x + subWidth, y - subHeight, subWidth, subHeight), zAxis);
    }

    private List<int> getIndexes(GameObject toCheck)
    {
        Sprite s_toCheck = SpriteBatching.GetSpriteFromObject(toCheck);
        if (s_toCheck == null) throw new Exception(toCheck.name + " et ses enfants n'ont pas de Sprite!");
		List<int> indexes = new List<int>();

		float spriteX = s_toCheck.getSpriteSize().x;
		float spriteY = s_toCheck.getSpriteSize().y;
		float posX = toCheck.transform.position.x;
		float posY = toCheck.transform.position.y;

		Vector2[] corners = new Vector2[4];

		corners[0] = new Vector2(spriteX + posX, spriteY + posY);
		corners[1] = new Vector2(posX, spriteY + posY);
		corners[2] = new Vector2(posX, posY);
		corners[3] = new Vector2(spriteX + posX, posY);

		for (int i = 0; i < corners.Length; i++)
		{
			for (int j = 0; j < nodes.Length; j++)
			{
				if (nodes[j].isInBound(corners[i]) && !indexes.Contains(j))
					indexes.Add(j);
			}
		}

        return indexes;
    }

    public void insert(GameObject newObject)
    {
		all_objects.Add(newObject);
    }

    private void insertDirect(GameObject newObject)
    {
        objects.Add(newObject);
    }

	public void shapeQuadTree() {
		if (objects.Count > maxObjects && level < maxLevels)
		{
			split ();
			foreach (GameObject anObject in objects)
			{
				List<int> indexes = getIndexes(anObject);
				foreach (int index in indexes)
				{
					nodes[index].insertDirect(anObject);
				}
			}

			for (int i = 0; i < nodes.Length; i++)
				nodes[i].shapeQuadTree();
		}
	}

	public bool inSameRect(GameObject go1, GameObject go2)
	{
		if(objects.Contains(go1) && objects.Contains(go2)) {
			return true;
		}

		for (int i = 0; i < nodes.Length; i++)
		{
				if(nodes[i].inSameRect(go1, go2))
					return true;
		}

		return false;
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
                nodes[i].Draw();
        }
    }

//     public int getLevel()
//     {
//         for (int i = 0; i < nodes.Length; i++)
//         {
//             if (nodes[i] != null)
//                 return nodes[i].getLevel();
//         }
// 
//         return level;
//     }

//     public int getNumObjects()
//     {
//         int toReturn = objects.Count;
// 
//         for (int i = 0; i < nodes.Length; i++)
//         {
//             if (nodes[i] != null)
//                 toReturn += nodes[i].getNumObjects();
//         }
// 
//         return toReturn;
//     }

    /*public bool isInBound(GameObject toCheck)
    {
        Vector3 position = toCheck.transform.position;
        return ((position.x >= bounds.x && position.x <= bounds.x + bounds.width) &&
                (position.y <= bounds.y && position.y >= bounds.y - bounds.height));
    }*/

	public bool isInBound(Vector2 toCheck)
	{
		return ((toCheck.x >= bounds.x && toCheck.x <= bounds.x + bounds.width) &&
		        (toCheck.y <= bounds.y && toCheck.y >= bounds.y - bounds.height));
	}

    public override string ToString()
    {
        return "Hello world!";
        //return "Level = " + getLevel() + ". Number of objects = " + getNumObjects();
    }
}