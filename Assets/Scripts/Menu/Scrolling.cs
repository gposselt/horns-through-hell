using EditorAttributes;
using UnityEngine;
using UnityEngine.UI;

public class Scrolling : MonoBehaviour
{

    
    [SerializeField] private RawImage _caveimg;
    [SerializeField] private RectTransform caveTransform;
    
    [SerializeField] private RawImage _waterimg;
    [SerializeField] private RectTransform waterTransform;

    public RectTransform canvasTransform;

    [SerializeField] private float _cavex, _cavey;
    [SerializeField] private float _waterx, _watery;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        float canvasWidth = canvasTransform.rect.width;
        
            
        
        //scroll the cave image
        var rect = _caveimg.uvRect;
        rect.position += new Vector2(_cavex, _cavey) * Time.deltaTime;
        _caveimg.uvRect = rect;
        
        //transform the cave image
        var caveTexture = _caveimg.mainTexture;

        float caveAspectRatio = ((float)caveTexture.width) / caveTexture.height;

        Rect caveImageUVRect = _caveimg.uvRect;
        float caveUvWidth = canvasWidth / (caveAspectRatio * caveTransform.rect.height);
        caveImageUVRect.width = caveUvWidth;
        
        _caveimg.uvRect = caveImageUVRect;

        ((RectTransform)_caveimg.transform).sizeDelta = new Vector2(canvasWidth, caveTransform.sizeDelta.y);

        //scroll the water image
        var uvRect = _waterimg.uvRect;
        uvRect.position += new Vector2(_waterx, _watery) * Time.deltaTime;
        _waterimg.uvRect = uvRect;
        
        //transform the water image
        var waterTexture = _waterimg.mainTexture;

        float waterAspectRatio = ((float)waterTexture.width) / waterTexture.height;

        Rect waterImageUVRect = _waterimg.uvRect;
        float waterUvWidth = canvasWidth / (waterAspectRatio * waterTransform.rect.height);
        waterImageUVRect.width = waterUvWidth;
        
        _waterimg.uvRect = waterImageUVRect;

        ((RectTransform)_waterimg.transform).sizeDelta = new Vector2(canvasWidth, waterTransform.sizeDelta.y);


    }
}
