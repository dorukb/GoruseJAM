Shader "Unlit/Water"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Specular ("Specular", 2D) = "white" {}
        _Refract ("Reflect", 2D) = "white" {}
        _Reflect ("Refract", 2D) = "white" {}
        _Amount  ("ReflectRefractAmount", float) = 0
        [ShowAsVector2] _Tiling  ("Tiling", Vector) = (0, 0, 0, 0)
        [ShowAsVector2] _Speed  ("Speed", Range(0, 1)) = 0
        [ShowAsVector2] _Height ("Height",  Range(0, 1)) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Cull Off
        ZWrite On
        ZTest Less
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #define DIV_289 0.00346020761245674740484429065744f


            float3 mod(float3 x, float3 y)
            {
	            return x - y * floor(x / y);
            }

            float3 mod289(float3 x)
            {
	            return x - floor(x / 289.0) * 289.0;
            }

            float4 mod289(float4 x)
            {
	            return x - floor(x / 289.0) * 289.0;
            }

            float3 permute(float3 x)
            {
	            return mod289(((x*34.0)+1.0)*x);
            }

            float4 permute(float4 x)
            {
	            return mod289(((x*34.0)+1.0)*x);
            }

            float4 taylorInvSqrt(float4 r)
            {
	            return (float4)1.79284291400159 - r * 0.85373472095314;
            }

            float3 fade(float3 t) {
	            return t*t*t*(t*(t*6.0-15.0)+10.0);
            }
            
            float2 fade(float2 t) {
                return 6.0 * pow(t, 5.0) - 15.0 * pow(t, 4.0) + 10.0 * pow(t, 3.0);
            }
            
            float mod289(float x) {
                return x - floor(x * DIV_289) * 289.0;
            }

            float3 dist (float3 x, float3 y, float3 z,  bool manhattanDistance)
			{
				return manhattanDistance ?  abs(x) + abs(y) + abs(z) :  (x * x + y * y + z * z);
			}
 
            float PerlinNoise2D(float2 P)
            {
                  float4 Pi = floor(P.xyxy) + float4(0.0, 0.0, 1.0, 1.0);
                  float4 Pf = frac (P.xyxy) - float4(0.0, 0.0, 1.0, 1.0);
 
                  float4 ix = Pi.xzxz;
                  float4 iy = Pi.yyww;
                  float4 fx = Pf.xzxz;
                  float4 fy = Pf.yyww;
 
                  float4 i = permute(permute(ix) + iy);
 
                  float4 gx = frac(i / 41.0) * 2.0 - 1.0 ;
                  float4 gy = abs(gx) - 0.5 ;
                  float4 tx = floor(gx + 0.5);
                  gx = gx - tx;
 
                  float2 g00 = float2(gx.x,gy.x);
                  float2 g10 = float2(gx.y,gy.y);
                  float2 g01 = float2(gx.z,gy.z);
                  float2 g11 = float2(gx.w,gy.w);
 
                  float4 norm = taylorInvSqrt(float4(dot(g00, g00), dot(g01, g01), dot(g10, g10), dot(g11, g11)));
                  g00 *= norm.x;
                  g01 *= norm.y;
                  g10 *= norm.z;
                  g11 *= norm.w;
 
                  float n00 = dot(g00, float2(fx.x, fy.x));
                  float n10 = dot(g10, float2(fx.y, fy.y));
                  float n01 = dot(g01, float2(fx.z, fy.z));
                  float n11 = dot(g11, float2(fx.w, fy.w));
 
                  float2 fade_xy = fade(Pf.xy);
                  float2 n_x = lerp(float2(n00, n01), float2(n10, n11), fade_xy.x);
                  float n_xy = lerp(n_x.x, n_x.y, fade_xy.y);
                  return 2.3 * n_xy;
            }
  
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _CameraDepthTexture;
            sampler2D _MainTex;
            sampler2D _Specular;
            float4 _MainTex_ST;
            sampler2D _Refract;
            sampler2D _Reflect;
            float _Amount;
            float2 _Tiling;
            float _Speed;
            float _Height;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float maxH : TEXCOORD2;
                float3 woldPos : TEXCOORD3;

            };

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float2 uv = o.uv;
                float time1 = float(_Time.z * _Speed) / 10;
                float time2 = float(-_Time.y * _Speed) / 10;
                float noise = (PerlinNoise2D(float2(uv.x * _Tiling.x * 10 + time1 * 5, uv.y * _Tiling.y * 10 - time2 * 5) + 1.0f).x / 2);
                float h = 1 - noise;
                v.vertex.y += h * _Height * .5f ;
                float a = h > 0.0f;
                v.vertex.y -= sin(_Time * 10 * UNITY_PI) * 0.03f * a;
                v.vertex.y += sin(_Time * 10 * UNITY_PI) * 0.03f * (1 - a);
                
                o.maxH = .5f;
                o.woldPos = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 uv = i.screenPos / i.screenPos.w;
                fixed4 refractCol = tex2D(_Refract, float2(uv.x, uv.y));
                fixed4 reflectCol = tex2D(_Reflect, float2(uv.x, 1- uv.y));
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv).r;
                depth = Linear01Depth(depth);
                if (depth < .5)
                {
                    float a = 100;
                    float b = 1;
                    float x = uv.x;
                    float y= uv.y;

                    float angle = a*exp(-(x*x+y*y)/(b*b)) * _Time/ 50;
                    float u = cos(angle)*x + sin(angle)*y;
                    float v = -sin(angle)*x + cos(angle)*y;
                    refractCol = clamp(0,1,pow(tex2D(_Refract, float2(uv.x + u, uv.y)),20)) + clamp(0, 4 ,reflectCol/ 5.0);
                }
                
                float4 col = lerp(reflectCol, refractCol, _Amount);
                
                float val = lerp(0, 1, i.woldPos.y - i.maxH);
                val = clamp(0, 1, val + (val > .1f));
                float x = 0;
                float smoothing_val = .25f - pow(lerp(1, 0,abs(i.maxH - i.woldPos.y) ), 20);
                if (val >.08f && val < .1f)
                {
                    x = clamp(0, 1, (PerlinNoise2D(i.vertex.xy ) > -.1f) );
                }
                col = clamp(0,1, col + (val + x) * smoothing_val);
                col = clamp(0, 0.8f, col);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
