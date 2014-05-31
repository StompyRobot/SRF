﻿Shader "SRF/Vertex Color" {
	Properties {

	}

	SubShader {

		Tags { "RenderType"="Opaque" }

		Pass {
	
			Lighting Off
			ColorMaterial AmbientAndDiffuse
			ZWrite On
			Cull Back
			//Blend One One

			Color [primary]

		}

	}
}