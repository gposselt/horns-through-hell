using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class DoorScript : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void OpenDoor()
    {
        // Reduce transparency
        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;

        // Get rid of the box collider (allows character through)
        boxCollider.enabled = false;
    }

    public void CloseDoor()
    {
        Color color = spriteRenderer.color;
        color.a = 1.0f;
        spriteRenderer.color = color;

        boxCollider.enabled = true;
    }
}
