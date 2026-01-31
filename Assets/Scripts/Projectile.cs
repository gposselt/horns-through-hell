using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public Rigidbody2D rigidbody2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchProjectile(Vector2 velocity, float lifetime)
    {
        rigidbody2.linearVelocity = velocity;
        Destroy(gameObject, lifetime);
    }
}
