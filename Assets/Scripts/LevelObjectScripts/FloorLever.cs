using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class FloorLever : MonoBehaviour
{
    private SpriteRenderer buttonRenderer;
    private BoxCollider2D boxCollider;
    public Sprite unpressed;
    public Sprite pressed;

    public UnityEvent onPressEvent;

    public bool isPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
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

        if (collision.gameObject.CompareTag("PlayerCollision") || collision.gameObject.CompareTag("PlayerProjectileCollision"))
        {
            buttonRenderer.sprite = pressed;
            isPressed = true;
            onPressEvent.Invoke();
            boxCollider.enabled = false;
        }
    }
}
