Shader "Daikon Forge/Default UI Shader"
{
	Properties
	{
		_MainTex ( "Base (RGB), Alpha (A)", 2D ) = "white" {}
	}

	SubShader
	{
		LOD 200

		Tags
		{
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"Queue" = "Overlay"
			"PreviewType"="Plane"
		}
		
		Pass
		{

			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGBA
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex		: POSITION;
				half4 color			: COLOR;
				float2 tex			: TEXCOORD0;
			}; 

			struct v2f
			{
				float4 vertex		: POSITION;
				half4 color			: COLOR;
				float2 tex			: TEXCOORD0;
			};

			sampler2D _MainTex; 

			v2f vert (appdata_t v)
			{

				v2f o;
				o.vertex = mul( UNITY_MATRIX_MVP, v.vertex );
				o.color = v.color;
				o.tex.xy  = v.tex.xy;  

				return o;

			}

			half4 frag (v2f IN) : COLOR
			{
				return tex2D( _MainTex, IN.tex.xy ) * IN.color;
			}

			ENDCG
		}
	}
	
	SubShader
	{
		Tags
		{
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"Queue" = "Overlay"
		}
		
		LOD 100
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		ColorMask RGBA
		AlphaTest Greater .01
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}