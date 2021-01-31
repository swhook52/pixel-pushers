using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseMedkit : MonoBehaviour
{
    public GameObject item;
    private PlayerController playerController;
    private PlayerInventory inventory;
    public int medkitHealing = 15;

    void Start()
    {
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    public void DestroyItem()
    {
        foreach(Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void Heal()
    {
        if (playerController.currentHealth != playerController.startingHealth && playerController.currentHealth >= (playerController.startingHealth - medkitHealing))
        {
            playerController.currentHealth = playerController.startingHealth;
            DestroyItem();
        }
        else
        {
            playerController.currentHealth += medkitHealing;
            DestroyItem();
        }
        
    }
}
