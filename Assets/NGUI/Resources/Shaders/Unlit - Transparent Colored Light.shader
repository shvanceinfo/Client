Shader "Unlit/Transparent Colored Light"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
	
	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Offset -1, -1
		ColorMask RGB
		AlphaTest Greater .01
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"
	
				sampler2D _MainTex;
				float4 _MainTex_ST;
				float2 _ClipSharpness = float2(20.0, 20.0);

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};
	
				struct v2f
				{
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.color = v.color;
					return o;

				}
				
				fixed4 frag (v2f IN) : COLOR
				{
					
					fixed4 col;
					if (IN.color.r==0&&
					IN.color.g==0&&
					IN.color.b==0
					)  
			    	{  
			        	col = tex2D(_MainTex, IN.texcoord);  
			        	//float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));  
			        	col.rgb = float3(1, 1, 1);  
			    	}  
			    	else  
			    	{  
			        	col = tex2D(_MainTex, IN.texcoord) * IN.color;
			    	}
					return col;

				}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
