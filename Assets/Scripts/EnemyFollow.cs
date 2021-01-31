using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 0.2f;
    public float dirX;
    public float stoppingDistance;
    public Transform target;
    Animator anim;
    bool attack;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
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
            Debug.Log("enemy follow enter");
            anim.SetBool("isWalking", true);
            target = collision.transform;
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("enemy follow exit");
            // target = null;
            // anim.SetBool("isWalking", false);
        }  
    }
}
