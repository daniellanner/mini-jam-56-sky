Shader "indiversity/gamejam/uf_missile"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
				_NoiseTex("Texture", 2D) = "white" {}
				[HDR] _Color1("Color 1", Color) = (1,1,1,1)
				[HDR] _Color2("Color 2", Color) = (1,1,1,1)
				[HDR] _Color3("Color 3", Color) = (1,1,1,1)
				_Ease("Color Blending", Range(0.5, 4.0)) = 2.0
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
						sampler2D _NoiseTex;
						float4 _NoiseTex_ST;
						float4 _Color1;
						float4 _Color2;
						float4 _Color3;
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

								float colt = saturate(i.uv.y) * tex2D(_NoiseTex, screensample).r;
								
								float leftDelta = Ease(saturate(colt * 2), _Ease);
								float4 leftcolor = lerp(_Color1, _Color2, leftDelta);

								float rightDelta = Ease(saturate(colt - .5) * 2, _Ease);
								float4 rightcolor = lerp(_Color2, _Color3, rightDelta);

								float4 col = rightcolor * GreaterThan(colt, 0.5) + leftcolor * GreaterThan(0.5, colt);
                // sample the texture
                col.a = tex2D(_MainTex, i.uv).a;
                return col;
            }
            ENDCG
        }
    }
}
