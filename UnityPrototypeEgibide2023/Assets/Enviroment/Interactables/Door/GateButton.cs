using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    [SerializeField] private GameObject _gate2D;
    [SerializeField] private GameObject _gate3D;
    [SerializeField] private float delay;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _gate2D.SetActive(false);
        _gate3D.GetComponent<Animator>().SetBool("Action", true);
        Invoke(nameof(CloseGate),delay);
    }

    private void CloseGate()
    {
        _gate2D.SetActive(true);
        _gate3D.GetComponent<Animator>().SetBool("Action", false);
    }
}
