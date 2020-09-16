using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySphere : MonoBehaviour
{
    
    void Update()
    {
        if(transform.position.y < -50)
        {
            Destroy(gameObject);
        }
        
    }
}
