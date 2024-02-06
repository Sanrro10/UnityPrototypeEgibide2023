using UnityEngine;

namespace Entities.Enemies.Witch.Scripts
{
    public class LWMissileActivation : MonoBehaviour
    {
        [SerializeField] private LandWitch parentScript;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Checks if the player has entered the Missile Range
            if (other.CompareTag("PlayerHelper"))
            {
                parentScript.SendMessage("SetMissilePossible", true, SendMessageOptions.RequireReceiver);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //Checks if the player has left the Missile Range
            if (other.CompareTag("PlayerHelper"))
            {
                parentScript.SendMessage("SetMissilePossible", false, SendMessageOptions.RequireReceiver);
            }
        }
    }
}
