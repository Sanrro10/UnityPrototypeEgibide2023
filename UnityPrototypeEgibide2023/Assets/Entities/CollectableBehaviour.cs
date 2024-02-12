using General.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    public class CollectableBehaviour : MonoBehaviour
    {
        public int id;
        private GameObject _collectableImage;
        private GameObject _infoText;
        void Start()
        {
            Search();
            _collectableImage = gameObject.transform.Find("Sprite").gameObject;
            _infoText = gameObject.transform.Find("Info").gameObject;
        }

        protected virtual void Effect()
        {
            _collectableImage.SetActive(false);
            _infoText.GetComponent<TextMeshPro>().text = GameController.Instance.collectedItems.Count + "/10";
            _infoText.SetActive(true);
            Invoke(nameof(Delete), 3f);
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

        protected virtual void Delete()
        {
            Destroy(gameObject);
        }

    }
}
