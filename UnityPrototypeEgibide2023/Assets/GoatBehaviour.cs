using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GoatBehaviour : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public bool facingRight;
    // reference to player

    private Rigidbody2D _rb;

    [SerializeField] private GameObject eyes;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(LookForEnemy), 0, 0.01f);
    }

    // Update is called once per frame
    public void ActivateEnemy()
    {
        InvokeRepeating(nameof(Move), 0, 0.01f);
        CancelInvoke(nameof(LookForEnemy));
    }
    

    private void Death()
    {
        CancelInvoke(nameof(Move));
        Destroy(this);
    }
    
    // Move the goat using the rigidbody2D
    public void Move()
    {
        _rb.velocity = new Vector2(speed * (facingRight ? 1 : -1), _rb.velocity.y);
    }
    
    public void Jump() 
    {
        _rb.AddForce(new Vector2(_rb.velocity.x, jumpForce));
    }
    
    // Get the direction the goat is facing

    

    private void TurnAround()
    {
        facingRight = !facingRight;
    }
    

    private void LookForEnemy()
    {
        Debug.DrawRay(eyes.transform.position, (facingRight ? Vector2.right : Vector2.left) * 3f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, (facingRight ? Vector2.right : Vector2.left), 3f);
        
        if (hit.collider != null)
        {

            
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player Detected");
                ActivateEnemy();
            }
        }
    }

    public void BounceAgainstWall()
    {
        Debug.Log("BOUNCING");
        CancelInvoke(nameof(Move));
        TurnAround();
        
        // Add force to the goat like a jump
        _rb.velocity = new Vector2(_rb.velocity.x * -1, jumpForce);
        
    }
}
