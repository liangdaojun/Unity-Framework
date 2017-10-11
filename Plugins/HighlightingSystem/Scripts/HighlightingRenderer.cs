using UnityEngine;
using System.Collections;

namespace HighlightingSystem
{
	public class HighlightingRenderer : HighlightingBase
	{
		// 
		[ImageEffectOpaque]
		void OnRenderImage(RenderTexture src, RenderTexture dst)
		{
			// Render highlightingBuffer using depthBuffer from src RenderTexture
			RenderHighlighting(src);

			// Don't blit the result to destination right now - just copy src to dst (highlightingBuffer blit will be performed later by the HighlightingBlitter component)
			Graphics.Blit(src, dst, blitMaterial);
		}
	}
}