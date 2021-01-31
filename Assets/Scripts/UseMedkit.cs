using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseMedkit : MonoBehaviour
{
    public GameObject item;
    
    private PlayerInventory inventory;
    

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    public void DestroyItem()
    {
        foreach(Transform child in transform)
        {
            GameObject.Destroy(child);
        }
    }
}
