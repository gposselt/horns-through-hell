using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CymbalProjectile : Projectile
{
    public float maxLifetime = 2.0f;
    public float noteLifetime = 2.0f;
    public float upVelocity = 1.0f;

    public SpriteRenderer newRenderer;


    void Update()
    {

        if (noteLifetime > Constants.TimeEpsilon && newRenderer)
        {
            Color tempColor = newRenderer.color;
            newRenderer.color = new Color(tempColor.r, tempColor.g, tempColor.b, Mathf.Sqrt(noteLifetime / maxLifetime));
            transform.localScale += new Vector3(1.0f * Time.deltaTime, 1.0f * Time.deltaTime, 0.0f);
            noteLifetime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    
    public override void LaunchProjectile(Vector2 velocity, float lifetime)
    {
        // base.LaunchProjectile(velocity, lifetime);
        // noteLifetime = lifetime;

        rigidbody2.linearVelocityY = upVelocity;
        
        //apply AoE effect
        var colliders = Physics2D.OverlapCircleAll(transform.position, 7.5f);

        foreach (var collider1 in colliders)
        {
            //damage
            if (collider1.transform.CompareTag("Enemy"))
            {
                Enemy enemy = collider1.gameObject.GetComponent<Enemy>();
                enemy.health--;
                // Play the appropriate damage animation: Hazel help!
                if (enemy.health <= 0)
                {
                    // Play a death sound if any
                    //SoundFXManager.Instance.PlaySoundFXClip(...);

                    // Drop mask (instantiate a prefab for the mask), then destroy
                    Destroy(collider1.gameObject);
                }
            }
        }


    }
    
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        return;
    }
}
