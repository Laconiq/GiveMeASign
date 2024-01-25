Shader "Custom/SpriteGradient" {
    Properties {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Left Color", Color) = (1,1,1,1)
        _Color2 ("Right Color", Color) = (1,1,1,1)
        _Scale ("Scale", Float) = 1
        _GradientAngle ("Gradient Angle", Float) = 0 // Angle en degrés

        // Propriétés UI requises
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert  
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            fixed4 _Color2;
            fixed  _Scale;
            float _GradientAngle;

            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 col : COLOR;
            };

            float2 RotateUV(float2 uv, float angle) {
                float rad = radians(angle);
                float s = sin(rad);
                float c = cos(rad);
                float2x2 rotMatrix = float2x2(c, -s, s, c);
                return mul(rotMatrix, uv - 0.5) + 0.5;
            }

            v2f vert (appdata_full v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float2 rotatedUV = RotateUV(v.texcoord.xy, _GradientAngle);
                o.col = lerp(_Color, _Color2, rotatedUV.x);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                return i.col; // La couleur contient déjà l'alpha pour la transparence
            }
            ENDCG
        }
    }
}
