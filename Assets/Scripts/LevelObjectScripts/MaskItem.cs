using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MaskItem : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public Masks maskType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Pick up the mask.
        if (collision.gameObject.CompareTag("PlayerCollision"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            // Only pick up if the player is not full of ammo.
            if (player.ammo.remainingAmmo[maskType] < AmmoHolder.maxAmmos[maskType])
            {
                player.ammo.remainingAmmo[maskType] = AmmoHolder.maxAmmos[maskType];
                Destroy(gameObject);
            }
        }
    }
}
