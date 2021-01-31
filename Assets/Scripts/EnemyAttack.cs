using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator anim;
    bool attack;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(attack) {
            Debug.Log("ATTACK");
            anim.SetTrigger("hit");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {    
        if (collision.CompareTag("Player"))
        {
            attack = true;
            Debug.Log("attack In range");
            // anim.SetBool("isWalking", false);
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            attack = false;
            Debug.Log("attack out of range");
            anim.SetBool("isWalking", true);
        }  
    }
}
