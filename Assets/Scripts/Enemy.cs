using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Hit by player projectile
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectileCollision"))
        {
            health--;

            if (health <= 0)
            {
                // Play a death sound (placeholder)
                //SoundFXManager.Instance.PlaySoundFXClip(...);
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
        }
    }
}
