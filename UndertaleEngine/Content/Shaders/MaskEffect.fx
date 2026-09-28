sampler2D MainTex : register(s0);
sampler2D MaskTex : register(s1);

float4 MainPS(float4 color : COLOR0, float2 uv : TEXCOORD0) : COLOR0
{
    float4 src = tex2D(MainTex, uv);   // gameplay pixel
    float mask = tex2D(MaskTex, uv).a; // alpha mask (0 = hidden, 1 = visible)
    return float4(src.rgb * mask, src.a * mask);
}

technique Basic
{
    pass P0
    {
        PixelShader = compile ps_4_0_level_9_1 MainPS();
    }
}
