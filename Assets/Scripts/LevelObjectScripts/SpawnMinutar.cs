using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject minotaur;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var askldfals = Instantiate(minotaur);
        askldfals.transform.SetPositionAndRotation(transform.position + new Vector3(1, 0, 0), Quaternion.identity);

        
        Destroy(gameObject);
    }

}
