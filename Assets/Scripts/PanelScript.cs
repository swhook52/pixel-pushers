using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PanelScript : MonoBehaviour
{
    // public static event Action<string> ButtonPressed = delegate {};
    public GameObject Panel;
    // Start is called before the first frame update
    // private void Start() {
    //     Panel.GetComponent<Button>().onClick.AddListener(ButtonClicked);
    // }
    public void SetActive() {
        Panel.SetActive(true);
    }

    public void SetInactive() {
        Panel.SetActive(false);
    }

    // public void ButtonClicked() {
    //     Debug.Log("clicked!");
    // }
}
