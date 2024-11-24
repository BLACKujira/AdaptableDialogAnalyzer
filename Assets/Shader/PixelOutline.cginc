#ifndef OUTLINE_INCLUDED
#define OUTLINE_INCLUDED
// Search surrounding pixels up to search width and return largest alpha value found.
// Based on current pixel and largest neighbour alpha, should current pixel be an outline pixel?
// Copy from https://www.reddit.com/r/Unity3D/comments/124mtz8/procedural_2d_outline_shader_tutorial_w_code_nodes/?rdt=35054

bool IsOutline(float currentAlpha, float largestNeighbourAlpha)
{
    if (currentAlpha < 0.5 && largestNeighbourAlpha >= 0.5)
    {
        return true;
    }
    return false;
}

void GetNeigbourWithLargestAlpha_float(UnityTexture2D baseTexture, UnitySamplerState sampler_baseTexture, float2 baseTextureUV, float2 baseTextureTexelSize, float currentAlpha, int searchWidth, float outlineWidth, out float alpha)
{
    alpha = currentAlpha;
    float2 texelSize = (outlineWidth / searchWidth) * baseTextureTexelSize.xy;

    for (int x = -searchWidth; x <= searchWidth; x++)
    {
        for (int y = -searchWidth; y <= searchWidth; y++)
        {
            if (x == 0 && y == 0)
                continue; // Ignore this pixel.

            float2 offset = float2(x, y) * texelSize;
            float4 neighbour = baseTexture.Sample(sampler_baseTexture, baseTextureUV + offset);

            alpha = max(alpha, neighbour.a);
        }
    }
    if (IsOutline(currentAlpha,alpha))
        alpha = 1;
    else
        alpha = 0;
}

#endif // OUTLINE_INCLUDED