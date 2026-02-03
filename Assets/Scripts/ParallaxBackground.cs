using System;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Player player;

    public Canvas hostCanvas;
    private RectTransform canvasRectTransform;

    private RectTransform rectTransform;

    private float aspectRatio;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
        canvasRectTransform = hostCanvas.GetComponent<RectTransform>();
        
        aspectRatio = rectTransform.rect.width / rectTransform.rect.height;



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        FixScale();
    }


    void FixScale()
    {
        
        float height = rectTransform.rect.height;
        float width = rectTransform.rect.width;
        
        // Debug.Log(canvasRect);

        Rect canvasRect = canvasRectTransform.rect;


        height = canvasRect.height;
        width = canvasRect.width * aspectRatio;
        
        // Debug.Log($"W: {width} | H: {height}");

        rectTransform.sizeDelta = new Vector2(width, height);

    }
}
