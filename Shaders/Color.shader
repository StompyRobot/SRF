// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Custom/Color" {

	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)	
	}

	Category 
	{
		Lighting Off
		ZWrite On
				//ZWrite On  // uncomment if you have problems like the sprite disappear in some rotations.
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
				//AlphaTest Greater 0.001  // uncomment if you have problems like the sprites or 3d text have white quads instead of alpha pixels.
		//Tags {Queue=Transparent}

		SubShader 
		{

			Pass 
			{
				Color [_Color]
			}
		}
	}
}