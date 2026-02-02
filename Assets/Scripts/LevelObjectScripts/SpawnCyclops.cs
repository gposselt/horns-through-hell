using UnityEngine;

public class SpawnCyclops : MonoBehaviour
{
    public GameObject cyclops;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var a = Instantiate(cyclops);
        a.transform.SetPositionAndRotation(transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        
        //Delete ourselves
        Destroy(gameObject);
    }
    
}
