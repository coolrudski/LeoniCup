texture ScreenTexture;
texture bloodTexture;
float blurAmount = 0.002;

// Our sampler for the texture, which is just going to be pretty simple
sampler TextureSampler = sampler_state
{
    Texture = <ScreenTexture>;
};

sampler bloodSampler = sampler_state
{
    Texture = <bloodTexture>;
};

texture overlay;

sampler overlaySampler = sampler_state
{
    Texture = <overlay>;
};

// Helper for modifying the saturation of a color.
float4 AdjustSaturation(float4 color)
{
    // The constants 0.3, 0.59, and 0.11 are chosen because the
    // human eye is more sensitive to green light, and less to blue.
    float grey = dot(color, float3(0.3, 0.59, 0.11));

    return lerp(grey, color, 1);//1 being saturation
}

float2 overlay_pos = float2(0,0);
float overlay_amount = 0;
float blood_level = 0;
float4 PixelShaderFunction(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{
   float4 c = tex2D(TextureSampler, TextureCoordinate);    
 
 if(TextureCoordinate.x<1||TextureCoordinate.y<1){
	for(int i=-2; i<2; i++)   
		for(int x=-2; x<2; x++)	{
			c += tex2D(TextureSampler, TextureCoordinate+float2(blurAmount*x,blurAmount*i));
		}
	}

	c = c/25;  
   float amount = 0.05;
   float4 finalColor = AdjustSaturation(saturate((c - amount) / (1 - amount)));
   float4 tex_over = tex2D(overlaySampler, TextureCoordinate+overlay_pos);
   float4 blood_over = tex2D(bloodSampler, TextureCoordinate);
   return float4( finalColor.x*(1-overlay_amount) + tex_over.x*(overlay_amount) + blood_over.x*(blood_level), (finalColor.y*(1-overlay_amount) + tex_over.y*(overlay_amount)),(finalColor.z*(1-overlay_amount) + tex_over.z*(overlay_amount)),1);
}

technique Plain
{
    pass Pass1
    {		
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}


float blur_factor = 1;
float4 BlurPixelShaderFunction(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{
   float4 c = tex2D(TextureSampler, TextureCoordinate);    
   float blur_amount;

   blur_amount = (abs(TextureCoordinate.x-0.5)+abs(TextureCoordinate.y-0.5))/2;
   
   blur_amount = abs(blur_amount)/25*blur_factor;  

   if(TextureCoordinate.x<1||TextureCoordinate.y<1){
	for(int i=-2; i<2; i++)   
		for(int x=-2; x<2; x++)	{					
			  c += tex2D(TextureSampler, TextureCoordinate+float2(blur_amount*x,blur_amount*i));			
		}
	}

	c = c/25;  
   float amount = 0.05;
   float4 tex_over = tex2D(overlaySampler, TextureCoordinate+overlay_pos);
   float4 finalColor = AdjustSaturation(saturate((c - amount) / (1 - amount))); 
   float4 blood_over = tex2D(bloodSampler, TextureCoordinate);
   return float4( finalColor.x*(1-overlay_amount) + tex_over.x*(overlay_amount) + blood_over.x*(blood_level) , (finalColor.y*(1-overlay_amount) + tex_over.y*(overlay_amount)),(finalColor.z*(1-overlay_amount) + tex_over.z*(overlay_amount)),1);

}

technique Blur
{
    pass Pass1
    {		
        PixelShader = compile ps_2_0 BlurPixelShaderFunction();
    }
}





//Textures
Texture a;
Texture b;
sampler Texa = sampler_state
{
    Texture = <a>;
};
sampler Texb = sampler_state
{
    Texture = <b>;
};

float4 BloomFunction(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(Texa, TextureCoordinate);
	float4 d = tex2D(Texb, TextureCoordinate);
	
	float average = (c.r+c.g+c.b)/3;
	if(average>0.4)
		return c;
	return d;



	//float amount = 0.05;
	//return AdjustSaturation(saturate((c - amount) / (1 - amount)));
}

technique Bloom
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 BloomFunction();
	}
}

float3 TintColor;
float4 ov_tint;
bool use_ov_tint;
float pic_alpha = 0;
float4 DOF_Function(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{    
	float4 baseColor = tex2D(TextureSampler, TextureCoordinate);
	
	float final_alpha;

	if(pic_alpha<0)
		final_alpha = 0;	
	else
		final_alpha = pic_alpha;	

	if((baseColor.x+baseColor.y+baseColor.z)/3<0.1)
	{
	   baseColor = float4(0,0,0,0);
	}

	if(use_ov_tint)
		return float4((ov_tint.r*baseColor.r)/2, (ov_tint.g*baseColor.g)/2, (ov_tint.b*baseColor.b)/2, baseColor.a-final_alpha);
	else
		return float4((TintColor.r*baseColor.r)/2, (TintColor.g*baseColor.g)/2, (TintColor.b*baseColor.b)/2, 0);
   	
}


technique DepthOfField
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 DOF_Function();
	}
}



float4 OverallFunction(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{
	
	return ov_tint;	
}

technique Overall
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 OverallFunction();
	}
}



float4 Alpha_Function(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{    
	float4 baseColor = tex2D(TextureSampler, TextureCoordinate);
	return baseColor;
}

technique AlphaBlending
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 Alpha_Function();
	}
}