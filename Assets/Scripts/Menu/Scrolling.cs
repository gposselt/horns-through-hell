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
        //scroll the cave image
        var rect = _caveimg.uvRect;
        rect.position += new Vector2(_cavex, _cavey) * Time.deltaTime;
        _caveimg.uvRect = rect;


        //scroll the water image
        var uvRect = _waterimg.uvRect;
        uvRect.position += new Vector2(_waterx, _watery) * Time.deltaTime;
        _waterimg.uvRect = uvRect;

    }
}
