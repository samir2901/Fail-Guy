using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpheres : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] spheres;
    public float startDelay = 2.0f;
    public float spawnRate = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnSpheres", startDelay, spawnRate);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnSpheres()
    {
        int sphereIndex = Random.Range(0, spheres.Length);
        int posIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(spheres[sphereIndex], spawnPoints[posIndex].position, spheres[sphereIndex].transform.rotation);
    }
}
