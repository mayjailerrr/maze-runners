Shader "Custom/CircleTransition"
{
    Properties
    {
        _Color("Overlay Color", Color) = (0,0,0,1)
        _Radius("Circle Radius", Float) = 0.0
        _Center("Circle Center", Vector) = (0.5, 0.5, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Radius;
            float2 _Center;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float dist = distance(uv, _Center);

                // Reveal based on the radius
                if (dist < _Radius)
                    discard;

                return _Color;
            }
            ENDCG
        }
    }
}
