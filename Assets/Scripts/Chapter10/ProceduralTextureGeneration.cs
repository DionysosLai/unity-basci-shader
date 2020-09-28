using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProceduralTextureGeneration : MonoBehaviour
{
    public Material material = null;

    #region Material properties
    [SerializeField, SetProperty("textureWidth")]
    private int m_texturewidth = 512;
    public int textureWidth
    {
        get
        {
            return m_texturewidth;
        }
        set
        {
            m_texturewidth = value;
            _UpdateMatetial();
        }
    }

    [SerializeField, SetProperty("backgroundColor")]
    private Color m_backgroundColor = Color.white;
    public Color backgroundColor
    {
        get
        {
            return m_backgroundColor;
        }
        set
        {
            m_backgroundColor = value;
            _UpdateMatetial();
        }
    }

    [SerializeField, SetProperty("circleColor")]
    private Color m_circleColor = Color.yellow;
    public Color circleColor
    {
        get
        {
            return m_circleColor;
        }
        set
        {
            m_circleColor = value;
            _UpdateMatetial();
        }
    }

    [SerializeField, SetProperty("blurFactor")]
    private float m_blurFactor = 2.0f;
    public float blurFactor
    {
        get
        {
            return m_blurFactor;
        }
        set
        {
            m_blurFactor = value;
            _UpdateMatetial();
        }
    }
    #endregion
    // Start is called before the first frame update

    private Texture2D m_genetateTexture = null;

    void Start()
    {
        if(null == material)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            if(null == renderer)
            {
                Debug.LogError("can not find render");
                return;
            }
            material = renderer.sharedMaterial;
        }
        _UpdateMatetial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _UpdateMatetial()
    {
        if(null != material)
        {
            m_genetateTexture = _GenerationProTexture();
            material.SetTexture("_MainTex", m_genetateTexture);
        }
    }

    private Color _MixColor(Color color0, Color color1, float mixFactor)
    {
        Color mixColor = Color.white;
        mixColor.r = Mathf.Lerp(color0.r, color1.r, mixFactor);
        mixColor.g = Mathf.Lerp(color0.g, color1.g, mixFactor);
        mixColor.b = Mathf.Lerp(color0.b, color1.b, mixFactor);
        mixColor.a = Mathf.Lerp(color0.a, color1.a, mixFactor);
        return mixColor;
    }

    private Texture2D _GenerationProTexture()
    {
        Texture2D proTexture = new Texture2D(textureWidth, textureWidth);

        float circleInterval = textureWidth / 4.0f;
		float radius = textureWidth / 10.0f;
        float edgeBlur = 1.0f / blurFactor;

        for (int w = 0; w < textureWidth; w++)
        {
            for(int h = 0; h < textureWidth; h++)
            {
                Color pixel = backgroundColor;
                for(int i = 0; i < 3; i++)
                {
                    for(int j = 0; j < 3; j++)
                    {
                        Vector2 circleCenter = new Vector2(circleInterval * (i + 1), circleInterval * (j + 1));
                        float dist = Vector2.Distance(new Vector2(w, h), circleCenter) - radius;
                        Color color = _MixColor(circleColor, new Color(pixel.r, pixel.g, pixel.b, 0.0f),
                            Mathf.SmoothStep(0f, 1.0f, dist * edgeBlur));
                        pixel = _MixColor(pixel, color, color.a);
                    }
                }
                proTexture.SetPixel(w, h, pixel);
            }
        }
        proTexture.Apply();
        return proTexture;
    }


}
