Shader "indiversity/unlit/uf_transparent_circle"
{
    Properties
    {
        _NoiseTex ("Texture", 2D) = "white" {}
				_AlphaEase("Ease", Range(0.5,3)) = 1.5
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
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
						float _AlphaEase;

						float Ease(float x, float a)
						{
							return pow(x, a) / (pow(x, a) + pow(1.0 - x, a));
						}


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _NoiseTex);
								o.uv.zw = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
								float noise = tex2D(_NoiseTex, i.uv.zw);
								noise = noise * 0.25;
								noise = noise + 0.75;

								float2 center = float2(.5, .5);
								float2 offset = i.uv - center;

								float sqrdistance = offset.x * offset.x + offset.y * offset.y;

								float alpha = .5 *.5;
								float norm = 1 - saturate(sqrdistance / alpha);
								noise = noise * norm;
								alpha = norm;
                // sample the texture
                fixed4 col = fixed4(1,1,1,1);
								col.a = alpha * noise;
								col.a = Ease(col.a, _AlphaEase);
                return col;
            }
            ENDCG
        }
    }
}
