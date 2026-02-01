using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class FloorButton : MonoBehaviour
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
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollision") && collision.GetContact(0).normal.y < -0.5f)
        {
            buttonRenderer.sprite = pressed;
            isPressed = true;
            boxCollider.enabled = false;
            transform.position += new Vector3(0.0f, 0.0f, 1.0f);
            onPressEvent.Invoke();
        }
    }

}
