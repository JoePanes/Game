using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotation : MonoBehaviour
{
    private float speed = 200;

    void Update()
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
