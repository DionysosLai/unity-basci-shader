using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPick : MonoBehaviour
{
    public BoxCollider pickCollider;

    private bool m_grab;
    private Camera m_camera;
    private Texture2D m_screenRenderTexture;
    private static Texture2D m_staticRectTextue;
    private static GUIStyle m_staticRectStyle;

    private static Vector3 m_pixelPosition = Vector3.zero;
    private Color m_pickerColor = Color.white;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
        if(null == m_camera)
        {
            Debug.Log("there is no camera");
            return;
        }

        if(null == pickCollider)
        {
            pickCollider = gameObject.AddComponent<BoxCollider>();
            pickCollider.center = Vector3.zero;
            pickCollider.center += m_camera.transform.worldToLocalMatrix.MultiplyVector(m_camera.transform.forward)
                * (m_camera.nearClipPlane + 0.2f);
            pickCollider.size = new Vector3(Screen.width, Screen.height, 0.1f);

        }
    }

    public static void GUIDrawRect(Rect position, Color color)
    {
        if(m_staticRectTextue == null)
        {
            m_staticRectTextue = new Texture2D(1, 1);
        }

        if(m_staticRectStyle == null)
        {
            m_staticRectStyle = new GUIStyle();
        }

        m_staticRectTextue.SetPixel(0, 0, color);
        m_staticRectTextue.Apply();

        m_staticRectStyle.normal.background = m_staticRectTextue;

        GUI.Box(position, GUIContent.none, m_staticRectStyle);
    }

    private void OnPostRender()
    {
        if (m_grab)
        {
            m_screenRenderTexture = new Texture2D(Screen.width, Screen.height);
            m_screenRenderTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            m_screenRenderTexture.Apply();

            m_pickerColor = m_screenRenderTexture.GetPixel(Mathf.FloorToInt(m_pixelPosition.x),
                Mathf.FloorToInt(m_pixelPosition.y));
            m_grab = false;

        }
    }

    private void OnMouseDown()
    {
        m_grab = false;
        m_pixelPosition = Input.mousePosition;
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 120, 200), "Color Picker");
        GUIDrawRect(new Rect(20, 30, 80, 80), m_pickerColor);
        GUI.Label(new Rect(10, 120, 100, 20), "R: " + System.Math.Round((double)m_pickerColor.r, 4) +
            "\t(" + Mathf.FloorToInt(m_pickerColor.r * 255) + ")");
        GUI.Label(new Rect(10, 140, 100, 20), "G: " + System.Math.Round((double)m_pickerColor.r, 4) +
    "\t(" + Mathf.FloorToInt(m_pickerColor.r * 255) + ")");
        GUI.Label(new Rect(10, 160, 100, 20), "B: " + System.Math.Round((double)m_pickerColor.r, 4) +
    "\t(" + Mathf.FloorToInt(m_pickerColor.r * 255) + ")");
        GUI.Label(new Rect(10, 180, 100, 20), "A: " + System.Math.Round((double)m_pickerColor.r, 4) +
    "\t(" + Mathf.FloorToInt(m_pickerColor.r * 255) + ")");
    }


}
