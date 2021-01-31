using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifetime = 2f;
    public float bulletSpeed = 20f;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Enemy"))
        {
            KillEnemy(collision);
            Destroy(gameObject);
        }
    }

    public void KillEnemy(Collider2D collider)
    {
        //also do an animation here
        SoundManager.PlaySound("death");
        Destroy(collider.gameObject.transform.parent.gameObject);
    }
}