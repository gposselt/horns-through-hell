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

        var spawn = Instantiate(new GameObject());
        spawn.transform.position += new Vector3(1, 0, 0);
        

        player = Instantiate(playerPrefab);
        player.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        player.spawnpoint = spawn;
        
        gameObject.SetActive(false);


    }

    private void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
    }
}
