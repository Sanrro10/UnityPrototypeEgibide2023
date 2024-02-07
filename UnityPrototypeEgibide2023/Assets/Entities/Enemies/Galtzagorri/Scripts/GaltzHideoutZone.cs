using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class GaltzHideoutZone : MonoBehaviour
    {
         private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject parent = transform.parent.gameObject;
            var entity = parent.GetComponent<NewGaltzScript>();
            if (entity.StateMachine.CurrentState != entity.StateMachine.GaltzHidingState) return;
            if (other.gameObject.name.Equals("HideoutCollider"))
            {
                entity.StateMachine.TransitionTo(entity.StateMachine.GaltzHiddenState);
            }
            
            
        }
    }
}
