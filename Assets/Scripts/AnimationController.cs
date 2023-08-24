using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [HideInInspector]
    public bool isLevelCompleteAnimationStarted = false;
    [HideInInspector]
    public bool isLevelCompleteAnimationFinished = false;
    [HideInInspector]
    public bool isNewLevelAnimationStarted = false;
    [HideInInspector]
    public bool isNewLevelAnimationFinished = false;
    [HideInInspector]
    public bool isShowTutorialStarted = false;
    [HideInInspector]
    public bool isShowTutorialFinished = false;

    public void LevelCompleteAnimationStart()
    {
        isLevelCompleteAnimationStarted = true;
        isLevelCompleteAnimationFinished = false;
    }

    public void LevelCompleteAnimationFinish()
    {
        isLevelCompleteAnimationStarted = false;
        isLevelCompleteAnimationFinished = true;

        UIController.GetInstance.HideFrame();
    }

    public void NewLevelAnimationStart()
    {
        UIController.GetInstance.ShowFrame();
        isNewLevelAnimationStarted = true;
        isNewLevelAnimationFinished = false;
    }

    public void NewLevelAnimationFinish()
    {
        isNewLevelAnimationStarted = false;
        isNewLevelAnimationFinished = true;
    }

    public void IdleAnimationRunning()
    {
        isLevelCompleteAnimationStarted = false;
        isLevelCompleteAnimationFinished = false;
        isNewLevelAnimationStarted = false;
        isNewLevelAnimationFinished = false;
    }

    public void ShowTutorialStarted()
    {
        isShowTutorialStarted = true;
        isShowTutorialFinished = false;
    }

    public void ShowTutorialFinished()
    {
        isShowTutorialStarted = false;
        isShowTutorialFinished = true;
    }

}
