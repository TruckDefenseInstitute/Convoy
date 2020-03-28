Shader "Custom/PausePanelShader"
{
    Properties
    {
        _BarsTexture("Bars Texture", 2D) = "white" {}
        _BarsUVOffset("Bars UV Offset", Vector) = (0, 0, 0, 0)
        _ColorTint("Color Tint", Color) = (0, 0, 0, 0)
        _TintDegree("Tint Degree", Range(0, 1)) = 0.1
        _Brightness("Brightness",  Range(1, 2)) = 1.5
        _UnscaledTime("UnscaledTime", float) = 0
        _RGBOffset("RGB Offset", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        GrabPass
        {
            "_BackgroundTexture"
        }

        Pass
        {
        CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _BackgroundTexture;

            sampler2D _BarsTexture;
            float4 _BarsUVOffset;
            
            float4 _ColorTint;
            float _TintDegree;
            float _Brightness;

            float _UnscaledTime;

            float4 _RGBOffset;

            struct vertexInput
            {
                float4 vertex: POSITION;
            };

            struct fragmentInput
            {
                float4 pos: SV_POSITION;
                float4 onScreenPosition: TEXCOORD0;
            };

            fragmentInput vert(vertexInput input)
            {
                fragmentInput o;

                // Get position in clip space
                o.pos = UnityObjectToClipPos(input.vertex);

                // Get position on screen
                o.onScreenPosition = ComputeGrabScreenPos(o.pos);

                return o;
            }
        
			float random(float2 p)
			{
				return frac(sin(dot(p.xy, float2(_UnscaledTime,65.115))) * 2773.8856);
			}

            float4 frag(fragmentInput input) : COLOR
            {
                // Chromatic Abberation
                float red   = tex2Dproj(_BackgroundTexture, float4(input.onScreenPosition.x + _RGBOffset.x, input.onScreenPosition.y, input.onScreenPosition.z, input.onScreenPosition.w)).r;
                float green = tex2Dproj(_BackgroundTexture, float4(input.onScreenPosition.x + _RGBOffset.y, input.onScreenPosition.y, input.onScreenPosition.z, input.onScreenPosition.w)).g;
                float blue  = tex2Dproj(_BackgroundTexture, float4(input.onScreenPosition.x + _RGBOffset.z, input.onScreenPosition.y, input.onScreenPosition.z, input.onScreenPosition.w)).b;

                float4 postAbberationColour = float4(red, green, blue, 1.0);

                // Scrolling Horizontal Bars
                float4 barSamplePosition = input.onScreenPosition;

                barSamplePosition.x += _BarsUVOffset.x;
                barSamplePosition.y += _BarsUVOffset.y;

                // Noise
                float2 p;
                p.x = input.onScreenPosition.x;
                p.y = input.onScreenPosition.y;
                float randfloat = random(p);
                fixed4 grain = fixed4(randfloat, randfloat, randfloat, 1.0);

                return lerp(postAbberationColour, _ColorTint, _TintDegree)
                       * tex2Dproj(_BarsTexture, barSamplePosition)
                       * lerp(1, grain, 0.15)
                       * _Brightness;
            }

        ENDCG
        }
    }
}
