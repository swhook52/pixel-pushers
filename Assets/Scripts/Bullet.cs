using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    private float lifetime = 2f;
    public float bulletSpeed = 20f;
    public float bulletDamage;

    void Start()
    {
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Debug.Log("I definitely hit a wall");
            Destroy(gameObject);
        }

        Debug.Log($"Bullet {collision.tag}");
        Debug.Log($"Bullet {collision.name}");
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("I see you - killing enemy");
            KillEnemy(collision);
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D hit");
    }

    public void KillEnemy(Collider2D collider)
    {
        //also do an animation here
        Destroy(collider.gameObject);
    }
}