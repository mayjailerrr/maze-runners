Shader "Custom/DarkenWithMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {} // For rounded edges
        _Darkness ("Darkness Level", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex; // Main Render Texture
            sampler2D _MaskTex; // Mask for rounded edges
            float _Darkness;    // Darkness factor

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord); // Get the main texture color
                float mask = tex2D(_MaskTex, i.texcoord).a; // Get the mask alpha

                col.rgb *= _Darkness; // Apply darkness
                col.a *= mask;        // Preserve rounded edges

                return col;
            }
            ENDCG
        }
    }
}
