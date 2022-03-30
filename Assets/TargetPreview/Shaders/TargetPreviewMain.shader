Shader "TargetPreview/Main"
{
    Properties
    {
        _Cube("Reflection Map", CUBE) = "" {}
        _Roughness("Roughness", Range(0.0, 10.0)) = 0.0
        _MainTex ("Texture", 2D) = "white" {}
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
            #pragma target 3.0

            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature_local _RENDER_REFLECTIVE
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
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


            };

            

            samplerCUBE _Cube;
            float _Roughness;
            half4 _Color;
            sampler2D _MainTex;
            half4 _MainTex_ST;


            v2f vert(appdata v)
            {
                v2f o;

#if _RENDER_REFLECTIVE
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
#else
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
#endif
                o.pos = UnityObjectToClipPos(v.vertex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
#if _RENDER_REFLECTIVE //Just render color and tint.
                half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                half3 reflection = reflect(-worldViewDir, i.worldNormal);
                half4 skyData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflection, _Roughness);
                half3 skyColor = DecodeHDR(skyData, unity_SpecCube0_HDR);
                return half4(skyColor, 1.0) * _Color;
#else
                fixed4 mainTextureColor = tex2D(_MainTex, i.uv);
                return _Color * mainTextureColor;
#endif


            }
            ENDCG
        }
    }
    FallBack Off
}