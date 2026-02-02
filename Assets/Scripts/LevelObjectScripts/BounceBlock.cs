using UnityEngine;

public class BounceBlock : MonoBehaviour
{

    public float bounceVelocity = 50.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player collides from the top
        if (collision.gameObject.CompareTag("PlayerCollision") && collision.GetContact(0).normal.y < -0.5f)
        {
            collision.rigidbody.linearVelocityY = bounceVelocity;
        }
    }
}
