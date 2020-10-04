using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlur : PostEffectsBase
{
	public Shader motionBlurShader;
	private Material motionBlurMaterial = null;

	public Material material
	{
		get
		{
			motionBlurMaterial = CheckShaderAndCreateMaterial(motionBlurShader, motionBlurMaterial);
			return motionBlurMaterial;
		}
	}

	[Range(0.0f, 0.9f)]
	public float blurAmount = 0.5f;

	private RenderTexture accumulationTexture;

    private void OnDisable()
    {
		DestroyImmediate(accumulationTexture);
    }

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (material != null)
		{
			// Create the accumulation texture
			if (accumulationTexture == null || accumulationTexture.width != source.width || accumulationTexture.height != source.height)
			{
				DestroyImmediate(accumulationTexture);
				accumulationTexture = new RenderTexture(source.width, source.height, 0);
				accumulationTexture.hideFlags = HideFlags.HideAndDontSave;
				Graphics.Blit(source, accumulationTexture);
			}

			// We are accumulating motion over frames without clear/discard
			// by design, so silence any performance warnings from Unity
			accumulationTexture.MarkRestoreExpected();

			material.SetFloat("_BlurAmount", 1.0f - blurAmount);

			Graphics.Blit(source, accumulationTexture, material);
			Graphics.Blit(accumulationTexture, destination);
		}
        else
        {
			Graphics.Blit(source, destination);
        }
    }

	// Blur iterations - larger number means more blur.

	/// 3rd edition: use iterations for larger blur

}
