using System.Collections;
using UnityEngine;

public class FlamePillarController : MonoBehaviour
{
    public Inactive inactiveSprite;
    public Active activeSprite;
    // public BoxCollider2D activeCollider;

    public bool active;

    public Coroutine ActiveCoroutine;

    
    IEnumerator SetActive()
    {

        if (!inactiveSprite)
        {
            inactiveSprite = GetComponentInChildren<Inactive>();
        }

        if (!activeSprite)
        {
            var activeDelegate = GetComponentInChildren<Active>();
            activeSprite = activeDelegate;
        }
        
        while (true)
        {
            if (active && inactiveSprite.inactiveSprite.enabled)
            {
                inactiveSprite.inactiveSprite.enabled = false;
                activeSprite.activeSprite.enabled = true;
                activeSprite.boxCollider.enabled = false;
            }
            else if (!active && activeSprite.activeSprite.enabled)
            {
                activeSprite.activeSprite.enabled = false;
                inactiveSprite.inactiveSprite.enabled = true;
                activeSprite.boxCollider.enabled = true;
            }

            yield return new WaitForFixedUpdate();


        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ActiveCoroutine == null)
            ActiveCoroutine = StartCoroutine(SetActive());
    }
}
