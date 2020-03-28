// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OpenVerticalTransition"
{
    Properties
    {
        _OffsetTexture("Offset Texture", 2D) = "white" {}
        _TransitionTime("Transition Time", float) = 100
        _TimeElapsed("Time Elapsed", float) = 0
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
            uniform float _TransitionTime;
            uniform float _TimeElapsed;

            struct vertexInput
            {
                float4 vertex: POSITION;
            };

            struct vertexOutput
            {
                float4 pos: SV_POSITION;
                float4 onScreenPosition: TEXCOORD0;
            };

            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;

                // Get position in clip space
                output.pos = UnityObjectToClipPos(input.vertex);

                // Get position on screen
                output.onScreenPosition = ComputeGrabScreenPos(output.pos);

                return output;
            }

            float4 frag(vertexOutput input) : COLOR
            {
                // REAL BACKUP CODE
                if (_TimeElapsed > _TransitionTime)
                {
                    return tex2Dproj(_BackgroundTexture, input.onScreenPosition);
                }
                else
                {
                    float timePassedAsRatio = _TimeElapsed / _TransitionTime;
                    float textureSample = tex2Dproj(_OffsetTexture, input.onScreenPosition).x;

                    if (timePassedAsRatio > 1 - textureSample)
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
