using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerAttack : MonoBehaviour
{

    private void OnParticleCollision(GameObject other)
    {
        //If particle collides with an enemy, destroy them and itself
        if (other.CompareTag("Enemy"))
        {
            Destroy(other);
        }
    }
}
