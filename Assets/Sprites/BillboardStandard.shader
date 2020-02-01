// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Billboard/StandardAtlas"
{
    Properties
    {
        _Color("Tint", Color) = (1,1,1,1)
        [NoScaleOffset] _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _Cutoff("Alpha cutoff", Range(0,1)) = 0.5
        /*_ScaleX("Scale X", Float) = 1.0
        _ScaleY("Scale Y", Float) = 1.0
        _OffsetY("Offset Y", Float) = 0.0*/
    }
        SubShader
    {
        Tags{
        //"RenderType" = "Opaque" "Queue" = "Geometry+1"
        //"RenderType"="TransparentCutout"
        //"Queue" = "Transparent"
        //"PreviewType" = "Plane" 
            "DisableBatching" = "True"
        }
    LOD 200

        //ZWrite Off
        //Blend One OneMinusSrcAlpha
        //Cull Off // disable backface culling not needed with billboards

    CGPROGRAM
    #pragma surface surf Standard vertex:vert alphatest:_Cutoff fullforwardshadows addshadow

    // Use shader model 3.0 target, to get nicer looking lighting
    #pragma target 3.0

    sampler2D _MainTex;

    struct Input {
        float2 uv_MainTex;
        float4 color : COLOR;
    };

    half _Glossiness;
    half _Metallic;
    fixed4 _Color;
    uniform float _ScaleX;
    uniform float _ScaleY;
    uniform float _OffsetY;

    void vert(inout appdata_full v) {

        // i love bgolus omggg
        //https://forum.unity.com/threads/standard-surface-shader-billboard.513060/
        // get the camera basis vectors
        float3 forward = -normalize(UNITY_MATRIX_V._m20_m21_m22);
        float3 up = normalize(UNITY_MATRIX_V._m10_m11_m12);
        //float3 up = float3(0, 1, 0);
        float3 right = normalize(UNITY_MATRIX_V._m00_m01_m02);

        // rotate to face camera
        float4x4 rotationMatrix = float4x4(
            right, 0,
            up, 0,
            forward, 0,
            0, 0, 0, 1
        );
        v.vertex = mul(v.vertex, rotationMatrix);
        v.normal = mul(v.normal, rotationMatrix);

        // undo object to world transform surface shader will apply
        v.vertex.xyz = mul((float3x3)unity_WorldToObject, v.vertex.xyz);
        v.normal = mul(v.normal, (float3x3)unity_ObjectToWorld);

        // shadows look slighty scuffed but shadow bias of 0.3 makes it less noticeable
    }

    void surf(Input IN, inout SurfaceOutputStandard o) {
        // Albedo comes from a texture tinted by color
        fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
        o.Albedo = c.rgb;
        // Metallic and smoothness come from slider variables
        o.Metallic = _Metallic;
        o.Smoothness = _Glossiness;
        o.Alpha = c.a;
    }
    ENDCG
    }
    //FallBack "Transparent/Cutout/VertexLit"
}