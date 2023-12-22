using Unity.VisualScripting;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class GaltzActiveZone : MonoBehaviour
    {
        [SerializeField] private GameObject[] galtzagorris;
        
        // Evento que activa el enemigo cuando entra en el rango
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            foreach (var galtzagorri in galtzagorris)
            {
                var script = galtzagorri.GetComponent<GaltzScript>();
                script.ActivateEnemy();
            }
            
        }
    
        // Evento que desactiva el enemigo cuando sale del rango
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            foreach (var galtzagorri in galtzagorris)
            {
                var script = galtzagorri.GetComponent<GaltzScript>();
                script.DeactivateEnemy();
            }
            
        }
    }
}
