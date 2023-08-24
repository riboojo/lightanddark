using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController GetInstance { get { return instance; } }

    Coordinate initialPosition;
    Coordinate finalPosition;
    
    float[] InitPlayerOffset = { 0.5f,0.5f };
    Coordinate currentPosition = new Coordinate { x=0, y=4 };

    [SerializeField]
    Animator anim;

    bool isMoveRAnimationRunning = false;
    Vector3 updatedPosition = new Vector3();
    Vector3 deltaPosition = new Vector3();
    bool canMoveAgain = true;

    enum KeyState
    {
        Up,
        Pressed,
        Down
    }

    KeyState keyD = KeyState.Up;
    KeyState keyA = KeyState.Up;
    KeyState keyW = KeyState.Up;
    KeyState keyS = KeyState.Up;

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

    void Update()
    {
        if (StateMachine.GameState.Playing == StateMachine.GetInstance.GetState())
        {
            CheckInputs();
            CalculateMove();
            Move();
            CheckTeleport();
        }
        else if (StateMachine.GameState.LevelStarted == StateMachine.GetInstance.GetState())
        {
            ResetInputs();
        }
    }

    public void StartGame()
    {
        initialPosition = MapController.GetInstance.GetBegin();
        finalPosition = MapController.GetInstance.GetEnd();

        SetInitialPosition();
    }

    public void SetInitialPosition()
    {
        float startX = InitPlayerOffset[0] + initialPosition.x;
        float startY = InitPlayerOffset[1] + initialPosition.y;
        transform.position = new Vector3(startX, startY, transform.position.z);

        currentPosition = MapController.GetInstance.GetInitialPosition();
        deltaPosition = transform.position;
    }

    void CheckInputs()
    {
        if ((Input.GetKeyDown(KeyCode.D)) && (KeyState.Up == keyD))
        {
            keyD = KeyState.Pressed;
        }
        if ((Input.GetKeyDown(KeyCode.A)) && (KeyState.Up == keyA))
        {
            keyA = KeyState.Pressed;
        }
        if ((Input.GetKeyDown(KeyCode.W)) && (KeyState.Up == keyW))
        {
            keyW = KeyState.Pressed;
        }
        if ((Input.GetKeyDown(KeyCode.S)) && (KeyState.Up == keyS))
        {
            keyS = KeyState.Pressed;
        }

        if ((Input.GetKeyUp(KeyCode.D)) && (KeyState.Down == keyD))
        {
            keyD = KeyState.Up;
        }
        if ((Input.GetKeyUp(KeyCode.A)) && (KeyState.Down == keyA))
        {
            keyA = KeyState.Up;
        }
        if ((Input.GetKeyUp(KeyCode.W)) && (KeyState.Down == keyW))
        {
            keyW = KeyState.Up;
        }
        if ((Input.GetKeyUp(KeyCode.S)) && (KeyState.Down == keyS))
        {
            keyS = KeyState.Up;
        }
    }

    void ResetInputs()
    {
        keyD = KeyState.Up;
        keyA = KeyState.Up;
        keyW = KeyState.Up;
        keyS = KeyState.Up;
    }

    void CalculateMove()
    {
        if (canMoveAgain)
        {
            float moveX = 0;
            float moveY = 0;

            if (KeyState.Pressed == keyD)
            {
                keyD = KeyState.Down;

                Coordinate newPosition = new Coordinate { x = currentPosition.x + 1, y = currentPosition.y };
                if (CanMove(newPosition))
                {
                    anim.SetTrigger("moveR");
                    moveX = 1.0f;
                    updatedPosition = new Vector3(moveX, moveY, 0);
                    deltaPosition = new Vector3(transform.position.x + moveX, transform.position.y, transform.position.z);
                    currentPosition.x += 1;
                    canMoveAgain = false;
                }
            }

            if (KeyState.Pressed == keyA)
            {
                keyA = KeyState.Down;

                Coordinate newPosition = new Coordinate { x = currentPosition.x - 1, y = currentPosition.y };
                if (CanMove(newPosition))
                {
                    moveX = -1.0f;
                    currentPosition.x -= 1;
                }
            }

            if (KeyState.Pressed == keyW)
            {
                keyW = KeyState.Down;

                Coordinate newPosition = new Coordinate { x = currentPosition.x, y = currentPosition.y + 1 };
                if (CanMove(newPosition))
                {
                    moveY = 1.0f;
                    currentPosition.y += 1;
                }
            }

            if (KeyState.Pressed == keyS)
            {
                keyS = KeyState.Down;

                Coordinate newPosition = new Coordinate { x = currentPosition.x, y = currentPosition.y - 1 };
                if (CanMove(newPosition))
                {
                    moveY = -1.0f;
                    currentPosition.y -= 1;
                }
            }
        }
    }

    void Move()
    {
        //transform.Translate(deltaPosition * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, deltaPosition);
        Debug.Log("Distance: " + distance);

        if (0.1f <= distance)
        {
            transform.position += updatedPosition * Time.deltaTime * 2.0f;
        }
        else
        {
            if (!canMoveAgain)
            {
                transform.position = deltaPosition;
            }
            
            updatedPosition = new Vector3(0, 0, 0);
            canMoveAgain = true;
        }
    }

    bool CanMove(Coordinate coordinate)
    {
        bool can = false;

        if ((coordinate.x >= 0) && (coordinate.x <= 8))
        {
            if ((coordinate.y >= 0) && (coordinate.y <= 8))
            {
                can |= !MapController.GetInstance.isUnit(coordinate);
                can &= !MapController.GetInstance.isBlocker(coordinate);
                can &= !MapController.GetInstance.isPortalOut(coordinate);
            }
        }

        if (!can)
        {
            JuiceController.GetInstance.PerformScreenshake();
        }

        return can;
    }

    void CheckTeleport()
    {
        if (MapController.GetInstance.isPortalIn(currentPosition))
        {
            Coordinate portalOut = MapController.GetInstance.GetPortalOut();

            float portalOutX = portalOut.x - currentPosition.x;
            float portalOutY = portalOut.y - currentPosition.y;
            transform.position = new Vector3(transform.position.x + portalOutX, transform.position.y + portalOutY, transform.position.z);

            currentPosition = portalOut;
        }
    }

    public Coordinate GetPosition()
    {
        return currentPosition;
    }

    public void MoveAnimationFinished()
    {
        isMoveRAnimationRunning = false;
    }
}
