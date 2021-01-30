using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject item;
    private Transform player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SpawnDroppedItem()
    {
        Vector2 playerPosition = new Vector2(player.position.x, player.position.y + 2);
        Instantiate(item, playerPosition, Quaternion.identity);
    }
}
