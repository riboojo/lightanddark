using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreator : MonoBehaviour
{
    [SerializeField]
    Tile unit;
    [SerializeField]
    bool tutorial;
    [SerializeField]
    int tutorialNumber = 0;    
    [SerializeField]
    int sizeX;
    [SerializeField]
    int sizeY;

    [SerializeField]
    Coordinate begin = new Coordinate();

    Tilemap maze;
    Map newMap;
    
    string path = "Assets/Jsons/GeneratedMap.json";

    public void Template()
    {
        maze = GetComponent<Tilemap>();
        maze.ClearAllTiles();

        for (int i = 0; i <= sizeX - 1; i++)
        {
            for (int j = 0; j <= sizeY - 1; j++)
            {
                maze.SetTile(new Vector3Int(i, j, 0), unit);
            }
        }
    }

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
        newMap.borderL = new List<Coordinate>();
        newMap.borderR = new List<Coordinate>();
        newMap.borderU = new List<Coordinate>();
        newMap.borderD = new List<Coordinate>();
        newMap.sizeX = sizeX;
        newMap.sizeY = sizeY;

        if (tutorial)
        {
            newMap.tutorial = tutorialNumber;
        }
        else
        {
            newMap.tutorial = -1;
        }

        for (int i = 0; i <= sizeX - 1; i++)
        {
            for (int j = 0; j <= sizeY - 1; j++)
            {
                TileBase tile = maze.GetTile(new Vector3Int(i, j, 0));
                if (tile != null)
                {
                    //Debug.Log(tile.name);
                    if ("Unit" == tile.name)
                    {
                        newMap.units.Add(new Coordinate(i, j));
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
                    else if ("BorderL" == tile.name)
                    {
                        newMap.borderL.Add(new Coordinate(i, j));
                    }
                    else if ("BorderR" == tile.name)
                    {
                        newMap.borderR.Add(new Coordinate(i, j));
                    }
                    else if ("BorderU" == tile.name)
                    {
                        newMap.borderU.Add(new Coordinate(i, j));
                    }
                    else if ("BorderD" == tile.name)
                    {
                        newMap.borderD.Add(new Coordinate(i, j));
                    }
                    else { }
                }
                else
                {
                    newMap.empty.Add(new Coordinate(i, j));
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
