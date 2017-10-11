using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HighlightingSystem
{
	public partial class Highlighter : MonoBehaviour
	{
		// Constants (don't touch this!)
		#region Constants
		// 2 * PI constant required for flashing
		private const float doublePI = 2f * Mathf.PI;
		
		// Occlusion color
		private readonly Color occluderColor = new Color(0f, 0f, 0f, 0f);
		
		// RenderQueue Geometry
		private const int renderQueueGeometry = 2000;
		
		// RenderQueue Geometry + 1
		private const int renderQueueOverlay = renderQueueGeometry + 1;
		
		// ZTest LEqual
		private const float zTestLessEqual = (float)UnityEngine.Rendering.CompareFunction.LessEqual;
		
		// ZTest Always
		private const float zTestAlways = (float)UnityEngine.Rendering.CompareFunction.Always;
		#endregion

		#region Static Fields
		// Global highlighting shaders ZWrite property
		static private float zWrite = -1f;				// Set to unusual default value to force initialization on start

		// Global highlighting shaders Offset Factor property
		static private float offsetFactor = float.NaN;	// Set to unusual default value to force initialization on start

		// Global highlighting shaders Offset Units property
		static private float offsetUnits = float.NaN;	// Set to unusual default value to force initialization on start
		#endregion

		#region Private Fields
		// Cached transform component reference
		private Transform tr;

		// Cached Renderers
		private List<HighlightingRendererCache> highlightableRenderers;

		// Materials reinitialization is required flag
		private bool materialsIsDirty = true;
		
		// Current state of highlighting
		private bool currentState = false;
		
		// Current highlighting color
		private Color currentColor;
		
		// Transition is active flag
		private bool transitionActive = false;
		
		// Current transition value
		private float transitionValue = 0f;
		
		// Flashing frequency (times per second)
		private float flashingFreq = 2f;
		
		// One-frame highlighting flag
		private int _once = 0;
		private bool once
		{
			get { return _once == Time.frameCount; }
			set { _once = value ? Time.frameCount : 0; }
		}
		
		// One-frame highlighting color
		private Color onceColor = Color.red;
		
		// Flashing state flag
		private bool flashing = false;
		
		// Flashing from color
		private Color flashingColorMin = new Color(0f, 1f, 1f, 0f);
		
		// Flashing to color
		private Color flashingColorMax = new Color(0f, 1f, 1f, 1f);
		
		// Constant highlighting state flag
		private bool constantly = false;
		
		// Constant highlighting color
		private Color constantColor = Color.yellow;
		
		// Occluder mode is enabled flag
		private bool occluder = false;
		
		// See-through mode flag (should have same initial value with zTest and renderQueue variables!)
		private bool seeThrough = true;
		
		// RenderQueue (true = Geometry+1, false = Geometry)
		private bool renderQueue = true;
		
		// Current ZTest value (true = Always, false = LEqual)
		private bool zTest = true;
		
		// Current Stencil Ref value (true = 1, false = 0)
		private bool stencilRef = true;
		
		// Returns real RenderQueue integer value which will be passed to the materials
		private int renderQueueInt { get { return renderQueue ? renderQueueOverlay : renderQueueGeometry; } }
		
		// Returns real ZTest float value which will be passed to the materials
		private float zTestFloat { get { return zTest ? zTestAlways : zTestLessEqual; } }
		
		// Returns real Stencil Ref float value which will be passed to the materials
		private float stencilRefFloat { get { return stencilRef ? 1f : 0f; } }

		// Opaque shader cached reference
		static private Shader _opaqueShader;
		static public Shader opaqueShader
		{
			get
			{
				if (_opaqueShader == null)
				{
					_opaqueShader = Shader.Find("Hidden/Highlighted/Opaque");
				}
				return _opaqueShader;
			}
		}
		
		// Transparent shader cached reference
		static private Shader _transparentShader;
		static public Shader transparentShader
		{
			get
			{
				if (_transparentShader == null)
				{
					_transparentShader = Shader.Find("Hidden/Highlighted/Transparent");
				}
				return _transparentShader;
			}
		}
		
		// Shared (for this component) replacement material for opaque geometry highlighting
		private Material _opaqueMaterial;
		private Material opaqueMaterial
		{
			get
			{
				if (_opaqueMaterial == null)
				{
					_opaqueMaterial = new Material(opaqueShader);
					_opaqueMaterial.hideFlags = HideFlags.HideAndDontSave;
					
					// Make sure that ShaderPropertyIDs is initialized
					ShaderPropertyID.Initialize();
					
					// Make sure that shader will have proper default values
					_opaqueMaterial.renderQueue = renderQueueInt;
					_opaqueMaterial.SetFloat(ShaderPropertyID._ZTest, zTestFloat);
					_opaqueMaterial.SetFloat(ShaderPropertyID._StencilRef, stencilRefFloat);
				}
				return _opaqueMaterial;
			}
		}
		#endregion

		// Internal class for renderers caching
		private class HighlightingRendererCache
		{
			public Renderer rendererCached;
			public GameObject goCached;
			private Material[] sourceMaterials;
			private Material[] replacementMaterials;
			private List<int> transparentMaterialIndexes;
			private int layer;
			
			// Constructor
			public HighlightingRendererCache(Renderer rend, Material[] mats, Material sharedOpaqueMaterial, int renderQueue, float zTest, float stencilRef)
			{
				rendererCached = rend;
				goCached = rend.gameObject;
				sourceMaterials = mats;
				replacementMaterials = new Material[mats.Length];
				transparentMaterialIndexes = new List<int>();
				
				for (int i = 0; i < mats.Length; i++)
				{
					Material sourceMat = mats[i];
					if (sourceMat == null) { continue; }
					
					string tag = sourceMat.GetTag("RenderType", true, "Opaque");
					if (tag == "Transparent" || tag == "TransparentCutout")
					{
						Material replacementMat = new Material(transparentShader);
						replacementMat.renderQueue = renderQueue;
						replacementMat.SetFloat(ShaderPropertyID._ZTest, zTest);
						replacementMat.SetFloat(ShaderPropertyID._StencilRef, stencilRef);
						if (sourceMat.HasProperty(ShaderPropertyID._MainTex))
						{
							replacementMat.SetTexture(ShaderPropertyID._MainTex, sourceMat.mainTexture);
							replacementMat.SetTextureOffset("_MainTex", sourceMat.mainTextureOffset);
							replacementMat.SetTextureScale("_MainTex", sourceMat.mainTextureScale);
						}
						
						int cutoff = ShaderPropertyID._Cutoff;
						replacementMat.SetFloat(cutoff, sourceMat.HasProperty(cutoff) ? sourceMat.GetFloat(cutoff) : transparentCutoff);
						
						replacementMaterials[i] = replacementMat;
						transparentMaterialIndexes.Add(i);
					}
					else
					{
						replacementMaterials[i] = sharedOpaqueMaterial;
					}
				}
			}
			
			// Enable / disable highlighting materials
			public void SetState(bool state)
			{
				if (state)
				{
					// Cache layer
					layer = goCached.layer;
					// Temporarily assign layer, renderable by the highlighting camera
					goCached.layer = highlightingLayer;

					rendererCached.sharedMaterials = replacementMaterials;
				}
				else
				{
					// Restore original layer
					goCached.layer = layer;

					rendererCached.sharedMaterials = sourceMaterials;
				}
			}
			
			// Sets given color as highlighting color on all transparent materials of this renderer
			public void SetColorForTransparent(Color clr)
			{
				for (int i = 0; i < transparentMaterialIndexes.Count; i++)
				{
					replacementMaterials[transparentMaterialIndexes[i]].SetColor(ShaderPropertyID._Outline, clr);
				}
			}
			
			// Sets RenderQueue parameter on all transparent materials of this renderer
			public void SetRenderQueueForTransparent(int renderQueue)
			{
				for (int i = 0; i < transparentMaterialIndexes.Count; i++)
				{
					replacementMaterials[transparentMaterialIndexes[i]].renderQueue = renderQueue;
				}
			}
			
			// Sets ZTest parameter on all transparent materials of this renderer
			public void SetZTestForTransparent(float zTest)
			{
				for (int i = 0; i < transparentMaterialIndexes.Count; i++)
				{
					replacementMaterials[transparentMaterialIndexes[i]].SetFloat(ShaderPropertyID._ZTest, zTest);
				}
			}
			
			// Sets Stencil Ref parameter on all transparent materials of this renderer
			public void SetStencilRefForTransparent(float stencilRef)
			{
				for (int i = 0; i < transparentMaterialIndexes.Count; i++)
				{
					replacementMaterials[transparentMaterialIndexes[i]].SetFloat(ShaderPropertyID._StencilRef, stencilRef);
				}
			}
		}

		#region Initialization
		// 
		private void Awake()
		{
			tr = GetComponent<Transform>();
			ShaderPropertyID.Initialize();
		}
		
		// 
		private void OnEnable()
		{
			if (!CheckInstance()) { return; }

			HighlightingBase.AddHighlighter(this);
		}
		
		// 
		private void OnDisable()
		{
			HighlightingBase.RemoveHighlighter(this);

			// Clear cached renderers
			if (highlightableRenderers != null) { highlightableRenderers.Clear(); }
			
			// Reset highlighting parameters to default values
			materialsIsDirty = true;
			currentState = false;
			currentColor = Color.clear;
			transitionActive = false;
			transitionValue = 0f;
			once = false;
			flashing = false;
			constantly = false;
			occluder = false;
			seeThrough = false;

			/* 
			// Reset custom parameters of the highlighting
			onceColor = Color.red;
			flashingColorMin = new Color(0f, 1f, 1f, 0f);
			flashingColorMax = new Color(0f, 1f, 1f, 1f);
			flashingFreq = 2f;
			constantColor = Color.yellow;
			*/
		}

		// 
		private void Update()
		{
			// Update transition value
			PerformTransition();
		}
		#endregion

		#region Private Methods
		// Allow only single instance of the Highlighter component on a GameObject
		private bool CheckInstance()
		{
			Highlighter[] highlighters = GetComponents<Highlighter>();
			if (highlighters.Length > 1 && highlighters[0] != this)
			{
				enabled = false;
				Debug.LogWarning("HighlightingSystem : Multiple Highlighter components on a single GameObject is not allowed! Highlighter has been disabled on a GameObject with name '" + gameObject.name + "'.");
				return false;
			}
			return true;
		}

		// This method defines the way in which renderers are initialized
		private void InitMaterials()
		{
			highlightableRenderers = new List<HighlightingRendererCache>();
			
			List<Renderer> renderers = new List<Renderer>();
			
			// Find all renderers which should be controlled by this Highlighter component
			GrabRenderers(tr, ref renderers);
			
			// Cache found renderers
			CacheRenderers(renderers);
			
			// Reset
			currentState = false;
			materialsIsDirty = false;
			currentColor = Color.clear;
		}
		
		// Follows hierarchy of objects from t, searches for Renderers and adds them to the list. Breaks if another Highlighter component found
		private void GrabRenderers(Transform t, ref List<Renderer> renderers)
		{
			GameObject g = t.gameObject;
			IEnumerator e;
			
			// Find all Renderers of all types on current GameObject g and add them to the renderers list
			for (int i = 0; i < types.Count; i++)
			{
				Component[] c = g.GetComponents(types[i]);
				
				e = c.GetEnumerator();
				while (e.MoveNext())
				{
					renderers.Add(e.Current as Renderer);
				}
			}
			
			// Return if transform t doesn't have any children
			if (t.childCount == 0) { return; }
			
			// Recursively cache renderers on all child GameObjects
			e = t.GetEnumerator();
			while (e.MoveNext())
			{
				Transform childTransform = e.Current as Transform;
				GameObject childGameObject = childTransform.gameObject;
				Highlighter h = childGameObject.GetComponent<Highlighter>();
				
				// Do not cache Renderers of this childTransform in case it has it's own Highlighter component
				if (h != null) { continue; }
				
				GrabRenderers(childTransform, ref renderers);
			}
		}
		
		// Caches given renderers
		private void CacheRenderers(List<Renderer> renderers)
		{
			int l = renderers.Count;
			
			for (int i = 0; i < l; i++)
			{
				Material[] materials = renderers[i].sharedMaterials;
				
				if (materials != null)
				{
					highlightableRenderers.Add(new HighlightingRendererCache(renderers[i], materials, opaqueMaterial, renderQueueInt, zTestFloat, stencilRefFloat));
				}
			}
		}

		// Turns on highlighting shaders on all highlightableRenderers
		public bool Highlight()
		{
			// Initialize new materials if needed
			if (materialsIsDirty) { InitMaterials(); }
			
			// Is any highlighting mode is enabled?
			currentState = (once || flashing || constantly || transitionActive);
			
			// In case framebuffer depth data is available or any highlighting mode is active
			if (HighlightingBase.current.isDepthAvailable || currentState)
			{
				// RenderQueue [seeThrough ? Geometry+1 : Geometry], ZTest [seeThrough ? Always : LEqual], Stencil Ref 1
				UpdateShaderParams(seeThrough, seeThrough, true);
			}
			// Otherwise, in case only occluder mode is enabled
			else if (occluder)
			{
				// RenderQueue Geometry, ZTest LEqual, Stencil Ref 0
				UpdateShaderParams(false, false, false);
				// Render occluder to the highlighting buffer
				currentState = true;
			}

			// In case once, flashing, constantly, transitionActive or occluder mode is active
			if (currentState)
			{
				bool isVisible = false;

				// Avoid null-reference exceptions when cached GameObject or Renderer was removed but ReinitMaterials wasn't called
				for (int i = highlightableRenderers.Count - 1; i >= 0; i--)
				{
					HighlightingRendererCache hrc = highlightableRenderers[i];
					if (hrc.goCached == null || hrc.rendererCached == null)
					{
						highlightableRenderers.RemoveAt(i);
					}
					// Check if at least one Renderer controlled by this Highlighter is currently visible
					else if (hrc.rendererCached.isVisible)
					{
						isVisible |= true;
					}
				}

				// Do nothing in case no Renderers is currently visible
				if (!isVisible)
				{
					currentState = false;
					// No visible renderers
					return false;
				}
				
				UpdateColors();
				if (highlightableRenderers != null)
				{
					for (int i = highlightableRenderers.Count - 1; i >= 0; i--)
					{
						highlightableRenderers[i].SetState(true);
					}
					// Highlihgted
					return true;
				}
				// No highlightableRenderers
				return false;
			}
			// No highlighting modes active
			return false;
		}
		
		// Turns off highlighting shaders on all highlightableRenderers
		public void Extinguish()
		{
			if (currentState && highlightableRenderers != null)
			{
				for (int i = 0; i < highlightableRenderers.Count; i++)
				{
					highlightableRenderers[i].SetState(false);
				}
			}
		}

		// Sets RenderQueue, ZTest and Stencil Ref parameters on all materials of all renderers of this object
		private void UpdateShaderParams(bool rq, bool zt, bool sr)
		{
			// RenderQueue
			if (renderQueue != rq)
			{
				renderQueue = rq;
				int rqi = renderQueueInt;
				opaqueMaterial.renderQueue = rqi;
				for (int i = 0; i < highlightableRenderers.Count; i++)
				{
					highlightableRenderers[i].SetRenderQueueForTransparent(rqi);
				}
			}
			
			// ZTest
			if (zTest != zt)
			{
				zTest = zt;
				float ztf = zTestFloat;
				opaqueMaterial.SetFloat(ShaderPropertyID._ZTest, ztf);
				for (int i = 0; i < highlightableRenderers.Count; i++) 
				{
					highlightableRenderers[i].SetZTestForTransparent(ztf);
				}
			}
			
			// Stencil Ref
			if (stencilRef != sr)
			{
				stencilRef = sr;
				float srf = stencilRefFloat;
				opaqueMaterial.SetFloat(ShaderPropertyID._StencilRef, srf);
				for (int i = 0; i < highlightableRenderers.Count; i++)
				{
					highlightableRenderers[i].SetStencilRefForTransparent(srf);
				}
			}
		}

		// Update highlighting color if necessary
		private void UpdateColors()
		{
			if (once)
			{
				SetColor(onceColor);
				return;
			}
			
			if (flashing)
			{
				// Flashing frequency is not affected by Time.timeScale
				Color c = Color.Lerp(flashingColorMin, flashingColorMax, 0.5f * Mathf.Sin(Time.realtimeSinceStartup * flashingFreq * doublePI) + 0.5f);
				SetColor(c);
				return;
			}
			
			if (transitionActive)
			{
				Color c = new Color(constantColor.r, constantColor.g, constantColor.b, constantColor.a * transitionValue);
				SetColor(c);
				return;
			}
			else if (constantly)
			{
				SetColor(constantColor);
				return;
			}
			
			if (occluder)
			{
				SetColor(occluderColor);
				return;
			}
		}

		// Set given highlighting color
		private void SetColor(Color value)
		{
			if (currentColor == value) { return; }
			currentColor = value;
			opaqueMaterial.SetColor(ShaderPropertyID._Outline, currentColor);
			for (int i = 0; i < highlightableRenderers.Count; i++)
			{
				highlightableRenderers[i].SetColorForTransparent(currentColor);
			}
		}
		
		// Calculate new transition value if necessary
		private void PerformTransition()
		{
			if (transitionActive == false) { return; }
			
			float targetValue = constantly ? 1f : 0f;
			
			// Is transition finished?
			if (transitionValue == targetValue)
			{
				transitionActive = false;
				return;
			}
			
			if (Time.timeScale != 0f)
			{
				// Calculating delta time untouched by Time.timeScale
				float unscaledDeltaTime = Time.deltaTime / Time.timeScale;
				
				// Calculating new transition value
				transitionValue = Mathf.Clamp01(transitionValue + (constantly ? constantOnSpeed : -constantOffSpeed) * unscaledDeltaTime);
			}
			else { return; }
		}
		#endregion

		#region Static Methods
		// Globally sets ZWrite shader parameter for all highlighting materials
		static public void SetZWrite(float value)
		{
			if (zWrite == value) { return; }
			zWrite = value;
			Shader.SetGlobalFloat(ShaderPropertyID._HighlightingZWrite, zWrite);
		}

		// Globally sets Offset Factor shader parameter for all highlighting materials
		static public void SetOffsetFactor(float value)
		{
			if (offsetFactor == value) { return; }
			offsetFactor = value;
			Shader.SetGlobalFloat(ShaderPropertyID._HighlightingOffsetFactor, offsetFactor);
		}

		// Globally sets Offset Units shader parameter for all highlighting materials
		static public void SetOffsetUnits(float value)
		{
			if (offsetUnits == value) { return; }
			offsetUnits = value;
			Shader.SetGlobalFloat(ShaderPropertyID._HighlightingOffsetUnits, offsetUnits);
		}
		#endregion
	}
}