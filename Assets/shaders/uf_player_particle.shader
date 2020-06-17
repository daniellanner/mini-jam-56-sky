Shader "indiversity/gamejam/uf_player_particle"
{
		Properties
		{
				_NoiseTex ("Texture", 2D) = "white" {}
				[HDR] _Color1 ("Color 1", Color) = (1,1,1,1)
				[HDR] _Color2("Color 2", Color) = (1,1,1,1)
				[HDR] _Color3("Color 3", Color) = (1,1,1,1)
				_Radius("Radius", Range(0.1, 0.5)) = 0.5
				_Ease("Color Blending", Range(0.5, 4.0)) = 2.0
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
						#include "id_utilities.cginc"

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
						float4 _Color1;
						float4 _Color2;
						float4 _Color3;
						float _Radius;
						float _Ease;

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
								float2 screensample = i.uv.zw;
								screensample.y *= 0.5;
								screensample.y -= _Time;

								float colt = tex2D(_NoiseTex, screensample).r;
								colt = (_SinTime.z + 1) * 0.1 + saturate(i.uv.y) * colt * tex2D(_NoiseTex, screensample + float2(colt, 0)).r;

								float leftDelta = Ease(saturate(colt * 2), _Ease);
								float4 leftcolor = lerp(_Color1, _Color2, leftDelta);

								float rightDelta = Ease(saturate(colt - .5) * 2, _Ease);
								float4 rightcolor = lerp(_Color2, _Color3, rightDelta);

								float4 col = rightcolor * GreaterThan(colt, 0.5) + leftcolor * GreaterThan(0.5, colt);
								return col;
						}
						ENDCG
				}
		}
}
