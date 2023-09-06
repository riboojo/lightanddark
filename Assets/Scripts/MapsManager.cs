using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapsManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset json;

    [SerializeField]
    Tile unit;
    [SerializeField]
    Tile safe;
    [SerializeField]
    Tile end;
    [SerializeField]
    Tile blocker;
    [SerializeField]
    Tile portalIn;
    [SerializeField]
    Tile portalOut;
    [SerializeField]
    Tile[] borders;
    [SerializeField]
    Tilemap maze;

    const int BORDER_L = 0;
    const int BORDER_R = 1;
    const int BORDER_U = 2;
    const int BORDER_D = 3;

    Levels levelsInJson;

    int currentLevel = 0;

    void Start()
    {
        StartGame();
    }
    
    void Update()
    {
        
    }

    void StartGame()
    {
        levelsInJson = JsonUtility.FromJson<Levels>(json.text);
        CreateMap();
    }

    void CreateMap()
    {
        maze.ClearAllTiles();

        Map newMap = levelsInJson.levels[currentLevel];

        List<Coordinate> safes = newMap.safes;

        foreach (Coordinate coordinate in safes)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), safe);
        }

        List<Coordinate> blockers = newMap.blockers;

        foreach (Coordinate coordinate in blockers)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), blocker);
        }
        
        maze.SetTile(new Vector3Int(newMap.end.x, newMap.end.y, 0), end);
        
        foreach (Coordinate coordinate in newMap.units)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), unit);
        }

        foreach (Coordinate coordinate in newMap.units)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), unit);
        }

        foreach (Coordinate coordinate in newMap.borderL)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), borders[BORDER_L]);
        }

        foreach (Coordinate coordinate in newMap.borderR)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), borders[BORDER_R]);
        }

        foreach (Coordinate coordinate in newMap.borderU)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), borders[BORDER_U]);
        }

        foreach (Coordinate coordinate in newMap.borderD)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), borders[BORDER_D]);
        }

        //if ((newMap.portalin.x != -1) && (newMap.portalin.y != -1))
        //{
        //    maze.SetTile(new Vector3Int(newMap.portalin.x, newMap.portalin.y, 0), portalIn);
        //}

        //if ((newMap.portalout.x != -1) && (newMap.portalout.y != -1))
        //{
        //    maze.SetTile(new Vector3Int(newMap.portalout.x, newMap.portalout.y, 0), portalOut);
        //}
    }
}
