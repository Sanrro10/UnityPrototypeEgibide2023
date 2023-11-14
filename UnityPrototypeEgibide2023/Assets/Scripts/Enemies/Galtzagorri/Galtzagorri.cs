using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Galtzagorri : EntityControler
{
    [SerializeField] private GameObject player;
    [SerializeField] private BasicEnemyData basicEnemyData;
    [SerializeField] private GameObject[] hideouts;

    [SerializeField]
    private HideoutZone scriptHideout;
    private NavMeshAgent _navMeshAgent;
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
        _navMeshAgent.acceleration = Random.Range(15, 30);
        
        //Set the Health Points
        gameObject.GetComponent<HealthComponent>().SendMessage("Set", basicEnemyData.health);
    }

    private void ChasePlayer()
    {
        if (CanReachPlayer())
        {
            Debug.Log(_waiting);
            CancelInvoke(nameof(Hide));
            _waitingForPlayer = false;
            if (_waiting) return;
            if (_hidden)
            {
                StartCoroutine(Appear());
            }
            else if(!_hiding)
            {
                _navMeshAgent.SetDestination(player.transform.position);
            }

            if ((Vector3.Distance(gameObject.transform.position, player.transform.position) < 3) && !_attacking)
            {
                StartCoroutine(nameof(Attack));
            }
            
        }
        else
        {
            if (_hiding || _hidden) return;
            _navMeshAgent.SetDestination(_lastAvailablePosition);
            if (!_waitingForPlayer)
            {
                _waitingForPlayer = true;
                CancelInvoke(nameof(Hide));
                Invoke(nameof(Hide), 2f);
            }
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

    private IEnumerator Appear()
    {
        _waiting = true;
        yield return new WaitForSeconds(2f);
        _waiting = false;
        int placeToAppear = Random.Range(0, hideouts.Length);
        if (placeToAppear < 0 || placeToAppear >= hideouts.Length) yield break;
        gameObject.transform.position = hideouts[placeToAppear].transform.position;
        _hidden = false;
        _hiding = false;
    }

    private IEnumerator Attack()
    {
        _attacking = true;
        CancelInvoke(nameof(ChasePlayer));
        _navMeshAgent.enabled = false;
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
        rb2D.AddForce(new Vector2((player.transform.position.x - transform.position.x) * 2, 4), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.8f);
        _navMeshAgent.enabled = true;
        ActivateEnemy();
        Hide();
        yield return new WaitForSeconds(1f);
        _attacking = false;
    }

    private bool CanReachPlayer()
    {
        NavMeshPath path = new NavMeshPath();
        if (_navMeshAgent.CalculatePath(player.transform.position, path))
        {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                _lastAvailablePosition = player.transform.position;
                return true;
            }
        }
        return false;
    }

    public void ActivateEnemy()
    {
        CancelInvoke(nameof(ChasePlayer));
        InvokeRepeating(nameof(ChasePlayer), 0f, 0.1f);
        scriptHideout.ResetHit();
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
