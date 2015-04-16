Shader "SRF/Unlit Color" {

	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)	
	}

	Category 
	{

		Lighting Off
		ZWrite On
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha

		SubShader 
		{

			Pass 
			{
				Color [_Color]
			}
		}

	}
}