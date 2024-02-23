using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    [SerializeField] private GameObject _gate2D;
    [SerializeField] private GameObject _gate3D;
    [SerializeField] private float delay;
    private static readonly int IsAbierta = Animator.StringToHash("IsAbierta");
    private static readonly int IsCerrada = Animator.StringToHash("IsCerrada");


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
        if (!other.CompareTag("PlayerHelper")) return;
        if (!other.name.Equals("EnemyDetection")) return;
        _gate2D.SetActive(false);
        _gate3D.GetComponent<Animator>().SetBool(IsAbierta, true);
        _gate3D.GetComponent<Animator>().SetBool(IsCerrada, false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerHelper")) return;
        if (!other.name.Equals("EnemyDetection")) return;
        Invoke(nameof(CloseGateAnim), delay - 0.4f);
    }

    private void CloseGateAnim()
    {
        _gate2D.SetActive(true);
        _gate3D.GetComponent<Animator>().SetBool(IsCerrada, true);
        _gate3D.GetComponent<Animator>().SetBool(IsAbierta, false);
    }
}
