Shader "Hidden/Highlighted/Composite"
{
	Properties
	{
		[HideInInspector] _MainTex ("", 2D) = "" {}
	}
	
	SubShader
	{
		Pass
		{
			Lighting Off
			Fog { Mode off }
			ZWrite Off
			ZTest Always
			Cull Off
			Blend Off
			
			// Color = Src.Color * Src.Alpha + Dst.Color * (1 - Src.Alpha),	// Lerp Dst to Src by Src.Alpha factor
			// Alpha = Src.Alpha * 0 + Dst.Alpha * 1						// Keep Dst.Alpha value intact
			Blend SrcAlpha OneMinusSrcAlpha, Zero One
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"
			
			struct v2f
			{
				float4 pos : POSITION;
				half2 uv : TEXCOORD0;
			};
			
			uniform sampler2D _MainTex;
			
			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}
			
			fixed4 frag(v2f i) : COLOR
			{
				return tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
	}
	FallBack Off
}