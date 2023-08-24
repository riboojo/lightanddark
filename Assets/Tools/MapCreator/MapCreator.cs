using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreator : MonoBehaviour
{
    [SerializeField]
    int tutorial;

    [SerializeField]
    bool inverted;

    [SerializeField]
    Coordinate begin = new Coordinate();

    Tilemap maze;
    Map newMap;

    Coordinate offset = new Coordinate(5, 5);

    const int maxX = 8;
    const int maxY = 8;

    string path = "Assets/Jsons/GeneratedMap.json";

    public void Create()
    {
        maze = GetComponent<Tilemap>();
        newMap = new Map();

        newMap.begin = begin;
        newMap.portalin = new Coordinate(-1,-1);
        newMap.portalout = new Coordinate(-1,-1);
        newMap.units = new List<Coordinate>();
        newMap.safes = new List<Coordinate>();
        newMap.empty = new List<Coordinate>();
        newMap.blockers = new List<Coordinate>();
        newMap.tutorial = tutorial;

        for (int i = 0; i <= maxX; i++)
        {
            for (int j = 0; j <= maxY; j++)
            {
                TileBase tile = maze.GetTile(new Vector3Int(i- offset.x, j - offset.y, 0));
                if (tile != null)
                {
                    if ("Unit" == tile.name)
                    {
                        if (!inverted)
                        {
                            newMap.units.Add(new Coordinate(i, j));
                        }
                        else
                        {
                            newMap.empty.Add(new Coordinate(i, j));
                        }
                    }
                    else if ("End" == tile.name)
                    {
                        newMap.end = new Coordinate(i, j);
                    }
                    else if ("Safe" == tile.name)
                    {
                        newMap.safes.Add(new Coordinate(i, j));
                    }
                    else if ("Blocker" == tile.name)
                    {
                        newMap.blockers.Add(new Coordinate(i, j));
                    }
                    else if ("PortalIn" == tile.name)
                    {
                        newMap.portalin = new Coordinate(i, j);
                    }
                    else if ("PortalOut" == tile.name)
                    {
                        newMap.portalout = new Coordinate(i, j);
                    }
                    else { }
                }
                else
                {
                    if (!inverted)
                    {
                        newMap.empty.Add(new Coordinate(i, j));
                    }
                    else
                    {
                        newMap.units.Add(new Coordinate(i, j));
                    }
                }
            }
        }

        string json = JsonUtility.ToJson(newMap);
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(json);
        writer.Close();

        Debug.Log("Map generated succesfully!");
    }
}
