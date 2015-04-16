Shader "SRF/Empty" 
{

	SubShader
	{ 
		
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask 0

		Pass { }

	}

}