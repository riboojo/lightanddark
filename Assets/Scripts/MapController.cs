using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    private static MapController instance;
    public static MapController GetInstance { get { return instance; } }

    [SerializeField]
    Tilemap maze;

    [SerializeField]
    Transform grid;

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
    private TextAsset json;

    private Levels levelsInJson;
    private Map currentMap;

    private const int MIN_SIZE = 9;
    private float[] SCALE_FACTORS = { 0, 0.1f, 0.175f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.475f, 0.5f, 0.525f };

    const int BORDER_L = 0;
    const int BORDER_R = 1;
    const int BORDER_U = 2;
    const int BORDER_D = 3;

    public enum tiletype
    {
        unit = 0,
        blocker,
        safe,
        end,
        portalOut,
        portalIn,
        border,
        none
    }

    private void Awake()
    {
        if ((instance != null) && (instance != this))
        {
            Debug.LogError("Instance of MapController has already been created!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        levelsInJson = JsonUtility.FromJson<Levels>(json.text);

        currentMap = levelsInJson.levels[0];
        GenerateRequestedMap(currentMap);
    }

    private void Update()
    {

    }

    private void GenerateRequestedMap(Map requestedMap)
    {
        //AdjustScale(requestedMap.sizeX);

        List<Coordinate> units = requestedMap.units;

        foreach (Coordinate coordinate in units)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), unit);
        }

        List<Coordinate> safes = requestedMap.safes;

        foreach (Coordinate coordinate in safes)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), safe);
        }

        List<Coordinate> blockers = requestedMap.blockers;

        foreach (Coordinate coordinate in blockers)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), blocker);
        }

        List<Coordinate> bordersL = requestedMap.borderL;

        foreach (Coordinate coordinate in bordersL)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), borders[BORDER_L]);
        }

        List<Coordinate> bordersR = requestedMap.borderR;

        foreach (Coordinate coordinate in bordersR)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), borders[BORDER_R]);
        }

        List<Coordinate> bordersU = requestedMap.borderU;

        foreach (Coordinate coordinate in bordersU)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), borders[BORDER_U]);
        }

        List<Coordinate> bordersD = requestedMap.borderD;

        foreach (Coordinate coordinate in bordersD)
        {
            maze.SetTile(new Vector3Int(coordinate.x, coordinate.y, 0), borders[BORDER_D]);
        }

        maze.SetTile(new Vector3Int(requestedMap.end.x, requestedMap.end.y, 0), end);

        if ((requestedMap.portalin.x != -1) && (requestedMap.portalin.y != -1))
        {
            maze.SetTile(new Vector3Int(requestedMap.portalin.x, requestedMap.portalin.y, 0), portalIn);
        }

        if ((requestedMap.portalout.x != -1) && (requestedMap.portalout.y != -1))
        {
            maze.SetTile(new Vector3Int(requestedMap.portalout.x, requestedMap.portalout.y, 0), portalOut);
        }
    }

    private void AdjustScale(int size)
    {
        float ratio = SCALE_FACTORS[size - MIN_SIZE];

        grid.localScale = new Vector3(1, 1, 1);
        grid.localScale -= new Vector3(grid.localScale.x * ratio, grid.localScale.y * ratio, 0);
    }

    public tiletype GetTileType(Coordinate coordinate)
    {
        tiletype ret = tiletype.none;

        TileBase tile;
        tile = maze.GetTile(new Vector3Int(coordinate.x, coordinate.y, 0));

        if (tile != null)
        {
            if (tile.name == "Unit")
            {
                ret = tiletype.unit;
            }
            else if (tile.name == "Blocker")
            {
                ret = tiletype.blocker;
            }
            else if (tile.name == "Safe")
            {
                ret = tiletype.safe;
            }
            else if (tile.name == "End")
            {
                ret = tiletype.end;
            }
            else if (tile.name == "PortalOut")
            {
                ret = tiletype.portalOut;
            }
            else if (tile.name == "PortalIn")
            {
                ret = tiletype.portalIn;
            }
            else if (tile.name == "BorderL" || tile.name == "BorderR" || tile.name == "BorderU" || tile.name == "BorderD")
            {
                ret = tiletype.border;
            }
        }
        
        return ret;
    }
}
