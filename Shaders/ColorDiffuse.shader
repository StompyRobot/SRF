
Shader "SRF/Color" {
Properties {
	_Color ("Color", Color) = (1,1,1,1)
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd

half4 _Color;

struct Input {
	bool empty;
};

void surf (Input IN, inout SurfaceOutput o) {
	o.Albedo = _Color.rgb;
	o.Alpha = _Color.a;
}
ENDCG
}

}
