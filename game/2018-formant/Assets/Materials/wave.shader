Shader "Unlit/wave"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Position ("Position", Float) = 0.0
		_NoisePosition("NoisePosition", Float) = 0.0
		_Radius ("Radius", Float) = 1.0
		_Thickness ("Thickness", Float) = 0.25
		_Amplitude ("Amplitude", Vector) = (1.0, 1.0, 1.0, 0.25)
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }
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
			float _Position;
			float _NoisePosition;
			float _Radius;
			float _Thickness;
			float4 _Amplitude;
			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				float2 arc = v.vertex.xy;
				float3 waveSample = tex2Dlod(_MainTex, float4(v.uv.x + _Position, 0.0, 0.0, 0.0)).rgb;
				float noiseSample = tex2Dlod(_MainTex, float4(v.uv.x + _NoisePosition, 0.0, 0.0, 0.0)).a;
				float scale = _Radius + (_Thickness * (v.uv.y - 0.5)) + dot(waveSample, _Amplitude.rgb) + noiseSample * _Amplitude.a;
				o.vertex = UnityObjectToClipPos(float3(arc * scale, 0.0));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
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
