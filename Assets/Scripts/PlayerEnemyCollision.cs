using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerEnemyCollision : MonoBehaviour
{
    private Player player;
    private BoxCollider2D boxCollider;
    [SerializeField] private float iframeTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        player = gameObject.GetComponentInParent<Player>();
        iframeTimer = 0.0f;
    }

    public void Update()
    {
        if (iframeTimer >= 0.0f)
        {
            iframeTimer -= Time.deltaTime;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && iframeTimer <= 0.0f)
        {
            Debug.Log("Player hit enemy!");
            player.playerHp--; // Get the player component and reduce hp by 1.
            // Give about half a second of iframes.
            iframeTimer = 0.5f;
            // Play an animation on the player that indicates damage... Hazel help!
        }
    }
}
