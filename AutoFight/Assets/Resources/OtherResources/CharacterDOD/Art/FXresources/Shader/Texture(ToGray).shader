Shader "Unlit/TextureToGray" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_EmissionTex("Emission Texture", 2D) = "black" {}
		_EmissionIntensity("Emission Intensity", Range(0, 3)) = 1
		[Toggle]_EmissionBlink("Emission Blink", float) = 1
		_BlinkFrequency("Blink Frequency", float) = 1
		_PetrifyingIntensity("Petrifying Intensity", Range(0,1)) = 0
		_Whosyourdaddy("Whosyourdaddy", Range(0, 1)) = 0
		_SpiritsColor("Spirits Color", Color) = (1, 1, 0, 1)
		_Spirits("Spirits", Range(0, 1)) = 0
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
					float3 normal : NORMAL;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					float2 texcoord : TEXCOORD0;
					float3 normal : TEXCOORD1;
					float3 viewDir : TEXCOORD2;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				sampler2D _EmissionTex;
				fixed4 _MainTex_ST;
				fixed4 _FresnelColor;
				half _PetrifyingIntensity;
				half _Whosyourdaddy;
				half _EmissionIntensity;
				half _EmissionBlink;
				half _BlinkFrequency;

				v2f vert(appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.normal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
					o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.texcoord);
					//石化
					float nocolor = dot(col.rgb, float3(0.3, 0.59, 0.11));
					fixed4 finalcol = nocolor * _PetrifyingIntensity + col * (1 - _PetrifyingIntensity);

					//无敌
					float fresnel = 1 - saturate(dot(i.normal, i.viewDir));
					finalcol += _Whosyourdaddy * fixed4(1, 1, 0, 1) * pow(fresnel, 1);

					//发光
					fixed4 emission = tex2D(_EmissionTex, i.texcoord) * _EmissionIntensity;
					if (_EmissionBlink)
						emission *= sin(_Time.g * _BlinkFrequency) * 0.5 + 0.5;
					finalcol += emission;

					UNITY_OPAQUE_ALPHA(col.a);
					return finalcol;
				}
				ENDCG
			}
			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha

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

					
				fixed4 _SpiritsColor;
				float _Spirits, _Whosyourdaddy;

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
					//鬼神化
					float fresnel = 1 - saturate(dot(i.normal, i.viewDir));
					fixed4 final = _SpiritsColor * _Spirits * pow(fresnel, 1) * (1 - _Whosyourdaddy);
					return final;
				}
				ENDCG
			}
		}

}
