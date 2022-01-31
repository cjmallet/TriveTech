Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Crack1("Crack1", 2D) = "white"{}
        _Crack2("Crack2", 2D) = "white"{}
        _Crack3("Crack3", 2D) = "white"{}
        _Health("Health", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _Crack1;
            sampler2D _Crack2;
            sampler2D _Crack3;
            float4 _MainTex_ST;
            float _Health;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 crack1 = tex2D(_Crack1,i.uv);
                fixed4 crack2 = tex2D(_Crack2, i.uv);
                fixed4 crack3 = tex2D(_Crack3, i.uv);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                if (_Health>0.75) {
                    return col;
                }
                else if (_Health>0.5) {
                    return crack1;
                }
                else if (_Health>0.25) {
                    return crack2;
                }
                else{
                    return crack3;
                }
            }
            ENDCG
        }
    }
}
