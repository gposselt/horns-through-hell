using System;
using EditorAttributes;
using UnityEngine;

public class PlayerSpawnBlock : MonoBehaviour
{

    public static PlayerSpawnBlock Instance;

    public Player playerPrefab;
    [ReadOnly] public Player player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        player = Instantiate(playerPrefab);
        
        gameObject.SetActive(false);


    }

    private void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
    }
}
