using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    bool isAvailable;

    bool buttonPressed = false;

    void Start()
    {
        GetComponent<Button>().enabled = isAvailable;
    }

    public void Clicked()
    {
        buttonPressed = true;
    }

    public bool IsRequested()
    {
        bool wasPressed = buttonPressed;
        buttonPressed = false;

        return wasPressed;
    }
}
