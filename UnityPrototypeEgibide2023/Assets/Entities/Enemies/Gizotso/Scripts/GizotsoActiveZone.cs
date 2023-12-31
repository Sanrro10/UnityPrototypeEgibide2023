using UnityEngine;

namespace Entities.Enemies.Gizotso.Scripts
{
    public class GizotsoActiveZone : MonoBehaviour
    {
    
        // Evento que activa el ataque del Gizotso
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("El otro es " + other.tag + "de nombre " + other.name);
            if (!other.gameObject.CompareTag("Player")) return;
            var script = transform.parent.GetComponent<Gizotso>();
            script.Attack();
        }
    }
}
