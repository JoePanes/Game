using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Words : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 11) < 5)
        {
            gameObject.SetActive(false);
        }
    }
}
