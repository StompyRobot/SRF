Shader "SRF/Unlit/UnlitAlphaWithFadeCulled" 
{
	Properties 
	{
		_Color ("Color Tint", Color) = (1,1,1,1)	
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white"
	}

	Category 
	{
		Lighting Off
		ZWrite Off
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {"RenderType" = "Transparent" "Queue"="Transparent"}

		SubShader 
		{

           		Pass 
           		{
            			SetTexture [_MainTex] 
            			{
					ConstantColor [_Color]
               				Combine Texture * constant
				}
			}
		}
	}
}