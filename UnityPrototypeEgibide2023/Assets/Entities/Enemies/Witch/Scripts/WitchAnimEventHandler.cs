using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchAnimEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WitchLaught()
    {
        gameObject.GetComponent<Animator>().SetInteger("WitchLaught", Random.Range(1, 11));
    }

    public void WitchStopLaught()
    {
        gameObject.GetComponent<Animator>().SetInteger("WitchLaught", 1);
    }
}
