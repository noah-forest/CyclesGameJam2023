using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Media;
using UnityEngine;

public class WipingController : MonoBehaviour
{
    public Texture2D maskTextureBase; // Assign the mask texture in the Inspector
    private Texture2D maskTexture; // Assign the mask texture in the Inspector
    public Texture2D brush; // Assign the mask texture in the Inspector

    private MeshRenderer meshRenderer;
    private Material maskMaterial;
    private bool isWiping;

    private Vector3 lastMouseMove = Vector3.zero;

    private float dirtAmount = 0;
    private float originalDirtAmount = 0;

    public int brushSize = 50;

    public TextMeshPro text;
    public List<int> pixelsToCheck = new List<int>();
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        maskMaterial = meshRenderer.material;
        
        Texture2D maskClone = new Texture2D(maskTextureBase.width, maskTextureBase.height);
        Color[] pixels = maskTextureBase.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            // Check if the pixel is not transparent (alpha value is not 0).
            if (pixels[i].a != 0)
            {
                // Set the pixel to white.
                pixels[i] = Color.white;
                pixelsToCheck.Add(i);
            }
        }
        maskClone.SetPixels(pixels);
        maskClone.Apply();

        maskTexture = maskClone;
        
        maskMaterial.SetTexture("_MaskTex", maskTexture);
        
        Texture2D scaledTexture = Bilinear(brush, brushSize, brushSize);

        brush = scaledTexture;
        
        originalDirtAmount = ReadDirtCount();
        StartCoroutine(WaitForFinished());
    }
    public static Texture2D Bilinear(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);

        float incX = 1.0f / (float)targetWidth;
        float incY = 1.0f / (float)targetHeight;

        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                float u = x * incX;
                float v = y * incY;
                result.SetPixel(x, y, source.GetPixelBilinear(u, v));
            }
        }

        result.Apply();
        return result;
    }

    private void OnMouseDrag()
    {
        if ((lastMouseMove - Input.mousePosition).magnitude > 2f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector2 wipePosition = GetWipePosition(hit.textureCoord);
                UpdateMaskTexture(wipePosition);
            }

            lastMouseMove = Input.mousePosition;
        }
    }
    
    private Vector2 GetWipePosition(Vector3 target)
    {
        Vector2 wipePosition = new Vector2(
            target.x * maskTexture.width,
            target.y * maskTexture.height
        );
        
        return wipePosition;
    }

    public void UpdateMaskTexture(Vector2 wipePosition)
    {
        if (!isWiping)
        {
            isWiping = true;
            
            StartCoroutine(UpdateMaskCoroutine(wipePosition));
        }
    }

    private float ReadDirtCount()
    {
        dirtAmount = 0;
        Color[] pixels = maskTexture.GetPixels();
        
        for (int i = 0; i < pixelsToCheck.Count; i++)
        {
            var g = pixels[pixelsToCheck[i]].g;
            if (g < 0.2f)
            {
                pixelsToCheck.RemoveAt(i);
                i--;
            }
            dirtAmount += g;
        }

        return dirtAmount;
    }

    private IEnumerator WaitForFinished()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            ReadDirtCount();
            var percent = (dirtAmount / originalDirtAmount) * 100;
            if (percent < 5)
            {
                GameManager.singleton.FinishMiniGame();
            }
        }
    }
    
    private System.Collections.IEnumerator UpdateMaskCoroutine(Vector2 wipePosition)
    {
        Vector2 brushSize = new Vector2(brush.width, brush.height);
        
        // Round down the wipePosition x and y coordinates
        int startX = Mathf.FloorToInt(wipePosition.x - brushSize.x/2);
        int startY = Mathf.FloorToInt(wipePosition.y - brushSize.y/2);


        // Update the pixels within the rectangular region to black
        for (int x = 0; x < brushSize.x; x++)
        {
            for (int y = 0; y < brushSize.y; y++)
            {
                Vector2Int wipeCenter = new Vector2Int(startX + x, startY + y);
                if (wipeCenter.x >= 0 && wipeCenter.x <= maskTexture.width && wipeCenter.y >= 0 &&
                    wipeCenter.y <= maskTexture.height)
                {
                    Color currentPixel = maskTexture.GetPixel(startX + x, startY + y);
                    Color brushPixel = brush.GetPixel(x, y);
                    maskTexture.SetPixel(wipeCenter.x, wipeCenter.y, new Color(0, currentPixel.g * brushPixel.g, 0));
                }
            }
        }
        
        maskTexture.Apply();

        yield return new WaitForEndOfFrame();
        isWiping = false;
    }
}
