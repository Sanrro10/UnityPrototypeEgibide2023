using UnityEngine;

namespace Entities.Enemies.Witch.Scripts
{
    public class LWMagicCircleActivation : MonoBehaviour
    {
        [SerializeField] private LandWitch parentScript;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Checks if the player has entered the MagicCircle range
            if (other.CompareTag("Player"))
            {
                parentScript.SendMessage("SetMagicCirclePossible", true, SendMessageOptions.RequireReceiver);
            }
        }
 
        private void OnTriggerExit2D(Collider2D other)
        {
            //Checks if the player has left the MagicCircle range
            if (other.CompareTag("Player"))
            {
                parentScript.SendMessage("SetMagicCirclePossible", false, SendMessageOptions.RequireReceiver);
            }
        }
    }
}
