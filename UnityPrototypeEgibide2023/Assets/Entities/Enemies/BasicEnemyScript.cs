using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemies
{
    public class EnemyMovement : EntityControler
    {
        [SerializeField] private GameObject player;
        [SerializeField] private BasicEnemyData basicEnemyData;
        private NavMeshAgent _navMeshAgent;
    
    
    
        private Vector3 origin;
        private bool _hidding = false;
        // Start is called before the first frame update
        void Start()
        {
            origin = transform.position;
            _navMeshAgent = GetComponent<NavMeshAgent>();
        
            //Set the Health Points
            gameObject.GetComponent<HealthComponent>().SendMessage("Set", basicEnemyData.health);
        
            InvokeRepeating(nameof(ChasePlayer), 0f, 0.25f);
        }

        void ChasePlayer()
        {
            if (CanReachPlayer())
            {
                Debug.Log("Puedo alcanzar al jugador");
                _hidding = false;
                _navMeshAgent.SetDestination(player.transform.position);
            }
            else
            {
                Debug.Log("No puedo alcanzar al jugador");
                if(_hidding) return;
                _hidding = true;
                Hide();
            }
        }
    
        void Hide()
        {
            //Change this to get a random hide location
            _navMeshAgent.SetDestination(origin);
        }
        
        

    
        bool CanReachPlayer()
        {
            NavMeshPath path = new NavMeshPath();
            if (_navMeshAgent.CalculatePath(player.transform.position, path))
            {
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    return true;
                }
            }
            return false;
        }


    

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                CancelInvoke(nameof(ChasePlayer));
                InvokeRepeating(nameof(ChasePlayer), 0f, 0.25f);
            }
        
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                CancelInvoke(nameof(ChasePlayer));
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
}
