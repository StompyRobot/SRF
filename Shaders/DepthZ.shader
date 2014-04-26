Shader "Unlit/Transparent/Depth Z" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}
 
SubShader {
    Tags {"RenderType"="Transparent" "Queue"="Transparent+1"}
    // Render into depth buffer only

    // Render normally
    Pass {
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        //ColorMask RGB
        Material {
            Diffuse [_Color]
            Ambient [_Color]
        }
        Lighting Off
        SetTexture [_MainTex] {
            ConstantColor [_Color]
            Combine Texture * constant
        } 
    }
}
}