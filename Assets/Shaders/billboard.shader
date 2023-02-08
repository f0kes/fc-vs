Shader "Unlit/Billboard"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

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
            float4 _MainTex_ST;
            

            v2f vert(appdata v)
            {
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

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}