using UnityEngine;

public class TubaBlast : Projectile
{
    public SpriteRenderer newRenderer;
    
    public float maxLifetime = 2.0f;
    public float noteLifetime = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if (noteLifetime > Constants.TimeEpsilon && newRenderer)
        {
            Color tempColor = newRenderer.color;
            newRenderer.color = new Color(tempColor.r, tempColor.g, tempColor.b, Mathf.Sqrt(noteLifetime / maxLifetime));
            transform.localScale += new Vector3(1.0f * Time.deltaTime / 2, 1.0f * Time.deltaTime / 2, 0.0f);
            noteLifetime -= Time.deltaTime / 2;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void LaunchProjectile(Vector2 velocity, float lifetime)
    {
        base.LaunchProjectile(velocity / 3, lifetime * 4);
    }


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            collision.gameObject.GetComponent<Enemy>().health -= 2;
        }
    }
}
