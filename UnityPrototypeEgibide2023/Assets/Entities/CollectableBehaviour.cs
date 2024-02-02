using General.Scripts;
using UnityEngine;

namespace Entities
{
    public class CollectableBehaviour : MonoBehaviour
    {
        public int id;
    
        void Start()
        {
            Search();
        }

        protected virtual void Effect()
        {
            Destroy(this);
        }

        protected virtual void Search()
        {
            foreach (int i in GameController.Instance.collectedItems)
            {
                if (i == id)
                {
                    Destroy(gameObject);
                    return;
                }
            }
            Effect();
        }
    
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 7)
            {
                GameController.Instance.collectedItems.Add(id);
            }
        }

    }
}
