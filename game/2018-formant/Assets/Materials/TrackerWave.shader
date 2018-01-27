Shader "Unlit/TrackerWave"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Amplitudes ("Amplitudes", Vector) = (0.5, 0.0, 0.0, 0.0)
		_Positions ("Positions", Vector) = (0.2, 0.0, 0.0, 0.0)
		_Widths ("Widths", Vector) = (0.7, 0.0, 0.0, 0.0)
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_NoiseAmplitude ("NoiseAmplitude", Float) = 0.25
		_NoisePosition ("NoisePosition", Float) = 0.0
		_Radius ("Radius", Float) = 2.0
		_Thickness ("Thickness", Float) = 0.25
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" }
		LOD 100

		Pass
		{
			Blend One One
			ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Amplitudes;
			float4 _Positions;
			float4 _Widths;
			float4 _Color;
			float _NoiseAmplitude;
			float _NoisePosition;
			float _Radius;
			float _Thickness;

			v2f vert (appdata v)
			{
				v2f o;
				float2 arc = v.vertex.xy;
				float noiseSample = tex2Dlod(_MainTex, float4(v.uv.x + _NoisePosition, 0.0, 0.0, 0.0)).r;
				float scale = _Radius + dot(_Amplitudes, cos(min(abs(v.uv.x - _Positions) / _Widths, 3.142)) * 0.5 + 0.5) + noiseSample * _NoiseAmplitude + (_Thickness * (v.uv.y - 0.5));
				o.vertex = UnityObjectToClipPos(float3(arc * scale, 0.0));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return fixed4(_Color.xyz * pow(abs(sin(i.uv.x * 3.141)), 4.0) * pow(abs(sin(i.uv.y * 3.141)), 16.0), 1.0);
			}
			ENDCG
		}
	}
}
