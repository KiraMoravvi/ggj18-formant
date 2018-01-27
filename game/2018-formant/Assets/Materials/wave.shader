Shader "Unlit/wave"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Position ("Position", Float) = 0.0
		_Radius ("Radius", Float) = 1.0
		_Thickness ("Thickness", Float) = 0.25
		_Amplitude ("Amplitude", Float) = 0.5
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }
		LOD 100

		Pass
		{
			Blend One One
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
			float _Radius;
			float _Thickness;
			float _Amplitude;
			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				float2 arc = v.vertex.xy;
				float waveSample = tex2Dlod(_MainTex, float4(v.uv.x + _Position, 0.0, 0.0, 0.0)).x;
				float scale = _Radius + (_Thickness * (v.uv.y - 0.5)) + waveSample * _Amplitude;
				o.vertex = UnityObjectToClipPos(float3(arc * scale, 0.0));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return fixed4(_Color.xyz * pow(sin(i.uv.x * 3.141), 4.0) * pow(sin(i.uv.y * 3.141), 16.0), 1.0);
			}
			ENDCG
		}
	}
}
