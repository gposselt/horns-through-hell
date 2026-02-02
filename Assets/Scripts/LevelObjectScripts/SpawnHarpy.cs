using UnityEngine;

public class SpawnHarpy : MonoBehaviour
{
    public GameObject harpy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var askldfals = Instantiate(harpy);
        askldfals.transform.SetPositionAndRotation(transform.position + new Vector3(1, 0, 0), Quaternion.identity);

        
        Destroy(gameObject);
    }

}
