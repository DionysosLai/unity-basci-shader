using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectsBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Editor causes this Start");
    }

    private void Awake()
    {
        Debug.Log("Editor causes this Awake");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Editor causes this Update");
    }

    protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if(null == shader)
        {
            return null;
        }

        if(shader.isSupported && material && material.shader == shader)
        {
            return material;
        }

        if (!shader.isSupported)
        {
            return null;
        }
        else
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if (material)
                return material;
            else
                return null;
        }
    }
}
