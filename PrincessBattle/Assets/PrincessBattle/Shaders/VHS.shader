Shader "PrincessBattle/VHS"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SndTex ("Secondary Texture", 2D) = "white" {}
        _OffsetNoiseX ("Offset Noise X", float) = 0.0
        _OffsetNoiseY ("Offset Noise Y", float) = 0.0
        _OffsetPosY ("Offset position Y", float) = 0.0
        _OffsetColor ("Offset Color", Range(0.005, 0.1)) = 0
        _OffsetDistortion ("Offset Distortion", float) = 500
        _Intensity ("Mask Intensity", Range(0.0, 1)) = 1.0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
             
            #include "UnityCG.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
                half2 offset : TEXCOORD2;
                half2 offsetN : TEXCOORD3;
            };

            half _OffsetNoiseX;
            half _OffsetNoiseY;
            float _OffsetColor;

            v2f vert (appdata_base v)
            {
                v2f o;
                    
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.uv2 = v.texcoord + float2(_OffsetNoiseX - 0.1f, _OffsetNoiseY);

                o.offset = half2(_OffsetColor, _OffsetColor);
                o.offsetN = half2(-_OffsetColor, -_OffsetColor);
                    
                return o;
            }
             
            sampler2D _MainTex;
            sampler2D _SndTex;

            fixed _Intensity;
            half _OffsetPosY;
            half _OffsetDistortion;

            fixed4 frag (v2f i) : SV_Target
            {
                i.uv = float2(frac(i.uv.x + cos((i.uv.y + _CosTime.y / 4) * 100) / _OffsetDistortion), frac(i.uv.y + _OffsetPosY));

                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_SndTex, i.uv2);

                // col.g = tex2D(_MainTex, i.uv + i.offset).g;
                // col.b = tex2D(_MainTex, i.uv + i.offsetN).b;

                return lerp(col, col2, ceil(col2.r - _Intensity)) * (1 - ceil(saturate(abs(i.uv.y - 0.5) - 0.49)));
            }
            ENDCG
        }
    }
}
