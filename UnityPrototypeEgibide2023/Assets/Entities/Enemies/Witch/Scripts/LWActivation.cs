using UnityEngine;

namespace Entities.Enemies.Witch.Scripts
{
    public class LWActivation : MonoBehaviour
    {
        [SerializeField] private LandWitch parentScript;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            /*Checks if the player has entered the witch's action zone*/
            if (other.CompareTag("Player"))
            {
                parentScript.SendMessage("SetActiveState" , true, SendMessageOptions.RequireReceiver);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            /*Checks if the player has left the witch's action zone*/
            if (other.CompareTag("Player"))
            {
                parentScript.SendMessage("SetActiveState" , false , SendMessageOptions.RequireReceiver);
            
            }
        }
    }
}
