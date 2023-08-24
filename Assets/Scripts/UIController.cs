using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    public static UIController GetInstance { get { return instance; } }

    [SerializeField]
    GameObject menu;

    [SerializeField]
    GameObject mainMenu;

    [SerializeField]
    GameObject levelSelector;

    [SerializeField]
    GameObject levelComplete;

    [SerializeField]
    TextMeshProUGUI levelCompleteFirstLine;

    [SerializeField]
    GameObject tutorial;

    [SerializeField]
    GameObject frame;

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject[] tutorialLevels;

    private void Awake()
    {
        if ((instance != null) && (instance != this))
        {
            Debug.LogError("Instance of UIController has already been created!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void ShowMenu()
    {
        menu.SetActive(true);
    }

    public void HideMenu()
    {
        menu.SetActive(false);
    }

    public void ShowLevelComplete()
    {
        if (MapController.GetInstance.isFinalLevel())
        {
            levelCompleteFirstLine.text = "Game";
        }
        else
        {
            levelCompleteFirstLine.text = "Level";
        }

        levelComplete.SetActive(true);
    }

    public void HideLevelComplete()
    {
        levelComplete.SetActive(false);
    }

    public void ShowTutorial(int level)
    {
        if (level != -1)
        {
            //Debug.Log("Showing tutorial");
            tutorial.SetActive(true);
            tutorialLevels[level].SetActive(true);

            tutorial.GetComponent<Animator>().SetBool("enter", true);
            tutorial.GetComponent<Animator>().SetBool("exit", false);
        }
    }

    public bool HideTutorial(int level)
    {
        bool done = true;

        if (level != -1)
        {
            done = false;

            tutorial.GetComponent<Animator>().SetBool("enter", false);
            tutorial.GetComponent<Animator>().SetBool("exit", true);


            // TODO: Agregar un tiempo para que inicie a reproducirse la animacion de salida
            //Debug.Log(tutorial.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name);

            if ((tutorial.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "TutorialIn")
               && (tutorial.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "TutorialOut"))
            {
                done = true;
                tutorial.SetActive(false);
                tutorialLevels[level].SetActive(false);
            }
        }

        return done;
    }

    public void ShowFrame()
    {
        frame.SetActive(true);
    }

    public void HideFrame()
    {
        frame.SetActive(false);
    }

    public bool IsFrameActive()
    {
        return frame.activeSelf;
    }

    public void ShowPlayer()
    {
        player.SetActive(true);
    }

    public void HidePlayer()
    {
        player.SetActive(false);
    }
}
