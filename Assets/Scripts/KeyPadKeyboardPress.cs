using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class KeyPadKeyboardPress : MonoBehaviour
{
    private int dividerPosition;
    private string buttonName, buttonValue;
    // Start is called before the first frame update
    void Start()
    {
        buttonName = gameObject.name;
        dividerPosition = buttonName.IndexOf('_');
        buttonValue = buttonName.Substring(0, dividerPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonValue == "One" && (Keyboard.current.numpad1Key.wasPressedThisFrame || Keyboard.current.digit1Key.wasPressedThisFrame))
        {
            GetComponent<Button>().onClick.Invoke();
        }
        else if (buttonValue == "Two" && (Keyboard.current.numpad2Key.wasPressedThisFrame || Keyboard.current.digit2Key.wasPressedThisFrame))
        {
            GetComponent<Button>().onClick.Invoke();
        }
        else if (buttonValue == "Three" && (Keyboard.current.numpad3Key.wasPressedThisFrame || Keyboard.current.digit3Key.wasPressedThisFrame))
        {
            GetComponent<Button>().onClick.Invoke();
        }
        else if (buttonValue == "Four" && (Keyboard.current.numpad4Key.wasPressedThisFrame || Keyboard.current.digit4Key.wasPressedThisFrame))
        {
            GetComponent<Button>().onClick.Invoke();
        }
        else if (buttonValue == "Five" && (Keyboard.current.numpad5Key.wasPressedThisFrame || Keyboard.current.digit5Key.wasPressedThisFrame))
        {
            GetComponent<Button>().onClick.Invoke();
        } 
        else if (buttonValue == "Six" && (Keyboard.current.numpad6Key.wasPressedThisFrame || Keyboard.current.digit6Key.wasPressedThisFrame))
        {
            GetComponent<Button>().onClick.Invoke();
        }
        else if (buttonValue == "Seven" && (Keyboard.current.numpad7Key.wasPressedThisFrame || Keyboard.current.digit7Key.wasPressedThisFrame))
        {
            GetComponent<Button>().onClick.Invoke();
        }
        else if (buttonValue == "Eight" && (Keyboard.current.numpad8Key.wasPressedThisFrame || Keyboard.current.digit8Key.wasPressedThisFrame))
        {
            GetComponent<Button>().onClick.Invoke();
        }
        else if (buttonValue == "Nine" && (Keyboard.current.numpad9Key.wasPressedThisFrame || Keyboard.current.digit9Key.wasPressedThisFrame))
        {
            GetComponent<Button>().onClick.Invoke();
        }
        else if (buttonValue == "Zero" && (Keyboard.current.numpad0Key.wasPressedThisFrame || Keyboard.current.digit0Key.wasPressedThisFrame))
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
}
