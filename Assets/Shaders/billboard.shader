Shader "Unlit/Billboard"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "Opaque" "DisableBatching" = "True"
        }

        Cull Off
        ZWrite On
        ZTest LEqual
        Blend One Zero
        AlphaToMask On
        //Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            #pragma instancing_options procedural:setup

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };


            sampler2D _MainTex;
            float4 _Color;
            float4 _MainTex_ST;
            float _SpriteSheetData[];

            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            StructuredBuffer<float4> positionBuffer;
            #endif

            void rotate2D(inout float2 v, float r) {
                float s, c;
                sincos(r, s, c);
                v = float2(v.x * c - v.y * s, v.x * s + v.y * c);
            }
 
            void setup() {
                #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
                    /*float4 data = positionBuffer[unity_InstanceID];

                    float rotation = data.w * data.w * _Time.y * 0.5f;
                    rotate2D(data.xz, rotation);

                    unity_ObjectToWorld._11_21_31_41 = float4(data.w, 0, 0, 0);
                    unity_ObjectToWorld._12_22_32_42 = float4(0, data.w, 0, 0);
                    unity_ObjectToWorld._13_23_33_43 = float4(0, 0, data.w, 0);
                    unity_ObjectToWorld._14_24_34_44 = float4(data.xyz, 1);
                    unity_WorldToObject = unity_ObjectToWorld;
                    unity_WorldToObject._14_24_34 *= -1;
                    unity_WorldToObject._11_22_33 = 1.0f / unity_WorldToObject._11_22_33;*/
                #endif
            }


            v2f vert(appdata v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv.xy;


                // billboard mesh towards camera
                float3 vpos = mul((float3x3)unity_ObjectToWorld, v.vertex.xyz);
                float4 worldCoord = float4(unity_ObjectToWorld._m03, unity_ObjectToWorld._m13, unity_ObjectToWorld._m23,
                                           1);
                float4 viewPos = mul(UNITY_MATRIX_V, worldCoord) + float4(vpos, 0);
                float4 outPos = mul(UNITY_MATRIX_P, viewPos);

                o.pos = outPos;

                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                UNITY_SETUP_INSTANCE_ID(i);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                //col = col+ float4(0.5, 0.5, 0.5, col.a);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}