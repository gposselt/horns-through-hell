using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleFloor : MonoBehaviour
{
    [SerializeField]
    private Sprite notBroken, broken;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private BoxCollider2D PlayerCollider;

    private Coroutine DestructionDelayCoroutine = null;

    private bool destroyed = false;

    public float destructionDelay = 1.5f, undestructionDelay = 1.5f;
    
    
    public bool queueDestruction;

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

        if (queueDestruction && DestructionDelayCoroutine == null)
        {
            DestructionDelayCoroutine = StartCoroutine(DestroyAfterTimeThenReform());
            queueDestruction = false;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("PlayerCollision") )
        {
            Debug.Log("Player Collision detected!");

            if (DestructionDelayCoroutine == null)
            {
                //destroy after short time and then reform
                DestructionDelayCoroutine = StartCoroutine(DestroyAfterTimeThenReform());
            }
            else
            {
                queueDestruction = true;
            }
        }
        
        Debug.Log($"Collision detected! {other}");

    }
 

    IEnumerator DestroyAfterTimeThenReform()
    {
        yield return new WaitForSeconds(destructionDelay);
        
        Destroyed = true;
        
        yield return new WaitForSeconds(undestructionDelay);
        
        Destroyed = false;

        //wait for a little  so that it won't immediatley detect it again
        yield return new WaitForSeconds(destructionDelay);

        DestructionDelayCoroutine = null;
    }
}
