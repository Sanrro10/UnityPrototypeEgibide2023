namespace Entities
{
    public class Zarzas : EntityControler
    {
        public void Start()
        {
           Health.Set(1);
        }
        
        public override void OnReceiveDamage(AttackComponent.AttackData attack, bool toTheRight = true)
        {
            // nothing 
        }
        public override void OnDeath()
        {
            
        }
    }
}