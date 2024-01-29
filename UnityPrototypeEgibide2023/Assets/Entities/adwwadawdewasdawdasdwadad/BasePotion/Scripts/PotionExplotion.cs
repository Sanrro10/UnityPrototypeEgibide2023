using UnityEngine;

namespace Entities.Potions.BasePotion.Scripts
{
    public class PotionExplotion : MonoBehaviour
    {
        private float _x = 0;
        private float _y = 0;
    
        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("grow",0f, 0.1f);
            Invoke("die",0.5f);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void grow()
        {
            _x += 1;
            _y += 1;
        
            transform.localScale = new Vector3(_x, _y, 0f);
        }

        private void die()
        {
            Destroy(this.gameObject);
        }
    }
}
