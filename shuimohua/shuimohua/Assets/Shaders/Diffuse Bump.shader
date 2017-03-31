Shader "Custom/Diffuse Bump" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_Tooniness("Tooniness", Range(0.1,20)) = 4
		_SpecularPower ("Specular Power", Range(0, 30)) = 2
		_SpecularColor ("Specular Color", Color) = (1,1,1,1)
		_AlphaCutOff ("AlphaCutOff", Range(0.0, 1)) = 0.5
		_Outline ("Outline", Range(0,1)) = 0.4
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Transparent"}
		LOD 200
		
		pass
		{
			cull front
			lighting off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			float _Outline;
			struct v2f
			{
				float4 pos : SV_POSITION;
				//float3 worldNormal : TEXCOORD0;
			};
			
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MV, float4(v.vertex.xyz, 1.0f));
					
				float3 worldNormal = normalize(mul(UNITY_MATRIX_IT_MV, v.normal));
					
				worldNormal.z = 0;
				
				o.pos = o.pos + float4(worldNormal, 0) * _Outline;
				o.pos = mul(UNITY_MATRIX_P, o.pos);

				return o;
			}
			
			fixed4 frag(v2f i) : COLOR
			{
				return fixed4(0, 0, 0, 1);
			}
			
			ENDCG
		}
		
		pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			cull back
			lighting on
	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#include "UnityCG.cginc"
			#include "Lighting.cginc"  
			#include "AutoLight.cginc"  

			sampler2D _MainTex;
			sampler2D _BumpMap;
			float4 _MainTex_ST;
			float _SpecularPower;
			float4 _SpecularColor;
			float _AlphaCutOff;
			float _Tooniness;
			//float _Outline;
			struct v2f
			{
				float4 pos : SV_POSITION;
				half4 uv : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				
				float3 lightDir : TEXCOORD2;
				float3 viewDir : TEXCOORD3;
				//float3 UnityLightCorlor0 : COLOR;
				//LIGHTING_COORDS(3, 4)
				//half3 viewDirTS : TEXCOORD2;
				//half3 lightDirTS : TEXCOORD3;
				//fixed4 color : COLOR;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz, 1.0f));

				//#define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw)
				o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);

				//把法线转化到世界空间  
				//float3 worldNormal = mul(_Object2World, v.normal).xyz;
				//归一化法线  
				o.worldNormal = normalize(mul((float3x3)_Object2World, v.normal));
				//o.worldNormal = worldNormal;
				//o.UnityLightCorlor0 = unity_LightColor0.xyz;cc
				//fixed4 _Diffuse = tex2D(_MainTex, o.uv);
				o.lightDir = normalize(WorldSpaceLightDir(v.vertex));
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
				//float cosValue = max(0.0, dot(worldNormal, lightDir));
				//o.color = fixed4(cosValue * _Diffuse.xyz * unity_LightColor0.xyz, 1.0);
				//TRANSFER_VERTEX_TO_FRAGMENT(output);

				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				fixed4 texColor = tex2D(_MainTex, i.uv);
				
				// alpha test
  				clip (texColor.a - _AlphaCutOff);
				
				float diff = max(0.0, dot(i.worldNormal, i.lightDir));
			
				float3 reflectionVector = normalize(2.0 * i.worldNormal * diff - i.lightDir);
				
				float h = normalize((i.lightDir + i.viewDir) / 2);
				
				
				//V * R 
				//float spec = pow(max(0, dot(i.viewDir, reflectionVector)), _SpecularPower);
				
				
				//N * H
				float spec = pow(max(0, dot(i.worldNormal, h)), _SpecularPower);
				
				float3 finalSpec = 0.1 * _SpecularPower * spec;
				
				float halfLambert = diff * 0.5 + 0.5;
				//float attenuation = LIGHT_ATTENUATION(input);
				//fixed4 color = fixed4(diff * texColor.xyz * _LightColor0 , 1.0f);
				
				fixed4 lightColor = diff * _LightColor0 + UNITY_LIGHTMODEL_AMBIENT;
				
				//lightColor = clamp(lightColor, fixed4(0.0f, 0.0f, 0.0f, 0.0f), fixed4(1.0f, 1.0f, 1.0f, 1.0f));
				lightColor = saturate(lightColor);
				
				fixed4 color = fixed4(lightColor * texColor.xyz * 2, texColor.a);
								//return color;
				//return fixed4(_LightColor0, 1.0);
				
				//half edge = saturate(dot (i.worldNormal, normalize(i.viewDir)));   
				//edge = edge < _Outline ? edge/4 : 1;
				
				//color = floor(color * _Tooniness) / _Tooniness * edge;
				color = floor(color * _Tooniness) / _Tooniness;
				return color;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
