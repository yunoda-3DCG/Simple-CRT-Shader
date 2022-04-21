Shader "Simple CRT"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DecalTex ("Decal Texture", 2D) = "white" {}
        _FilmDirtTex ("Dirt Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 decaluv : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            int _WhiteNoiseOnOff;
            int _ScanlineOnOff;
            int _MonochormeOnOff;

            int _LetterBoxOnOff;
            int _LetterBoxEdgeBlur;
            int _LetterBoxType;
            
            float _ScreenJumpLevel;
            
            float _FlickeringStrength;
            float _FlickeringCycle;
            
            int _SlippageOnOff;
            float _SlippageStrength;
            float _SlippageInterval;
            float _SlippageScrollSpeed;
            float _SlippageNoiseOnOff;
            float _SlippageSize;

            float _ChromaticAberrationStrength;
            int _ChromaticAberrationOnOff;

            int _MultipleGhostOnOff;
            float _MultipleGhostStrength;

            sampler2D _DecalTex;
            float4 _DecalTex_ST;
            int _DecalTexOnOff;
            float2 _DecalTexPos;
            float2 _DecalTexScale;

            int _FilmDirtOnOff;
            sampler2D _FilmDirtTex;
            float4 _FilmDirtTex_ST;

            float GetRandom(float x);
            float EaseIn(float t0, float t1, float t);

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.decaluv = TRANSFORM_TEX(v.uv, _DecalTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                /////Jump noise
                uv.y = frac(uv.y + _ScreenJumpLevel);
                /////

                /////frickering
                float flickeringNoise = GetRandom(_Time.y);
                float flickeringMask = pow(abs(sin(i.uv.y * _FlickeringCycle + _Time.y)), 10);
                uv.x = uv.x + (flickeringNoise * _FlickeringStrength * flickeringMask); 
                /////

                /////slippage
                float scrollSpeed = _Time.x * _SlippageScrollSpeed;
                float slippageMask = pow(abs(sin(i.uv.y * _SlippageInterval + scrollSpeed)), _SlippageSize);
                float stepMask = round(sin(i.uv.y * _SlippageInterval + scrollSpeed - 1));
                uv.x = uv.x + (_SlippageNoiseOnOff * _SlippageStrength * slippageMask * stepMask) * _SlippageOnOff; 
                /////

                /////Chromatic Aberration
                float red = tex2D(_MainTex, float2(uv.x - _ChromaticAberrationStrength * _ChromaticAberrationOnOff, uv.y)).r;
                float green = tex2D(_MainTex, float2(uv.x, uv.y)).g;
                float blue = tex2D(_MainTex, float2(uv.x + _ChromaticAberrationStrength * _ChromaticAberrationOnOff, uv.y)).b; 
                float4 color = float4(red, green, blue, 1);
                /////

                /////Multiple Ghost
                float4 ghost1st = tex2D(_MainTex, uv - float2(1, 0) * _MultipleGhostStrength * _MultipleGhostOnOff);
                float4 ghost2nd = tex2D(_MainTex, uv - float2(1, 0) * _MultipleGhostStrength * 2 * _MultipleGhostOnOff);
                color = color * 0.8 + ghost1st * 0.15 + ghost2nd * 0.05;
                /////

                /////File dirt
                float2 pp = -1.0 + 2.0 * uv;
                float time = _Time.x;
                float aaRad = 0.1;
                float2 nseLookup2 = pp + time * 1000;
                float3 nse2 =
                    tex2D(_FilmDirtTex, 0.1 * nseLookup2.xy).xyz +
                    tex2D(_FilmDirtTex, 0.01 * nseLookup2.xy).xyz +
                    tex2D(_FilmDirtTex, 0.004 * nseLookup2.xy).xyz;
                float thresh = 0.6;
                float mul1 = smoothstep(thresh - aaRad, thresh + aaRad, nse2.x);
                float mul2 = smoothstep(thresh - aaRad, thresh + aaRad, nse2.y);
                float mul3 = smoothstep(thresh - aaRad, thresh + aaRad, nse2.z);
                
                float seed = tex2D(_FilmDirtTex, float2(time * 0.35, time)).x;
                
                float result = clamp(0, 1, seed + 0.7);
                
                result += 0.06 * EaseIn(19.2, 19.4, time);

                float band = 0.05;
                if(_FilmDirtOnOff == 1)
                {
                if( 0.3 < seed && 0.3 + band > seed )
                    color *=  mul1 * result;
                else if( 0.6 < seed && 0.6 + band > seed )
                    color *= mul2 * result;
                else if( 0.9 < seed && 0.9 + band > seed )
                    color *= mul3 * result;
                }
                /////

                /////Letter box
                float band_uv = fmod(_MainTex_TexelSize.z, 640) / _MainTex_TexelSize.z / 2;
                if(i.uv.x < band_uv || 1 - band_uv < i.uv.x)
                {
                    float pi = 6.28318530718; 
                    float directions = 16.0; 
                    float quality = 3.0; 
                    float size = 8.0; 
                
                    float2 Radius = size * _MainTex_ST.zw;
                    float4 samplingColor = tex2D(_MainTex, uv);
                    
                    for(float d = 0.0; d < pi; d += pi / directions)
                    {
                        for(float i = 1.0 / quality; i <= 1.0; i += 1.0 / quality)
                        {
                            samplingColor += tex2D(_MainTex, uv + float2(cos(d), sin(d)) * 0.015 * i);		
                        }
                    }
                    samplingColor /= quality * directions - 15.0;
                    
                    if(_LetterBoxOnOff == 1)
                    {
                        color = color;
                    }
                    else if(_LetterBoxType == 0) // LetterBox is Black
                    {
                        color = 0;
                    }
                    else if(_LetterBoxType == 1) // LetterBox is Blur
                    {
                        color = samplingColor;
                    }
                }
                /////

                /////White noise
                if(_WhiteNoiseOnOff == 1)
                {
                    return frac(sin(dot(i.uv, float2(12.9898, 78.233)) + _Time.x) * 43758.5453);
                }
                /////

                /////Decal texture
                float4 decal = tex2D(_DecalTex, (i.decaluv - _DecalTexPos) * _DecalTexScale) * _DecalTexOnOff;
                color = color * (1 - decal.a) + decal;
                /////

                /////Scanline
                float scanline = sin((i.uv.y + _Time.x) * 800.0) * 0.04;
                color -= scanline * _ScanlineOnOff;
                /////

                //////scanline noise
                float noiseAlpha = 1;
                if(pow(sin(uv.y + _Time.y * 2), 200) >= 0.999)
                {
                    noiseAlpha = GetRandom(uv.y);
                    //color *= noiseAlpha;
                }
                //////

                //////Monochorome
                if(_MonochormeOnOff == 1)
                {
                    color.xyz = 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
                }
                //////

                return color;
            }

            float GetRandom(float x)
            {
                return frac(sin(dot(x, float2(12.9898, 78.233))) * 43758.5453);
            }

            float EaseIn(float t0, float t1, float t)
            {
                return 2.0 * smoothstep(t0, 2.0 * t1 - t0, t);
            }
            ENDCG
        }
    }
}
