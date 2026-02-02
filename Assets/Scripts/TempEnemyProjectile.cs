using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SimpleProjectile : MonoBehaviour
{
    private CircleCollider2D circleCollider;

    public float speed = 8f;
    public float lifetime = 1.5f;

    private Vector3 direction;

    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }
    
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        //Init(Vector2.left);
    }
    void Update()
    {
        transform.position += (speed * Time.deltaTime * direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Hit by player projectile (parry)
        if (collision.gameObject.CompareTag("PlayerProjectileCollision"))
        {
            Destroy(gameObject);
        }
    }
}