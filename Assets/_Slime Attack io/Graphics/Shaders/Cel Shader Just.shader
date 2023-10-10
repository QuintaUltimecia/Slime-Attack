Shader "Unlit/Cel Shader Just"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [HDR]
        _Color("Color", Color) = (1, 1, 1, 1)
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
			Cull front 
			LOD 100

            Pass
            {
                Cull off

                Stencil
                {
                    Ref 1
                    Comp Always
                    Pass Replace
                }

                Tags
                {
                    "LightMode" = "ForwardBase"
                    "PassFlags" = "OnlyDirectional"
                }

                CGPROGRAM
                #pragma multi_compile_instancing
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog
                #pragma multi_compile_fwdbase

                #include "UnityCG.cginc"
                #include "Lighting.cginc"
                #include "AutoLight.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;

                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float3 worldNormal : NORMAL;
                    float3 viewDir : TEXCOORD1;

                    SHADOW_COORDS(2)

                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                UNITY_INSTANCING_BUFFER_START(Props)
                    UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
                UNITY_INSTANCING_BUFFER_END(Props)

                v2f vert(appdata v)
                {
                    UNITY_SETUP_INSTANCE_ID(v);

                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.viewDir = WorldSpaceViewDir(v.vertex);

                    UNITY_TRANSFER_INSTANCE_ID(v, o);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    UNITY_SETUP_INSTANCE_ID(i);

                    fixed4 color = tex2D(_MainTex, i.uv);

                    return color * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                }
                ENDCG
            }
        }
            FallBack "Mobile/Diffuse"
}