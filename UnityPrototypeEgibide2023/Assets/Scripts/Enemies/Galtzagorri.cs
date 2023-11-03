using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Galtzagorri : EntityControler
{
    [SerializeField] private GameObject player;
    [SerializeField] private BasicEnemyData basicEnemyData;
    [SerializeField] private GameObject[] hideouts;
    [SerializeField] private GameObject playerDestinationChecker;
    
    private Vector3 _position;
    private NavMeshAgent _navMeshAgent;
    private NavMeshAgent _playerNavMesh;
    private bool _followPlayer = false;

    private bool _isHidden = false;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerNavMesh = playerDestinationChecker.GetComponent<NavMeshAgent>();
        _playerNavMesh.SetDestination(player.transform.position);
        
        //Set the Health Points
        gameObject.GetComponent<HealthComponent>().SendMessage("Set", basicEnemyData.health);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_playerNavMesh.pathStatus);
        if (!_isHidden)
        {
            Debug.Log("1");
            if (_navMeshAgent.destination == player.transform.position)
            {
                Debug.Log("2");
                if (_playerNavMesh.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    Debug.Log("3");
                    _navMeshAgent.SetDestination(player.transform.position);
                    if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 1.5)
                    {
                        Attack();
                        Hide();
                    }
                }
                else
                {
                    Debug.Log("4");
                    Hide();
                }
            }
            else
            {
                Debug.Log("5");
                if (_playerNavMesh.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    Debug.Log("6");
                    FollowPlayer(false);
                }
            }
        }
        else
        {
            Debug.Log("7");
            if (_playerNavMesh.pathStatus == NavMeshPathStatus.PathComplete)
            {
                Debug.Log("8");
                FollowPlayer(true);
            }
        }
        
    }

   

    private void Attack()
    {
        Debug.Log("TE ATACOOOOOOO");
    }

    private void FollowPlayer(bool fromHiding)
    {
        if (fromHiding)
        {
            StartCoroutine(ChangeHideout());
        }
        else
        {
           _followPlayer = true;
           _isHidden = false;
           _navMeshAgent.SetDestination(player.transform.position);
        }
        
    }

    private void Hide()
    {
        _followPlayer = false;
        var placeToHide = Random.Range(0, hideouts.Length);
        if (placeToHide < 0 || placeToHide >= hideouts.Length) return;
        Vector3 whereToHide = hideouts[placeToHide].transform.position;
        _navMeshAgent.SetDestination(whereToHide);
        _isHidden = true;
    }
    
    private void Appear()
    {
        _followPlayer = true;
        _isHidden = false; 
        Debug.Log("APAREZCOOOO");
        int placeToAppear = Random.Range(0, hideouts.Length);
        if (placeToAppear < 0 || placeToAppear >= hideouts.Length) return;
        gameObject.transform.position = hideouts[placeToAppear].transform.position;
    }

    IEnumerator ChangeHideout()
    {
        yield return new WaitForSeconds(3f);
        Appear();
        
    }

    IEnumerator UpdatePathing()
    {
        _playerNavMesh.SetDestination(player.transform.position);
        yield return null;
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _followPlayer = true;
        }
        
    }

    /*private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _followPlayer = false;
        }
    }*/
    
    public override void OnDeath()
    {
        //TO-DO Activar animacion de muerte y logica relacionada
        
        Invoke(nameof(DestroyThis),2f);
        
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}