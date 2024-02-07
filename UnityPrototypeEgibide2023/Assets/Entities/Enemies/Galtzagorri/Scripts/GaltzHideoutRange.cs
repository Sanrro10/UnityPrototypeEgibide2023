using System;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class GaltzHideoutRange : MonoBehaviour
    {
        public static event Action<GameObject> PlayerEntered;
        public static event Action<GameObject> PlayerExited; 
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("PlayerHelper"))
            {
                PlayerEntered?.Invoke(gameObject.transform.parent.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("PlayerHelper"))
            {
                PlayerExited?.Invoke(gameObject.transform.parent.gameObject);
            }
        }
    }
}
