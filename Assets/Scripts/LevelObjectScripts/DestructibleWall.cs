using System;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    [SerializeField]
    private Sprite notBroken, broken;

    [SerializeField] private SpriteRenderer renderer;

    // [SerializeField] private BoxCollider2D boxCollider;

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
            renderer.sprite = broken;
        else
            renderer.sprite = notBroken;
        

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.CompareTag(""))
    }
}
