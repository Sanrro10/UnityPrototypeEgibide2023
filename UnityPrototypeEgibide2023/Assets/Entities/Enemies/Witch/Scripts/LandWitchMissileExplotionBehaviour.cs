using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandWitchMissileExplotionBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Delete), 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
