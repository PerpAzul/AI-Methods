Shader "EliteIT/LowPolyRiver"
{
    Properties
    {
        _ShallowColor ("Shallow Water Color", Color) = (0.3, 0.7, 0.9, 0.8)
        _DeepColor ("Deep Water Color", Color) = (0.1, 0.3, 0.6, 1.0)
        _EdgeColor ("Edge Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _DepthFactor ("Depth Factor", Range(0.1, 5.0)) = 1.0
        _EdgeThickness ("Edge Thickness", Range(0.01, 3.0)) = 0.5
        _DepthSteps ("Depth Steps", Range(2, 10)) = 4
        _SpecularColor ("Specular Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _SpecularPower ("Specular Power", Range(1.0, 100.0)) = 32.0
        _SpecularIntensity ("Specular Intensity", Range(0.0, 2.0)) = 1.0
        _GlitterSize ("Glitter Size", Range(10.0, 200.0)) = 50.0
        _GlitterDensity ("Glitter Density", Range(0.1, 2.0)) = 1.0
        _Transparency ("Transparency", Range(0.0, 1.0)) = 0.8
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float eyeDepth : TEXCOORD4;
                float3 viewDir : TEXCOORD5;
            };
            
            fixed4 _ShallowColor;
            fixed4 _DeepColor;
            fixed4 _EdgeColor;
            float _DepthFactor;
            float _EdgeThickness;
            int _DepthSteps;
            fixed4 _SpecularColor;
            float _SpecularPower;
            float _SpecularIntensity;
            float _GlitterSize;
            float _GlitterDensity;
            float _Transparency;
            
            sampler2D _CameraDepthTexture;
            
            // Simple hash function for glitter
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }
            
            v2f vert(appdata v)
            {
                v2f o;
                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.pos);
                o.eyeDepth = -UnityObjectToViewPos(v.vertex).z;
                o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos);
                
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                // Sample depth texture
                float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                float surfaceZ = i.eyeDepth;
                
                // Calculate water depth
                float depthDifference = sceneZ - surfaceZ;
                float rawDepth = saturate(depthDifference * _DepthFactor);
                
                // Create hard-edged depth steps for low poly look
                float steppedDepth = floor(rawDepth * _DepthSteps) / _DepthSteps;
                
                // Calculate sharp edge factor
                float edgeDistance = depthDifference / _EdgeThickness;
                float edgeFactor = 1.0 - saturate(edgeDistance);
                
                // Make edges extremely sharp and well-defined
                edgeFactor = step(0.5, edgeFactor); // Hard cutoff for sharp edges
                
                // Blend shallow and deep colors with hard steps
                fixed4 waterColor = lerp(_ShallowColor, _DeepColor, steppedDepth);
                
                // Apply sharp edge color
                waterColor = lerp(waterColor, _EdgeColor, edgeFactor);
                
                // Apply transparency
                waterColor.a *= _Transparency;
                
                // Responsive flat lighting for low poly look
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 normal = normalize(i.normal);
                
                // Calculate light contribution
                float NdotL = saturate(dot(normal, lightDir));
                
                // Get ambient lighting from Unity's lighting system
                float3 ambient = ShadeSH9(half4(normal, 1.0));
                
                // Apply proper light color and intensity
                float3 diffuse = NdotL * _LightColor0.rgb * _LightColor0.a;
                
                // Combine ambient and diffuse, but keep it flat for low poly style
                float3 finalLighting = ambient + diffuse;
                
                // Apply lighting to water color
                waterColor.rgb *= finalLighting;
                
                return waterColor;
            }
            ENDCG
        }
    }
    
    FallBack "Transparent/Diffuse"
}