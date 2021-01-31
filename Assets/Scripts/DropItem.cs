using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject item;
    private Transform player;
    //private int distanceFromPlayer = 1;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SpawnDroppedItem()
    {
        Vector2 playerPosition = new Vector2(player.position.x, player.position.y + 0.35f);
        Instantiate(item, playerPosition, Quaternion.identity);
    }
}
