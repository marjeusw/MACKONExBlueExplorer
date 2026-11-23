Shader "Custom/PortalStencilMask"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass
        {
            ZWrite Off
            ZTest LEqual
            ColorMask 0

            Stencil
            {
                Ref 2
                Comp Always
                Pass Replace
            }
        }
    }
}