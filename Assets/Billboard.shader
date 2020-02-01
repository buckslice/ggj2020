
Shader "Billboard/Sprite" {
    Properties
    {
        //[PerRendererData] 
        _MainTex("Sprite Texture", 2D) = "white" {}
        _ScaleX("Scale X", Float) = 1.0
        _ScaleY("Scale Y", Float) = 1.0
        _OffsetY("Offset Y", Float) = 0.0
        _TilingOffset("Tiling/Offset", Vector) = (0,0,0,0)
    }

        SubShader
    {
        Tags
    {
        "Queue" = "Transparent"
        "IgnoreProjector" = "True"
        "RenderType" = "Transparent"
        "PreviewType" = "Plane"
        "CanUseSpriteAtlas" = "True"
        //"DisableBatching" = "True"
    }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
    {
        CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma multi_compile_instancing
    #include "UnityCG.cginc"

    sampler2D _MainTex;
    //float4 _MainTex_ST;
    //fixed4 _TilingOffset;
    uniform float _ScaleX;
    uniform float _ScaleY;
    uniform float _OffsetY;

    UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_DEFINE_INSTANCED_PROP(fixed4, _TilingOffset)
    UNITY_INSTANCING_BUFFER_END(Props)

    struct appdata {
        UNITY_VERTEX_INPUT_INSTANCE_ID
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
    };

    struct v2f {
        float4 vertex : SV_POSITION;
        float2 uv : TEXCOORD0;
    };

    v2f vert(appdata IN) {
        v2f OUT;
        UNITY_SETUP_INSTANCE_ID(IN);

        // billboarditize
        float4 model = mul(UNITY_MATRIX_M, float4(0, (IN.vertex.y + _OffsetY) * _ScaleY, 0.0, 1.0));
        float4 view = mul(UNITY_MATRIX_V, model);
        float4 proj = mul(UNITY_MATRIX_P, view
            + float4(IN.vertex.x, 0.0, 0.0, 0.0)
            * float4(_ScaleX, 1.0, 1.0, 1.0));
        OUT.vertex = proj;

        //OUT.texcoord = IN.texcoord;
        //OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);

        fixed4 f = UNITY_ACCESS_INSTANCED_PROP(Props, _TilingOffset);
        //fixed4 f = _TilingOffset;
        OUT.uv = IN.uv.xy * f.xy + f.zw;

        return OUT;
    }

    fixed4 frag(v2f IN) : COLOR
    {
        fixed4 color = tex2D(_MainTex, IN.uv);
        color.rgb *= color.a;

        return color;
    }
        ENDCG
    }
    }
}