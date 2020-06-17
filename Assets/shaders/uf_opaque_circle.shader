Shader "indiversity/unlit/uf_opaque_circle"
{
		Properties
		{
				_NoiseTex ("Texture", 2D) = "white" {}
				_Color1("Color 1", Color) = (1,1,1,1)
				_Color2("Color 2", Color) = (1,1,1,1)
				_AlphaEase("Ease", Range(1,2)) = 1.5
		}
		SubShader
		{
			Tags
			{
				"RenderType" = "Opaque"
			}
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
								float4 uv : TEXCOORD0;
								float4 vertex : SV_POSITION;
						};

						sampler2D _NoiseTex;
						float4 _NoiseTex_ST;
						float _AlphaEase;
						fixed4 _Color1;
						fixed4 _Color2;


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

								float t = alpha * noise;
								t = Ease(t, _AlphaEase);

								fixed4 col = lerp(_Color1, _Color2, t);
								return col;
						}
						ENDCG
				}
		}
}
