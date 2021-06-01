using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecodeMock : MonoBehaviour
{
    [SerializeField] private Text vitcText;

    [SerializeField] private RenderTexture inputTexture;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputTexture != null)
        {
            // Get grayscale colors at first line
            var colors = GetPixels(inputTexture);
            Array.Resize<Color>(ref colors, inputTexture.width);
            var grayscale = GetGrayScaleColor(colors);

            // Read VITC
            var result = ReadVitcLine(grayscale, 8);
            vitcText.text = result.ToString(@"mm\:ss\.ff");
        }
    }

    public TimeSpan ReadVitcLine(float[] vitcLine, int lineSize, float thresholdBlack = 0.1f,
        float thresholdGray = 0.3f, float thresholdWhite = 0.5f)
    {
        // Decode here
        // THIS IS COLOR READ TEST
        Debug.Log($"Last line: {vitcLine[vitcLine.Length - (lineSize/2)]:F1}");
        
        
        // Temporary result by Time.time
        //https://docs.unity3d.com/ja/2019.4/ScriptReference/Time-time.html
        var temporary = Time.time;
        return TimeSpan.FromSeconds(temporary);
    }

    private float[] GetGrayScaleColor(Color[] color)
    {
        var grayscale = new float[color.Length];
        for (int i = 0; i < color.Length; i++)
        {
            grayscale[i] = color[i].grayscale;
        }

        return grayscale;
    }

    /// <summary>
    /// Get color pixels from render texture
    /// </summary>
    /// <param name="rt"></param>
    /// <returns></returns>
    private Color[] GetPixels(RenderTexture rt)
    {
        // Cache active render texture
        var currentRT = RenderTexture.active;

        // Set active render texture to target
        RenderTexture.active = rt;
        
        // Set pixels to new texture by Texture2D.ReadPixels on active render texture
        var texture = new Texture2D(rt.width, rt.height);
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();

        // Get colors from texture
        var colors = texture.GetPixels();
            
        // Restore active render texture
        RenderTexture.active = currentRT;

        return colors;
    }
}
