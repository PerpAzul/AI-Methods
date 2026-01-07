Shader "Custom/BoxMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;

            float3 _BoxCenter;
            float3 _BoxSize;
            float4x4 _BoxMatrix;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // World → Box local
                float3 localPos = mul(_BoxMatrix, float4(i.worldPos, 1)).xyz;

                float3 halfSize = _BoxSize * 0.5;

                bool inside =
                    abs(localPos.x) < halfSize.x &&
                    abs(localPos.y) < halfSize.y &&
                    abs(localPos.z) < halfSize.z;

                clip(inside ? -1 : 1);

                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
