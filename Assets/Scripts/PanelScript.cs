using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour
{
    public GameObject Panel;
    // Start is called before the first frame update
    public void SetActive() {
        Panel.SetActive(true);
    }

    public void SetInactive() {
        Panel.SetActive(false);
    }
}
