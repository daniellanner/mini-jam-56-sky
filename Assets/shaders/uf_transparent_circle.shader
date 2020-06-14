Shader "indiversity/unlit/uf_transparent_circle"
{
    Properties
    {
        _NoiseTex ("Texture", 2D) = "white" {}
				_AlphaEase("Ease", Range(1,2)) = 1.5
				_Color1("Color 1", Color) = (1,1,1,1)
    }
		SubShader
		{
			Tags
			{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}
			LOD 100

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
						float _AlphaEase;
						float4 _Color1;

						float Ease(float x, float a)
						{
							return pow(x, a) / (pow(x, a) + pow(1.0 - x, a));
						}


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
								float2 center = float2(.5, .5);
								float2 offset = i.uv - center;

								float sqrdistance = offset.x * offset.x + offset.y * offset.y;

								float alpha = .5 *.5;
								float norm = 1 - saturate(sqrdistance / alpha);

								float t = Ease(norm, _AlphaEase);

								float4 col = _Color1;
								col.a *= t;

								return col;
            }
            ENDCG
        }
    }
}
