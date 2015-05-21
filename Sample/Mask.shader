﻿Shader "Tutorial/Textured Colored" { 

    Properties { 


        _Color ("Main Color", Color) = (1,1,1,0.5) 
        _MainTex ("Texture", 2D) = "white" { } 
        _FrameNumber("Frame Number" , int) = 1
        _AlphaTex ("Texture", 2D) = "white" { } } 


    SubShader {
        
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Less
     
        Pass 
        {
     
           CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
            
           #include "UnityCG.cginc"
            
            float4 _Color;
            int _FrameNumber;
            sampler2D _MainTex;
            sampler2D _AlphaTex;
            
            struct v2f {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
                float2  texcoord : TEXCOORD0;
            };
            
            float4 _MainTex_ST;
            float4 _AlphaTex_ST;
            
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
                o.texcoord = v.texcoord;
                return o;
            }
            
            half4 frag (v2f i) : COLOR
            {
            	
                float2 tmp;
                tmp.x = i.texcoord.x;
                tmp.y = i.texcoord.y;
                
                int index = 0;
                float delta = 1.0/_FrameNumber;
                
                while (index < _FrameNumber){
                	
                	if (delta * index > tmp.y)
                		break;
                	index ++;
                }
                
                float cap = index * delta;
                tmp.y = 1 - (cap - tmp.y)/delta;
                half4 texcol = tex2D (_MainTex, i.uv);
                texcol.a = tex2D (_AlphaTex, TRANSFORM_TEX (tmp, _AlphaTex) ).a * texcol.a;
                return texcol * _Color;
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
 } 
