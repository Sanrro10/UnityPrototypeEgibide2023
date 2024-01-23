using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRockas : MonoBehaviour
{
    //Tiempo para que empiezen a spawnear
    [SerializeField] private float tiempoInical;
    //Tiempo entre spawns
    [SerializeField] private float tiempoRepeticion;
    //Objeto a spawnear
    [SerializeField] private GameObject rock;
    
    
    void Start()
    {
        InvokeRepeating("SpawnRocks", tiempoInical, tiempoRepeticion);
    }
    
    private void SpawnRocks()
    { 
        Instantiate(rock, transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
    }
    
}
