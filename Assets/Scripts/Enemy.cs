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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Hit by player projectile
        if (collision.gameObject.CompareTag("PlayerProjectileCollision"))
        {
            //double damage lol
            if (collision.gameObject.GetComponent<Projectile>() is TubaBlast)
                health--;
            
            health--;
            // Play the appropriate damage animation: Hazel help!
            if (health <= 0)
            {
                // Play a death sound if any
                //SoundFXManager.Instance.PlaySoundFXClip(...);

                // Drop mask (instantiate a prefab for the mask), then destroy
                Destroy(gameObject);
            }
        }
    }
}
