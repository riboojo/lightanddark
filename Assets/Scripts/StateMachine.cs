using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private static StateMachine instance;
    public static StateMachine GetInstance { get { return instance; } }

    bool canMoveSate = true;

    public enum GameState
    {
        MainMenu = 0,
        LevelSelector,
        Tutorial,
        LevelStarted,
        Playing,
        PauseMenu,
        LevelFinished,
        LevelTransition,
        QuitGame
    }

    GameState state = GameState.MainMenu;

    private void Awake()
    {
        if ((instance != null) && (instance != this))
        {
            Debug.LogError("Instance of StateMachine has already been created!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        SetState(GameState.MainMenu);
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.MainMenu:
                StateMainMenu();
                break;
            case GameState.LevelSelector:
                break;
            case GameState.LevelStarted:
                StateLevelStarted();
                break;
            case GameState.Tutorial:
                StateTutorial();
                break;
            case GameState.Playing:
                StatePlaying();
                break;
            case GameState.PauseMenu:
                break;
            case GameState.LevelFinished:
                StateLevelFinished();
                break;
            case GameState.LevelTransition:
                StateLevelTransition();
                break;
            case GameState.QuitGame:
                break;
            default:
                break;
        }
    }

    public GameState GetState()
    {
        return state;
    }

    public void SetState(GameState newSatate)
    {
        state = newSatate;
    }

    void StateMainMenu()
    {
        UIController.GetInstance.HideLevelComplete();
        UIController.GetInstance.ShowMenu();
    }

    void StateLevelTransition()
    {
        if (JuiceController.GetInstance.IsPerformNewLevelFinished())
        {
            SetState(StateMachine.GameState.LevelStarted);
            return;
        }

        if (JuiceController.GetInstance.IsFrameIdle() || !UIController.GetInstance.IsFrameActive())
        {
            UIController.GetInstance.ShowLevelComplete();
            UIController.GetInstance.ShowFrame();
        }

        if (Input.GetMouseButtonDown(0))
        {
            UIController.GetInstance.HideLevelComplete();
            UIController.GetInstance.HideMenu();

            MapController.GetInstance.NewLevel();

            JuiceController.GetInstance.PerformNewLevel();
        }
    }

    void StateLevelStarted()
    {
        UIController.GetInstance.HideLevelComplete();
        UIController.GetInstance.HideMenu();
        UIController.GetInstance.ShowFrame();

        //Debug.Log("Starting level");
        if (MapController.GetInstance.isTutorialNeeded())
        {
            SetState(StateMachine.GameState.Tutorial);
        }
        else
        {
            //Debug.Log("Tutorial dont needed");
            SetState(StateMachine.GameState.Playing);
        }
    }

    void StateTutorial()
    {
        //Debug.Log("Tutorial needed");
        UIController.GetInstance.ShowTutorial(MapController.GetInstance.GetCurrentLevel());
        JuiceController.GetInstance.BlurBackground();

        //Debug.Log("StateLevelStarted");
        if (Input.GetMouseButtonDown(0))
        {
            canMoveSate = UIController.GetInstance.HideTutorial(MapController.GetInstance.GetCurrentLevel());
            JuiceController.GetInstance.FocusBackground();
            //Debug.Log("canMoveSate: " + canMoveSate);
        }
        else
        {
            canMoveSate = false;
        }

        if (canMoveSate)
        {
            //Debug.Log("Playing");
            SetState(StateMachine.GameState.Playing);
        }
    }

    void StatePlaying()
    {
        UIController.GetInstance.ShowPlayer();
    }

    void StateLevelFinished()
    {
        if (JuiceController.GetInstance.IsPerformFinishLevelFinished())
        {
            SetState(StateMachine.GameState.LevelTransition);
            return;
        }

        if (JuiceController.GetInstance.IsFrameIdle())
        {
            Debug.Log("Animation requested");
            UIController.GetInstance.HidePlayer();
            JuiceController.GetInstance.PerformFinishLevel();
        }
    }
}
