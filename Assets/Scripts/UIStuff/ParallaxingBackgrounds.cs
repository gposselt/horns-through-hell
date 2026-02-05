using System;
using EditorAttributes;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;



[Serializable]
public struct EditorParallaxLayer
{
    public RawImage layerImage;
    public float distance; // should this be parallax scale instead?
    public float pixelsPerUnit;

}

[Serializable]
public class ParallaxLayer
{
    public RawImage layerImage; 
    public float distance; // should this be parallax scale instead?
    public float pixelsPerUnit;

    public readonly float gameUnitsConversionFactor;
    public Vector2 originalImageSizeDelta;
    public RectTransform transform;

    public ParallaxLayer(EditorParallaxLayer oldLayer)
    {
        layerImage = oldLayer.layerImage;
        distance = oldLayer.distance;
        pixelsPerUnit = oldLayer.pixelsPerUnit;
        gameUnitsConversionFactor = layerImage.texture.width / pixelsPerUnit;
    }
}

[RequireComponent(typeof(Canvas))]
public class ParallaxingBackgrounds : MonoBehaviour
{
    public Canvas canvas;
    private RectTransform canvasTransform;

    public EditorParallaxLayer[] parallaxLayers;
    
    public Vector2 worldPos;

    public Player player;

    [SerializeField] public List<ParallaxLayer> layers;

    [SerializeField, ReadOnly] private float canvasHeight;
    [SerializeField, ReadOnly] private float canvasWidth;

    

    // private bool initalized = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasTransform = canvas.GetComponent<RectTransform>();
        canvasHeight = canvasTransform.rect.height;
        canvasWidth = canvasTransform.rect.width;
        
        
        foreach (var pl in parallaxLayers)
        {
            var texture = pl.layerImage.mainTexture;
            float width = texture.width;
            float height = texture.height;
            
            

            float aspectRatio = width / height;
            
            Vector2 baseSizeDelta = new Vector2(aspectRatio * canvasHeight + 1, canvasHeight);

            ParallaxLayer newLayer = new ParallaxLayer(pl);

            // newLayer.aspectRatio = aspectRatio;
            newLayer.transform = pl.layerImage.GetComponent<RectTransform>();
            newLayer.originalImageSizeDelta = baseSizeDelta;

            
            Rect imageUVRect = pl.layerImage.uvRect;
            float uvWidth = canvasWidth / baseSizeDelta.x;
            imageUVRect.width = uvWidth;
            pl.layerImage.uvRect = imageUVRect;

            newLayer.transform.sizeDelta = new Vector2(canvasWidth, canvasHeight);
            
            layers.Add(newLayer);
            
        }

        // initalized = true;

    }

    // Update is called once per frame
    void Update()
    {
        //update width and height variables
        canvasHeight = canvasTransform.rect.height;
        canvasWidth = canvasTransform.rect.width;

        worldPos = player.transform.position;

        //iterate over managed layers and scale them for the UI
        foreach (ParallaxLayer parallaxLayer in layers)
        {
            //correct the scale of the layers for the scale of the UI
            HandleLayerScaling(parallaxLayer);
            
            //Actually execute the parallax effect
            ScrollLayer(parallaxLayer);
        }
    }

    private void ScrollLayer(ParallaxLayer parallaxLayer)
    {
        Rect uvRect = parallaxLayer.layerImage.uvRect;
        
        
        //apply a conversion factor from game units -> texture units
        float correctedWorldPos = worldPos.x / parallaxLayer.gameUnitsConversionFactor;
        
        //apply the parallax effect
        uvRect.x = correctedWorldPos / parallaxLayer.distance;

        parallaxLayer.layerImage.uvRect = uvRect;

    }

    public void HandleLayerScaling(ParallaxLayer parallaxLayer)
    {
        //see if the the difference between the layer's old width and height
        //and the canvas's new width and height is more than a little
        //and then transform the image to fit the new canvas size
        Vector2 layerDelta = parallaxLayer.transform.sizeDelta;
        if (
            Math.Abs(layerDelta.x - canvasWidth) <= Constants.RenderEpsilon ||
            Math.Abs(layerDelta.y - canvasHeight) <= Constants.RenderEpsilon)
        {
            return;
        }

        // float deltaCanvasWidth = canvasWidth - oldCanvasWidth;
        
        Rect imageUVRect = parallaxLayer.layerImage.uvRect;
        float uvWidth = canvasWidth / parallaxLayer.originalImageSizeDelta.x;
        imageUVRect.width = uvWidth;
        
        parallaxLayer.layerImage.uvRect = imageUVRect;

        parallaxLayer.transform.sizeDelta = new Vector2(canvasWidth, canvasHeight);
    }
    
}
