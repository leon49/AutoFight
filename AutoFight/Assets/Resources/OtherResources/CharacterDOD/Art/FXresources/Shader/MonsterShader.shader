Shader "Unlit/MonsterShader" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		[HDR]_FresnelColor("Fresnel Color", Color) = (0.1331024, 0.4935883, 1.098095, 1)
		_Fresnel("Fresnel", Range(0, 1)) = 0
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass 
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				fixed4 _MainTex_ST;

				v2f vert(appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.texcoord);
					UNITY_OPAQUE_ALPHA(col.a);
					return col;
				}
				ENDCG
			}
			Pass
			{
				Blend SrcAlpha One//MinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float3 normal : TEXCOORD0;
					float3 viewDir : TEXCOORD1;
				};

					
				fixed4 _FresnelColor;
				float _Fresnel;

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.normal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
					o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float fresnel = pow(1 - saturate(dot(i.normal, i.viewDir)), 0.5);
					fixed4 final = _FresnelColor * fresnel * _Fresnel;
					return final;
				}
				ENDCG
			}
		}

}
