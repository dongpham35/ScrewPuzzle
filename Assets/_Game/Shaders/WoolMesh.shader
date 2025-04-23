Shader "Horus/Unlit/WoolMesh"
{
    Properties
    {
        _Normal ("Normal", 2D) = "bump" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Display ("Display", Range(0, 1)) = 0.5
        _Scale("Scale", Vector) = (0, 1, 0, 0)
        _LightIntensity ("Light Intensity", Range(0, 2)) = 1.0
        _ScreenTiling ("Screen Tiling", Vector) = (1, 1, 0, 0)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 positionOS : TEXCOORD2;
                float3 normalWS : TEXCOORD3;
                float4 tangentWS : TEXCOORD4;
                float3 bitangentWS : TEXCOORD5;
                float4 screenPos : TEXCOORD6;
                float3 viewDirWS : TEXCOORD7;
            };

            sampler2D _Normal;
            float4 _Normal_ST;
            UNITY_INSTANCING_BUFFER_START(Pros)
            UNITY_DEFINE_INSTANCED_PROP(float, _Display)
            UNITY_DEFINE_INSTANCED_PROP(float4, _Scale)
            UNITY_DEFINE_INSTANCED_PROP(float, _LightIntensity)
            UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
            UNITY_DEFINE_INSTANCED_PROP(float4, _ScreenTiling)
            UNITY_INSTANCING_BUFFER_END(Pros)

            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v,o);
                
                o.positionOS = v.vertex.xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normalWS = UnityObjectToWorldNormal(v.normal);
                o.tangentWS.xyz = UnityObjectToWorldDir(v.tangent.xyz);
                o.tangentWS.w = v.tangent.w;
                o.bitangentWS = cross(o.normalWS, o.tangentWS.xyz) * v.tangent.w;
                o.uv = TRANSFORM_TEX( v.uv, _Normal);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.screenPos.xy = o.screenPos.xy * UNITY_ACCESS_INSTANCED_PROP(Pros, _ScreenTiling).xy;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDirWS = UnityObjectToWorldDir(worldPos);
                return o;
            }

            void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                float posY = 0;
                Unity_Remap_float(i.positionOS.y, UNITY_ACCESS_INSTANCED_PROP(Pros, _Scale).xy, float2(0,1), posY);
                clip(UNITY_ACCESS_INSTANCED_PROP(Pros, _Display) - posY);

                float3x3 tangentToWorldFrontFace = float3x3(
                    normalize(i.tangentWS.xyz),
                    normalize(i.bitangentWS),
                    normalize(i.normalWS)
                );

                float3x3 tangentToWorldBackFace = float3x3(
                    normalize(i.tangentWS.xyz),
                    normalize(i.bitangentWS),
                    normalize(fixed3(0, 1, 0))
                );

                // Check if this is a back face by comparing normal and view direction
                float isBackFace = dot(normalize(i.normalWS), i.viewDirWS) * 0.5 + 0.5;

                // Use screen space coordinates for back faces
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                fixed3 normalBackFace = UnpackNormal(tex2D(_Normal, screenUV));
                fixed3 normalBF = mul(normalBackFace, tangentToWorldBackFace);


                // Front face - use normal mapping and lighting
                float3 normalFrontFace = UnpackNormal(tex2D(_Normal, i.uv));
                fixed3 normalFF = mul(normalFrontFace, tangentToWorldFrontFace);
                fixed3 normalWS = lerp(normalBF, normalFF, isBackFace);

                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float ndotl = max(0, dot(normalWS, lightDir) * 0.5 + 0.5) * UNITY_ACCESS_INSTANCED_PROP(Pros, _LightIntensity);
                float3 diffuse = ndotl * _LightColor0.rgb;

                fixed4 col = fixed4(diffuse, 1.0) * UNITY_ACCESS_INSTANCED_PROP(Pros, _Color);
                return col;
            }
            ENDCG
        }
    }
}