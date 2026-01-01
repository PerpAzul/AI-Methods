Shader "EliteIT/LowPolySkybox"
{
    Properties
    {
        _TopColor      ("Top Color", Color)            = (0.4, 0.7, 1.0, 1.0)
        _HorizonColor  ("Horizon Color", Color)        = (0.8, 0.9, 1.0, 1.0)
        _BottomColor   ("Bottom Color", Color)         = (0.6, 0.8, 0.9, 1.0)
        _SkySteps      ("Sky Gradient Steps", Range(2, 10)) = 5
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Background"
            "Queue"="Background"
            "PreviewType"="Skybox"
        }

        Pass
        {
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos     : SV_POSITION;
                float3 viewDir : TEXCOORD0;
            };

            fixed4 _TopColor;
            fixed4 _HorizonColor;
            fixed4 _BottomColor;
            int    _SkySteps;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos     = UnityObjectToClipPos(v.vertex);
                o.viewDir = normalize(v.vertex.xyz);   // direction from cube center
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 viewDir = normalize(i.viewDir);
                float  height  = viewDir.y;

                // Stepped sky gradient around the horizon
                float skyGradient;
                if (height > 0.0)
                {
                    float upper = saturate(height * 2.0);
                    upper = floor(upper * _SkySteps) / _SkySteps;
                    skyGradient = 0.5 + upper * 0.5;   // horizon→zenith
                }
                else
                {
                    float lower = saturate(-height * 2.0);
                    lower = floor(lower * _SkySteps) / _SkySteps;
                    skyGradient = 0.5 - lower * 0.5;   // horizon→nadir
                }

                // Blend colors using stepped gradient
                fixed4 skyColor;
                if (skyGradient > 0.5)
                {
                    float t = (skyGradient - 0.5) * 2.0;
                    skyColor = lerp(_HorizonColor, _TopColor, t);
                }
                else
                {
                    float t = skyGradient * 2.0;
                    skyColor = lerp(_BottomColor, _HorizonColor, t);
                }

                return skyColor;
            }
            ENDCG
        }
    }

    FallBack "Skybox/Procedural"
}
