using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController_Old : MonoBehaviour
{
    private static MapController_Old instance;
    public static MapController_Old GetInstance { get { return instance; } }

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
    Tilemap maze;

    [SerializeField]
    ButtonController newGame;

    Coordinate InitMapOffset = new Coordinate(-5, -5);
    bool isInverted = false;
    Levels levelsInJson;

    int currentLevel = 0;
    public int initialLevel = 0;
    public int finalLevel = 8;

    enum KeyState
    {
        Up,
        Pressed,
        Down
    }

    KeyState MouseRight = KeyState.Up;

    [SerializeField]
    private TextAsset json;

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

    void StartGame()
    {
        levelsInJson = JsonUtility.FromJson<Levels>(json.text);
        
        CreateMap(isInverted);
        PlayerController.GetInstance.StartGame();
        StateMachine.GetInstance.SetState(StateMachine.GameState.LevelStarted);
    }

    void Update()
    {
        if (StateMachine.GameState.Playing == StateMachine.GetInstance.GetState())
        {
            CheckPlayerPosition();
            CheckInputs();
            CheckInvert();
        }
        else if (StateMachine.GameState.LevelStarted == StateMachine.GetInstance.GetState())
        {
            ResetInputs();
            isInverted = false;
        }
        else
        {
            if ((newGame.IsRequested()) && (StateMachine.GameState.MainMenu == StateMachine.GetInstance.GetState()))
            {
                currentLevel = initialLevel;
                StartGame();
            }
        }
    }

    void CreateMap(bool inverted)
    {
        maze.ClearAllTiles();

        Map newMap = levelsInJson.levels[currentLevel];

        List<Coordinate> safes = newMap.safes;

        foreach (Coordinate coordinate in safes)
        {
            int safeX = InitMapOffset.x + coordinate.x;
            int safeY = InitMapOffset.y + coordinate.y;

            maze.SetTile(new Vector3Int(safeX, safeY, 0), safe);
        }

        List<Coordinate> blockers = newMap.blockers;

        foreach (Coordinate coordinate in blockers)
        {
            int blockerX = InitMapOffset.x + coordinate.x;
            int blockerY = InitMapOffset.y + coordinate.y;

            maze.SetTile(new Vector3Int(blockerX, blockerY, 0), blocker);
        }

        int endX = InitMapOffset.x + newMap.end.x;
        int endY = InitMapOffset.y + newMap.end.y;

        maze.SetTile(new Vector3Int(endX, endY, 0), end);

        List<Coordinate> blocks;

        if (inverted && StateMachine.GameState.Playing == StateMachine.GetInstance.GetState())
        {
            blocks = newMap.empty;
        }
        else
        {
            blocks = newMap.units;
        }

        foreach (Coordinate coordinate in blocks)
        {
            int unitX = InitMapOffset.x + coordinate.x;
            int unity = InitMapOffset.y + coordinate.y;

            maze.SetTile(new Vector3Int(unitX, unity, 0), unit);
        }

        if ((newMap.portalin.x != -1) && (newMap.portalin.y != -1))
        { 
            int portalInY = InitMapOffset.y + newMap.portalin.y;
            int portalInX = InitMapOffset.x + newMap.portalin.x;
            maze.SetTile(new Vector3Int(portalInX, portalInY, 0), portalIn);
        }

        if ((newMap.portalout.x != -1) && (newMap.portalout.y != -1))
        {
            int portalOutX = InitMapOffset.x + newMap.portalout.x;
            int portalOutY = InitMapOffset.y + newMap.portalout.y;
            maze.SetTile(new Vector3Int(portalOutX, portalOutY, 0), portalOut);
        }
    }

    void CheckPlayerPosition()
    {
        Coordinate playerPosition = PlayerController.GetInstance.GetPosition();
        Coordinate finalPosition = GetEnd();
        
        if ((playerPosition.x == finalPosition.x) && (playerPosition.y == finalPosition.y))
        {
            //Debug.Log("You win!");
            StateMachine.GetInstance.SetState(StateMachine.GameState.LevelFinished);
        }
    }

    void CheckInvert()
    {
        if (isSafe(PlayerController.GetInstance.GetPosition()) && (KeyState.Pressed == MouseRight))
        {
            MouseRight = KeyState.Down;
            InvertMap();
        }
    }

    void CheckInputs()
    {
        if (isSafe(PlayerController.GetInstance.GetPosition()))
        {
            if ((Input.GetMouseButtonDown(1) == true) && (KeyState.Up == MouseRight))
            {
                MouseRight = KeyState.Pressed;
            }
            else if ((Input.GetMouseButtonUp(1) == true) && (KeyState.Down == MouseRight))
            {
                MouseRight = KeyState.Up;
            }
            else { }
        }
    }

    void ResetInputs()
    {
        MouseRight = KeyState.Up;
    }

    void InvertMap()
    {
        isInverted = !isInverted;

        maze.ClearAllTiles();
        CreateMap(isInverted);

        Coordinate playerPosition = PlayerController.GetInstance.GetPosition();

        if (isUnit(playerPosition))
        {
            PlayerController.GetInstance.SetInitialPosition();
        }
    }

    public Coordinate GetInitialPosition()
    {
        Map map = levelsInJson.levels[currentLevel];
        
        Coordinate begin = new Coordinate(map.begin.x, map.begin.y);

        return begin;
    }

    public Coordinate GetBegin()
    {
        Map map = levelsInJson.levels[currentLevel];

        int startX = InitMapOffset.x + map.begin.x;
        int startY = InitMapOffset.y + map.begin.y;

        Coordinate begin = new Coordinate { x = startX, y = startY };

        return begin;
    }

    public Coordinate GetEnd()
    {
        Map map = levelsInJson.levels[currentLevel];

        int endX = map.end.x;
        int endY = map.end.y;

        Coordinate end = new Coordinate { x = endX, y = endY };

        return end;
    }

    public Coordinate GetPortalOut()
    {
        Map map = levelsInJson.levels[currentLevel];

        int portaloutX = map.portalout.x;
        int portaloutY = map.portalout.y;

        Coordinate portalout = new Coordinate { x = portaloutX, y = portaloutY };

        return portalout;
    }

    public bool isUnit(Coordinate coordinate)
    {
        bool ret = false;

        Map map = levelsInJson.levels[currentLevel];
        List<Coordinate> units;

        if (!isInverted)
        {
            units = map.units;
        }
        else
        {
            units = map.empty;
        }

        foreach (Coordinate unit in units)
        {
            if ((unit.x == coordinate.x) && (unit.y == coordinate.y))
            {
                ret = true;
                break;
            }
        }

        return ret;
    }

    public bool isBlocker(Coordinate coordinate)
    {
        bool ret = false;

        Map map = levelsInJson.levels[currentLevel];
        List<Coordinate> blockers;

        blockers = map.blockers;

        foreach (Coordinate blocker in blockers)
        {
            if ((blocker.x == coordinate.x) && (blocker.y == coordinate.y))
            {
                ret = true;
                break;
            }
        }

        return ret;
    }

    public bool isSafe(Coordinate coordinate)
    {
        bool ret = false;

        Map map = levelsInJson.levels[currentLevel];
        List<Coordinate> safes;

        safes = map.safes;

        foreach (Coordinate safe in safes)
        {
            if ((safe.x == coordinate.x) && (safe.y == coordinate.y))
            {
                ret = true;
                break;
            }
        }

        return ret;
    }

    public bool isPortalOut(Coordinate coordinate)
    {
        bool ret = false;

        Map map = levelsInJson.levels[currentLevel];
        Coordinate portalOut;

        portalOut = map.portalout;
        
        if ((portalOut.x == coordinate.x) && (portalOut.y == coordinate.y))
        {
            ret = true;
        }

        return ret;
    }

    public bool isPortalIn(Coordinate coordinate)
    {
        bool ret = false;

        Map map = levelsInJson.levels[currentLevel];
        Coordinate portalIn;

        portalIn = map.portalin;

        if ((portalIn.x == coordinate.x) && (portalIn.y == coordinate.y))
        {
            ret = true;
        }

        return ret;
    }

    public void NewLevel()
    {
        if (!isFinalLevel())
        {
            currentLevel++;
            CreateMap(isInverted);
            isInverted = false;
            PlayerController.GetInstance.StartGame();
            //StateMachine.GetInstance.SetState(StateMachine.GameState.LevelStarted);
        }
        else
        {
            StateMachine.GetInstance.SetState(StateMachine.GameState.MainMenu);
        }
    }

    public bool isTutorialNeeded()
    {
        bool needed = false;
        Map newMap = levelsInJson.levels[currentLevel];

        if (newMap.tutorial != -1)
        {
            needed = true;
        }

        return needed;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public bool isFinalLevel()
    {
        return finalLevel == GetCurrentLevel();
    }

}
