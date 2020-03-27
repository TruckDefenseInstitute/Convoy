// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OpenVerticalTransition"
{
    Properties
    {
        _OffsetTexture("Offset Texture", 2D) = "white" {}
        _TransitionTime("Transition Time", float) = 100
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

            sampler2D _OffsetTexture;
            float _TransitionTime;

            struct vertexInput
            {
                float4 vertex: POSITION;
            };

            struct vertexOutput
            {
                float4 pos: SV_POSITION;
                float4 onScreenPosition: TEXCOORD0;
                float time: TEXCOORD1;
            };

            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;

                // Get position in clip space
                output.pos = UnityObjectToClipPos(input.vertex);

                // Get position on screen
                output.onScreenPosition = ComputeGrabScreenPos(output.pos);

                output.time = _Time.y;

                return output;
            }

            float4 frag(vertexOutput input) : COLOR
            {
                if (input.time > _TransitionTime)
                {
                    return tex2Dproj(_BackgroundTexture, input.onScreenPosition);
                }
                else
                {
                    float timePassedAsRatio = input.time / _TransitionTime;
                    float yeet2 = tex2Dproj(_OffsetTexture, input.onScreenPosition).x;

                    if (timePassedAsRatio > 1 - yeet2)
                    {
                        return tex2Dproj(_BackgroundTexture, input.onScreenPosition);
                    }
                    else
                    {
                        return timePassedAsRatio * tex2Dproj(_BackgroundTexture, input.onScreenPosition);
                    }
                }
            }

            ENDCG
        }
    }
}
