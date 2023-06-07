Shader "Custom/GuideShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _CarDistance ("Distance from car", Range(0,50)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard addshadow fullforwardshadows vertex:vert alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _CarDistance;

        void vert (inout appdata_full v){
            if(v.vertex.y > 0){
                v.vertex.xyz += v.normal * 0.07*sin(2*_Time.y);
            }else{
                v.vertex.xyz += v.normal * 0.07*sin(2*_Time.y + 3.14);
            }
        }

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            if(sin(30*IN.worldPos.y) < 0)
            {
                clip(-1);
            }

            float2 screen_pos = (IN.screenPos.xy / IN.screenPos.w);

            float lerpFactor = saturate(_CarDistance / 50.0);


            float3 startColor = float3(1, 0, 0);
            float3 middleColor = float3(0, 0, 1);
            float3 endColor = float3(0, 1, 0);

            float3 interpolatedColor;

            if (lerpFactor < 0.5)
            {
                interpolatedColor = lerp(startColor, middleColor, lerpFactor * 2.0);
            }
            else
            {
                interpolatedColor = lerp(middleColor, endColor, (lerpFactor - 0.5) * 2.0);
            }


            o.Albedo = interpolatedColor * _Color.rgb;


            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = _Color.a * (1.0-lerpFactor);
        }
        ENDCG
    }
    FallBack "Diffuse"
}