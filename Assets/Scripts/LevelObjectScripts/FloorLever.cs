using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class FloorLever : MonoBehaviour
{
    private SpriteRenderer buttonRenderer;
    public Sprite unpressed;
    public Sprite pressed;

    public UnityEvent onPressEvent;

    public bool isPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonRenderer = GetComponent<SpriteRenderer>();
        buttonRenderer.sprite = unpressed;
        isPressed = false;
        transform.position += new Vector3(0.0f, 0.0f, 1.0f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPressed)
        {
            return;
        }

        if (collision.gameObject.CompareTag("PlayerCollision"))
        {
            buttonRenderer.sprite = pressed;
            isPressed = true;
            onPressEvent.Invoke();
        } else if (collision.gameObject.CompareTag("PlayerProjectileCollision"))
        {
            buttonRenderer.sprite = pressed;
            isPressed = true;
            onPressEvent.Invoke();
            // Remember to destroy the projectile!
            Destroy(collision.gameObject);
        }
    }
}
