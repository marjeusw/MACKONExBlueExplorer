Shader "Custom/PortalDepthMask"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue" = "Geometry-1" // Renders BEFORE the falling object
        }

        Pass
        {
            ZWrite On       // Writes the portal's depth into the depth buffer
            ZTest LEqual    // Respects walls/objects in front of the portal
            ColorMask 0     // Draws zero color (completely invisible!)
        }
    }
}