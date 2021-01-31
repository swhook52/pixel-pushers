using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DamagePanelScript : MonoBehaviour
{
    public GameObject Panel;
    public static event Action<string> ButtonPressed = delegate {};
    
    // Start is called before the first frame update
    void Start()
    {
        Panel.SetActive(false);
        Panel.GetComponent<Button>().onClick.AddListener(ButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetGame() {

    }

    public void ButtonClicked() {
        Debug.Log("clicked!");
    }
}
