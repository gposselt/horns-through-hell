using UnityEngine;
using UnityEngine.UI;

public class Scrolling : MonoBehaviour
{

    
    [SerializeField] private RawImage _caveimg;
    [SerializeField] private RawImage _waterimg;
    [SerializeField] private float _cavex, _cavey;
    [SerializeField] private float _waterx, _watery;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _caveimg.uvRect = new Rect(_caveimg.uvRect.position + new Vector2(_cavex, _cavey) * Time.deltaTime, _caveimg.uvRect.size);
        _waterimg.uvRect = new Rect(_waterimg.uvRect.position + new Vector2(_waterx, _watery) * Time.deltaTime, _waterimg.uvRect.size);
    }
}
