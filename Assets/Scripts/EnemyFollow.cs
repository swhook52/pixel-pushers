using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 0.2f;
    public float dirX;
    public float stoppingDistance;
    public Transform target;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null)
        {
            anim.SetBool("isWalking", false);
            return;
        }

        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            anim.SetBool("isWalking", true);
            var direction = target.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = null;
        }
    }
}
