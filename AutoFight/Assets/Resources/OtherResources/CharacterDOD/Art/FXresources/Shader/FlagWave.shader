Shader "Custom/FlagWave"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_Frequency("Frequency", Range(0, 4)) = 2
		_WaveLength("Wave Length", Range(0, 20)) = 10
		_Strength("Strength", Range(0, 1)) = 0.2
	}

		SubShader
		{
			Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent"}
			LOD 200
			ZWrite Off
			//ZTest Always
			Cull Off
			Blend SrcAlpha One

			Pass
			{
				CGPROGRAM
				#include "UnityCG.cginc"
				#pragma vertex vert
				#pragma fragment frag

				fixed4 _Color;
				sampler2D _MainTex;
				fixed4 _MainTex_ST;
				half _Frequency;
				half _WaveLength;
				half _Strength;

				struct appdata
				{
					fixed4 vertex : POSITION;
					fixed4 color : COLOR;
					fixed2 uv : TEXCOORD0;
				};

				struct v2f
				{
					fixed4 pos : SV_POSITION;
					fixed4 color : COLOR;
					fixed2 uv : TEXCOORD0;
				};

				v2f vert(appdata v)
				{
					v2f o;
					v.vertex.xyz += sin(_Time.g * _Frequency + (v.vertex.x + v.vertex.y * 0.6) * _WaveLength) * _Strength * fixed3(0, 1, 0) * v.uv.r;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 main = tex2D(_MainTex, i.uv) * i.color * _Color;
					return main;
				}

				ENDCG
			}
		}
}
