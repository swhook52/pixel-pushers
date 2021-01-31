using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private PlayerInventory inventory;
    private PlayerController playerController;
    public int i;
    public int medkitHealing = 15;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }

    private void Update()
    {
        if(transform.childCount <= 0)
        {
            inventory.isFull[i] = false;
        }
    }

    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<DropItem>().SpawnDroppedItem();
            GameObject.Destroy(child.gameObject);
        }
    }

    public void Heal()
    {
        if (playerController.currentHealth != playerController.startingHealth && playerController.currentHealth >= (playerController.startingHealth - medkitHealing))
        {
            playerController.currentHealth = playerController.startingHealth;

        }
        else
        {
            playerController.currentHealth += medkitHealing;

        }

    }

    public void UseMedkit()
    {
        foreach (Transform child in transform)
        {
            Heal();
            GameObject.Destroy(child.gameObject);
        }
    }
}
