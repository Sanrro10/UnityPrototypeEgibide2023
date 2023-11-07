using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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


    private bool _waiting;
    private bool _waitingForPlayer;
    private Vector3 _lastAvailablePosition;
    private bool _hiding;
    private bool _hidden;
    private bool _attacking;
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
      
        if (CanReachPlayer())
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
            else if(!_hiding)
            {
                Debug.Log("5");
                if (_playerNavMesh.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    Debug.Log("6");
                    FollowPlayer(false);
                }
            }

            if (!(Vector3.Distance(gameObject.transform.position, player.transform.position) < 1.5)) return;
            if (!_attacking)
            {
                StartCoroutine(nameof(Attack));
                Hide();
            }
            
        }
        else
        {
            if (_hiding || _hidden) return;
            _navMeshAgent.SetDestination(_lastAvailablePosition);
            if (!_waitingForPlayer)
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
        if (_hiding || _hidden) return;
        _hiding = true;
        var placeToHide = Random.Range(0, hideouts.Length);
        if (placeToHide < 0 || placeToHide >= hideouts.Length) return;
        Vector3 whereToHide = hideouts[placeToHide].transform.position;
        _navMeshAgent.SetDestination(whereToHide);
        
    }
    
    private void Appear()
    {
        _followPlayer = true;
        _isHidden = false; 
        Debug.Log("APAREZCOOOO");
        int placeToAppear = Random.Range(0, hideouts.Length);
        if (placeToAppear < 0 || placeToAppear >= hideouts.Length) return;
        gameObject.transform.position = hideouts[placeToAppear].transform.position;
        _hidden = false;
        _hiding = false;
    }

    private IEnumerator Attack()
    {
        _attacking = true;
        Debug.Log("ATACOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        yield return new WaitForSeconds(0.5f);
        _attacking = false;
    }

    public void MakeAttack() {
        
    }

    IEnumerator UpdatePathing()
    {
        _playerNavMesh.SetDestination(player.transform.position);
        yield return null;
    }

    public void ActivateEnemy()
    {
        CancelInvoke(nameof(ChasePlayer));
        InvokeRepeating(nameof(ChasePlayer), 0f, 0.1f);
    }

    public void DeactivateEnemy()
    {
        CancelInvoke(nameof(ChasePlayer));
    }

    public void ActivateHiding(Collider2D other)
    {
        if (Vector3.Distance(other.gameObject.transform.position, _navMeshAgent.destination) < 1 && _hiding)
        {
            _hiding = false;
            _hidden = true;
        }
    }
    
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