using System;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    [SerializeField]
    private Sprite notBroken, broken;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private BoxCollider2D PlayerCollider;

    private bool destroyed = false; 
    
    public bool Destroyed
    {
        get => destroyed;

        set => SetDestroyed(value);

    }

    private void SetDestroyed(bool value)
    {
        destroyed = value;

        if (destroyed)
            spriteRenderer.sprite = broken;
        else
            spriteRenderer.sprite = notBroken;
        
        

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Destroyed && spriteRenderer.sprite != broken)
        {
            spriteRenderer.sprite = broken;
        }
        else if (!Destroyed && spriteRenderer.sprite != notBroken)
        {
            spriteRenderer.sprite = notBroken;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Destroyed && PlayerCollider.enabled)
        {
            PlayerCollider.enabled = false;
        }
        else if(!Destroyed && !PlayerCollider.enabled)
        {
            PlayerCollider.enabled = true;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("PlayerProjectileCollision"))
        {
            Debug.Log("Player Collision detected!");
            Destroyed = true;
        }
        
        Debug.Log($"Collision detected! {other}");

    }
}
