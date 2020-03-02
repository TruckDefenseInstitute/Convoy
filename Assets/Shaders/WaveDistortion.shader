Shader "Custom/WaveDistortion"
{
    Properties
    {
        _Strength("Distort Strength", Range(0, 1)) = 0.15
        _Noise("Noise Texture", 2D) = "white" {}
        _Speed("Distort Speed", float) = 100
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent+1000"  // Ensures it renders after particle effects
        }

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
            sampler2D _StrengthFilter;
            sampler2D _Noise;
            float _Strength;
            float _Speed;

            struct vertexInput
            {
                float4 vertex: POSITION;
                float4 texCoord: TEXCOORD0;
            };

            struct vertexOutput
            {
                float4 pos: SV_POSITION;
                float4 grabPos: TEXCOORD0;
            };

            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;
                
                output.pos = UnityObjectToClipPos(input.vertex);
                output.grabPos = ComputeGrabScreenPos(output.pos);

                float noise = tex2Dlod(_Noise, input.texCoord).rgb;

                output.grabPos.x += cos(noise * _Time.x * _Speed) * _Strength;
                output.grabPos.y += sin(noise * _Time.x * _Speed) * _Strength;
                
                return output;
            }

            float4 frag(vertexOutput input) : COLOR
            {
                return tex2Dproj(_BackgroundTexture, input.grabPos);
            }

        ENDCG
        }

    }
}
