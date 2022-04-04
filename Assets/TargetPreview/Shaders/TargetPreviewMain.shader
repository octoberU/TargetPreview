Shader "TargetPreview/Main"
{
    Properties
    {
        _Cube("Reflection Map", CUBE) = "" {}
        _Roughness("Roughness", Range(0.0, 10.0)) = 0.0
        _TargetTextures ("Texture", 2DArray) = "" {}
        _TextureIndex ("Texture Index", Range(0, 7)) = 0
        [HDR]_Color ("Main Color", Color) = (1,1,1,1)
        [Toggle(_RENDER_REFLECTIVE)]_RenderReflective("_RenderReflective", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        pass
        {
            CGPROGRAM
            #pragma target 3.5

            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature_local _RENDER_REFLECTIVE
            #pragma multi_compile_instancing
            #pragma require 2darray
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
#if _RENDER_REFLECTIVE
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
#else

                float2 uv : TEXCOORD0;
#endif
                UNITY_VERTEX_INPUT_INSTANCE_ID

            };

            
            
            samplerCUBE _Cube;
            float _Roughness;
            sampler2D _MainTex;
            half4 _MainTex_ST;

            UNITY_DECLARE_TEX2DARRAY(_TargetTextures);

            UNITY_INSTANCING_BUFFER_START(Props)
            half4 _Color;
            float _TextureIndex;
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

#if _RENDER_REFLECTIVE
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
#else
                o.uv = v.uv;
#endif
                o.pos = UnityObjectToClipPos(v.vertex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
#if _RENDER_REFLECTIVE //Just render color and tint.
                half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                half3 reflection = reflect(-worldViewDir, i.worldNormal);
                half4 skyData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflection, _Roughness);
                half3 skyColor = DecodeHDR(skyData, unity_SpecCube0_HDR);
                return half4(skyColor, 1.0) * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
#else
                float3 projectedCoordinates = float3(i.uv, UNITY_ACCESS_INSTANCED_PROP(Props, _TextureIndex));
                half4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                
                fixed4 mainTextureColor = UNITY_SAMPLE_TEX2DARRAY(_TargetTextures, projectedCoordinates);
                return mainTextureColor * color;
#endif


            }
            ENDCG
        }
    }
    FallBack Off
}