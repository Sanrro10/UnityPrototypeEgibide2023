using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    [SerializeField] private GameObject _gate;
    
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
        _gate.SetActive(false);
        Invoke(nameof(CloseGate),1f);
    }

    private void CloseGate()
    {
        _gate.SetActive(true);
    }
}
