using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 1.5f;

    private Vector2 direction;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }
    
    void Start()
    {
        Init(Vector2.left);
    }
    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
}