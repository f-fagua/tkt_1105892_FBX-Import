Shader "GLU/ZBS/BaseBuilding"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LighmapTex("LighmapTex", 2D) = "white" {}
		_LightmapMultiply("LightmapMultiply", Range(0, 3)) = 2
		_TintColor("TintColor", Color) = (0.5, 0.5, 0.5, 1)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile _ CUSTOM_SKINNING
			
			#include "UnityCG.cginc"
			//#include "FCSUber/CustomSkinning.cginc"

			struct v_in
			{
				float4 vertex : POSITION;
				#ifdef CUSTOM_SKINNING
				half3 texcoord0 : TEXCOORD0;
				#else
				half2 texcoord0 : TEXCOORD0;
				#endif
				half2 texcoord1 : TEXCOORD1;
			};

			struct v_out
			{
				float4 pos : SV_POSITION;
				half2 uv0 : TEXCOORD0;
				half2 uv1 : TEXCOORD1;
				UNITY_FOG_COORDS(2)
			};

			sampler2D _MainTex;	float4 _MainTex_ST;
			sampler2D _LighmapTex;	float4 _LighmapTex_ST;
			fixed4 _TintColor;
			fixed _LightmapMultiply;

			v_out vert (v_in v)
			{
				v_out o;
				o.pos = v.vertex;
				o.uv0 = TRANSFORM_TEX(v.texcoord0.xy, _MainTex);
				o.uv1 = TRANSFORM_TEX(v.texcoord1, _LighmapTex);
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			
			fixed4 frag (v_out i) : SV_Target
			{
				fixed4 main_color = tex2D(_MainTex, i.uv0);
				fixed4 lightmap_color = tex2D(_LighmapTex, i.uv1);
				fixed4 base_color = (main_color * lightmap_color * _TintColor * _LightmapMultiply);

				UNITY_APPLY_FOG(i.fogCoord, base_color);
				return base_color;
			}
			ENDCG
		}
	}
}
