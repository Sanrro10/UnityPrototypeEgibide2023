using UnityEngine;

namespace Entities.Enemies.Gizotso.Scripts
{
    public class GizotsoActiveZone : MonoBehaviour
    {
    
        // Evento que activa el ataque del Gizotso
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var script = transform.parent.GetComponent<Gizotso>();
            script.Attack();
        }
    }
}
