Shader "Custom/AsteroidButton"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SelectedColor("Selected_Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_UnselectedColor("Unselected_Color", Color) = (1.0, 1.0, 1.0, 0.0)
		_Selected("Selected", Float) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _SelectedColor;
			float4 _UnselectedColor;
			float _Selected;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				clip(col.w - 0.7);

				if (_Selected == 1.0)
				{
					col.xyz = col.xyz * _SelectedColor.xyz;
				}

				return col;
			}
			ENDCG
		}
	}
}
