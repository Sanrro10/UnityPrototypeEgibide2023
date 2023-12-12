using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class GaltzagorriActiveZone : MonoBehaviour
    {
        private GameObject _parent;

        void Start()
        {
            _parent = transform.parent.gameObject;
        }

        // Evento que activa el enemigo cuando entra en el rango
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var script = _parent.GetComponent<Galtzagorri>();
            script.ActivateEnemy();
        }
    
        // Evento que desactiva el enemigo cuando sale del rango
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var script = _parent.GetComponent<Galtzagorri>();
            script.DeactivateEnemy();
        }
    }
}
