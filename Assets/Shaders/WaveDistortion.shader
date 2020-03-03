Shader "Custom/WaveDistortion"
{
    Properties
    {
        _Noise("Noise Texture", 2D) = "white" {}
        _Strength("Distort Strength", Range(0, 1)) = 0.15
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
            sampler2D _Noise;
            float _Strength;
            float _Speed;

            sampler2D _CameraDepthNormalsTexture;

            struct vertexInput
            {
                float4 vertex: POSITION;    // Position of vertex in the world
                float4 texCoord: TEXCOORD0; // Texture coordinates at the vertex
                float3 normal: NORMAL;      // Normal vector at that point
            };

            struct vertexOutput
            {
                float4 pos: SV_POSITION;                  // Position in clip space
                float4 preDistortionPosition: TEXCOORD0;  // Actual position on-screen
                float4 postDistortionPosition: TEXCOORD1; // Position on-screen to actually grab from 
                float4 depth: DEPTH;
                float3 normal: NORMAL;
                float3 viewDir: TEXCOORD2;
            };

            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;

                // Get position in clip space
                output.pos = UnityObjectToClipPos(input.vertex);

                // Get position on screen
                output.preDistortionPosition = ComputeGrabScreenPos(output.pos);

                // Get position on screen post distortion
                output.postDistortionPosition = output.preDistortionPosition;
                float noise = tex2Dlod(_Noise, input.texCoord).rgb;
                output.postDistortionPosition.x += cos(noise * _Time.x * _Speed) * _Strength;
                output.postDistortionPosition.y += sin(noise * _Time.x * _Speed) * _Strength;

                // Get depth
                output.depth = UnityObjectToViewPos(input.vertex).z * _ProjectionParams.w;

                // Get normal
                output.normal = UnityObjectToWorldNormal(input.normal);

                // Get view direction
                output.viewDir = normalize(UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, input.vertex)));

                return output;
            }

            float4 frag(vertexOutput input) : COLOR
            {
                // Re-obtain the distortion in x and y directions
                float xDisplacement = input.postDistortionPosition.x - input.preDistortionPosition.x;
                float yDisplacement = input.postDistortionPosition.y - input.preDistortionPosition.y;

                // Attenuation factor to simulate tapering towards the rim
                float rim = abs(dot(input.normal, normalize(input.viewDir)));

                float4 fragmentToGrab = input.preDistortionPosition;
                fragmentToGrab.x += xDisplacement * rim;
                fragmentToGrab.y += yDisplacement * rim;

                // Grab the fragment from the background texture
                return tex2Dproj(_BackgroundTexture, fragmentToGrab);
            }

        ENDCG
        }

    }
}
