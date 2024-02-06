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
            Destroy(gameObject);
        }

        protected virtual void Search()
        {
            GameController.Instance.collectedItems.ForEach(i =>
            {
                if (i == id) Destroy(this.gameObject);
            });
            
        }
    
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 6)
            {
                GameController.Instance.collectedItems.Add(id);
                Effect();
            }
        }

    }
}
