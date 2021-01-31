using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseMedkit : MonoBehaviour
{
    private PlayerController playerController;
    public int medkit = 15;

    public void Use()
    {
        if (playerController.currentHealth >= (playerController.startingHealth - medkit))
        {
            playerController.currentHealth = playerController.startingHealth;
        }
        else
        {
            playerController.currentHealth += medkit;
        }
    }
}
