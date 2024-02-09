using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class GaltzHideoutZone : MonoBehaviour
    {
        // Hace meterse al galtzagorri en el estado "Hidden" cuando su estado el "Hiding" y se encuentra con el escondite asignado
        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject parent = transform.parent.gameObject;
            var entity = parent.GetComponent<NewGaltzScript>();
            if (entity.StateMachine.CurrentState != entity.StateMachine.GaltzHidingState) return;
            if (other.gameObject.name.Equals("HideoutCollider") &&
                entity.currentHideout.Equals(other.transform.parent.gameObject))
            {
                entity.StateMachine.TransitionTo(entity.StateMachine.GaltzHiddenState);
            }
        }
    }
}
