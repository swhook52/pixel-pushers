using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public Transform target;
    public float enemyHealth = 1;
    public float enemyDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.3f;
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("I see you");
            target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("I do not see you");
            target = null;
        }
    }
}
