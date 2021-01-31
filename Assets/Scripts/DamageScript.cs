using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public void DealDamage() {
        int rand = Random.Range(0, 5);
        Game.Instance.RemoveHealth(rand);
    }
}
