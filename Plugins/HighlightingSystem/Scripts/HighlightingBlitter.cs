using UnityEngine;
using System.Collections;

namespace HighlightingSystem
{
	public class HighlightingBlitter : MonoBehaviour
	{
		public HighlightingRenderer highlightingRenderer;

		// 
		void LateUpdate()
		{
			enabled = (highlightingRenderer != null && highlightingRenderer.enabled);
		}

		// 
		void OnRenderImage(RenderTexture src, RenderTexture dst)
		{
			if (highlightingRenderer == null || !highlightingRenderer.enabled)
			{
				Graphics.Blit(src, dst);
				Debug.LogWarning("HighlightingSystem : HighlightingRenderer component is not assigned or not enabled. Disabling HighlightingBlitter.");
				enabled = false;
				return;
			}

			highlightingRenderer.BlitHighlighting(src, dst);
		}
	}
}