Shader "Custom/VertexColoredUnlit" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Cutoff("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        //Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Tags { "IgnoreProjector"="True" }
        LOD 200
       
        CGPROGRAM
        #pragma surface surf NoLighting alpha alphatest:_Cutoff noforwardadd//fullforwardshadows addshadow 
       
        sampler2D _MainTex;
 
        struct Input {
            float2 uv_MainTex;
            float4 color: Color; // Vertex color
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * IN.color.rgb; // vertex RGB
            o.Alpha = c.a * IN.color.a; // vertex Alpha
        }

        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed4 c;
            c.rgb = s.Albedo; 
            c.a =  0;
            return c;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
 