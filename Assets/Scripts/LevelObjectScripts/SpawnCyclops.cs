using UnityEngine;

public class SpawnCyclops : MonoBehaviour
{
    public GameObject cyclops;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(cyclops);
        cyclops.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        
        //Delete ourselves
        Destroy(gameObject);
    }
    
}
