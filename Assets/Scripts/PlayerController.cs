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
        CheckInputs();
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
            positionChanged = true;
            playerPosition.x--;
        }
        else if (isPressedD && Input.GetKeyUp(KeyCode.D))
        {
            isPressedD = false;
            positionChanged = true;
            playerPosition.x++;
        }
        else if (isPressedW && Input.GetKeyUp(KeyCode.W))
        {
            isPressedW = false;
            positionChanged = true;
            playerPosition.y++;
        }
        else if (isPressedS && Input.GetKeyUp(KeyCode.S))
        {
            isPressedS = false;
            positionChanged = true;
            playerPosition.y--;
        }
        else { /* No nothing */ }
    }

    void UpdatePlayerPosition()
    {
        if (positionChanged)
        {
            playerMap.ClearAllTiles();
            playerMap.SetTile(new Vector3Int(playerPosition.x, playerPosition.y, 0), playerTile);
        }
    }

    public void SetPlayerPosition(Coordinate pos)
    {
        playerPosition = pos;
    }
}
