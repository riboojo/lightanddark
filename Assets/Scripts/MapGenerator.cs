using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    private static MapGenerator instance;
    public static MapGenerator GetInstance { get { return instance; } }

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
    private TextAsset json;

    private Levels levelsInJson;

    private const int MIN_SIZE = 9;
    private float[] SCALE_FACTORS = { 0, 0.1f, 0.175f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.475f, 0.5f, 0.525f };
    
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
        GenerateRequestedMap(levelsInJson.levels[1]);
    }

    private void Update()
    {
        
    }

    private void GenerateRequestedMap(Map requestedMap)
    {
        AdjustScale(requestedMap.size);

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
}
