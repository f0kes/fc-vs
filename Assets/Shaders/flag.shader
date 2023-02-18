Shader "Unlit/Flag"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _ColorSecondary ("ColorSecondary", Color) = (1,1,1,1)
        _SecondaryColorLookup ("SecondaryColorLookup", Color) = (0,0,1,1)
        _ColorAdd ("ColorAdd", Color) = (0,0,0,0)
        _WaveFrequency ("WaveFrequency", Float) = 300
        _WaveAmplitude ("WaveAmplitude", Float) = 30
        _WaveSpeed ("WaveSpeed", Float) = 5
        _Height ("Height", Float) = 0
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
            float4 _Color;
            float4 _ColorSecondary;
            float4 _SecondaryColorLookup;
            float _WaveFrequency;
            float _WaveAmplitude;
            float _WaveSpeed;
            float _Height;

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float4, _ColorAdd)
            UNITY_INSTANCING_BUFFER_END(Props)


            v2f vert(appdata v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                
                o.uv = v.uv.xy;

                
                // billboard mesh towards camera
              
                float4 worldCoord = float4(unity_ObjectToWorld._m03, unity_ObjectToWorld._m13, unity_ObjectToWorld._m23,
                                           1);
                float2 scale = float2(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21)
                    );

                
                
                float4 viewPos = mul(UNITY_MATRIX_V, worldCoord);

                float2 vertex = v.vertex.xy * scale;
                vertex *= -viewPos.z;
                viewPos.xy += vertex;
                viewPos.y += _Height * -viewPos.z ;
               
                float4 outPos =  mul(UNITY_MATRIX_P, viewPos);
                
                

                float waveMultiplier = 1 - o.uv.y;

                //change y to time*xpos
                outPos.y = outPos.y + (sin(v.vertex.x * _WaveFrequency + _Time.y * _WaveSpeed) / _WaveAmplitude *
                    waveMultiplier);
                outPos.x = outPos.x + (cos(v.vertex.y * _WaveFrequency + _Time.y * _WaveSpeed) / _WaveAmplitude *
                    waveMultiplier);
                o.pos = outPos;

                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                UNITY_SETUP_INSTANCE_ID(i);

                fixed4 secondaryLookup = _SecondaryColorLookup;
                fixed4 colThis = tex2D(_MainTex, i.uv);
                //if  colthis is blue, use secondary color
                fixed4 col = colThis;
                if (all(colThis == secondaryLookup))
                {
                    col = _ColorSecondary;
                }
                else
                {
                    col *= _Color;
                }
                fixed4 colAdd = UNITY_ACCESS_INSTANCED_PROP(Props, _ColorAdd);
                fixed4 colDark = fixed4(0, 0, 0, 0);
                //multiply coldark alpha
                //colDark.a = colDark.a * sin(i.uv.y*_Time.y * _WaveSpeed)/_WaveAmplitude*(1-i.uv.y);
                col.x = col.x * (1 - colDark.a) + colDark.x * colDark.a;
                col.y = col.y * (1 - colDark.a) + colDark.y * colDark.a;
                col.z = col.z * (1 - colDark.a) + colDark.z * colDark.a;
                colDark.a = col.a;


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}