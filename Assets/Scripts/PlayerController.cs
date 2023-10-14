using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController GetInstance { get { return instance; } }

    [SerializeField]
    TileBase playerTile;
    
    Tilemap playerMap;
    Coordinate playerPosition;

    bool positionChanged = false;
    bool isPressedA = false;
    bool isPressedD = false;
    bool isPressedW = false;
    bool isPressedS = false;

    private void Awake()
    {
        if ((instance != null) && (instance != this))
        {
            Debug.LogError("Instance of PlayerController has already been created!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        playerMap = GetComponent<Tilemap>();
        SetPlayerPosition(new Coordinate(1, 11));
    }
    
    void Update()
    {
        //CheckInputs();
        CheckTouch();
        UpdatePlayerPosition();
    }

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isPressedA = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            isPressedD = true;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            isPressedW = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            isPressedS = true;
        }
        else { /* Do nothing */ }

        if (isPressedA && Input.GetKeyUp(KeyCode.A))
        {
            isPressedA = false;
            LeftMoveRequested();
        }
        else if (isPressedD && Input.GetKeyUp(KeyCode.D))
        {
            isPressedD = false;
            RightMoveRequested();
        }
        else if (isPressedW && Input.GetKeyUp(KeyCode.W))
        {
            isPressedW = false;
            UpMoveRequested();
        }
        else if (isPressedS && Input.GetKeyUp(KeyCode.S))
        {
            isPressedS = false;
            DownMoveRequested();
        }
        else { /* No nothing */ }
    }

    void CheckTouch()
    {

    }

    public void LeftMoveRequested()
    {
        Coordinate requested = new Coordinate(playerPosition.x - 1, playerPosition.y);

        if (CanMove(requested))
        {
            positionChanged = true;
            playerPosition.x--;
        }
    }

    public void RightMoveRequested()
    {
        Coordinate requested = new Coordinate(playerPosition.x + 1, playerPosition.y);

        if (CanMove(requested))
        {
            positionChanged = true;
            playerPosition.x++;
        }
    }

    public void UpMoveRequested()
    {
        Coordinate requested = new Coordinate(playerPosition.x, playerPosition.y + 1);

        if (CanMove(requested))
        {
            positionChanged = true;
            playerPosition.y++;
        }
    }

    public void DownMoveRequested()
    {
        Coordinate requested = new Coordinate(playerPosition.x, playerPosition.y -1);

        if (CanMove(requested))
        {
            positionChanged = true;
            playerPosition.y--;
        }
    }

    void UpdatePlayerPosition()
    {
        if (positionChanged)
        {
            playerMap.ClearAllTiles();
            playerMap.SetTile(new Vector3Int(playerPosition.x, playerPosition.y, 0), playerTile);
        }
    }

    bool CanMove(Coordinate requested)
    {
        bool ret = false;
        MapController.tiletype tile;

        tile = MapController.GetInstance.GetTileType(requested);

        switch (tile)
        {
            case MapController.tiletype.none:
            case MapController.tiletype.safe:
            case MapController.tiletype.portalIn:
                ret = true;
                break;
            case MapController.tiletype.end:
                // TODO: Level Finished
                Debug.Log("Level Finished");
                break;
            default:
                break;
        }

        return ret;
    }

    public void SetPlayerPosition(Coordinate pos)
    {
        playerPosition = pos;
    }
    
}
