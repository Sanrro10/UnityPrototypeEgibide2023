Shader "Custom/ToonBasic"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0.0, 0.1)) = 0.005
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            struct Input
            {
                float2 uv_MainTex;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineWidth;

            void surf(Input IN, inout SurfaceOutput o)
            {
                // Aplica la textura y el color base
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                o.Alpha = c.a;

                // Calcula el valor del borde
                float4 outline = tex2D(_MainTex, IN.uv_MainTex);
                float alpha = 1.0 - step(0.5, outline.a);

                // Aplica el borde
                o.Alpha *= alpha;

                // Calcula la distancia al borde y aplica el color del borde
                float2 d = fwidth(IN.uv_MainTex);
                float alphaOutline = tex2D(_MainTex, IN.uv_MainTex + d).a;
                alphaOutline += tex2D(_MainTex, IN.uv_MainTex - d).a;
                alphaOutline += tex2D(_MainTex, IN.uv_MainTex + float2(d.x, -d.y)).a;
                alphaOutline += tex2D(_MainTex, IN.uv_MainTex + float2(-d.x, d.y)).a;
                alphaOutline *= _OutlineWidth * 0.25;
                alphaOutline = 1.0 - step(0.5, alphaOutline);
                o.Emission = _OutlineColor.rgb * _OutlineColor.a * alphaOutline;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
