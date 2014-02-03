// -------------------
// -- Key Variables --
// -------------------

float4x4 World;
float4x4 View;
float4x4 Projection;
float3 ViewVector = float3(1, 0, 0);
float3 cameraPos;
float3 LightPosition = float3(-100, 100, 0);
float contrast = 0.05;

float3 secondLightPosition = float3(0, 30, 0);

float tex_shift;

Texture Tex;
sampler TextureSampler = sampler_state { texture = <Tex> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

Texture Norm;
sampler NormalsSampler = sampler_state { texture = <Norm>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

Texture Plasma;
sampler PlasmaSampler = sampler_state { texture = <Plasma>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

bool useBump = true;
Texture Bump;
sampler BumpSampler = sampler_state { texture = <Bump>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

float4 Ambient = float4(0.2,0.2,0.2,1);
float4 Tint = float4(1,1,1,1);

float bloomFactor = 3;
bool bloom=true;
bool Colored=true;
float mode=0;

float energy=0;


float bright_area = 200;


float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);
}





//
struct VertexShaderInput
{
    float4 Position : POSITION0;	
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TexCoords : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float3 Position3D : TEXCOORD2;
};





VertexShaderOutput VertexShaderFunction( float4 inPos : POSITION0, float3 inNormal: NORMAL0, float2 inTexCoords : TEXCOORD0)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(inPos, World);
    float4 viewPosition = mul(worldPosition, View);
	float4x4 preWorldViewProjection = mul(World, View);

    output.Position = mul(viewPosition, Projection);
	output.TexCoords = inTexCoords;
	output.Normal = normalize(mul(inNormal, (float3x3)World));  
	output.Position3D = mul(inPos, World);
    return output;
}

float4 PixelNormalsFunction(VertexShaderOutput input) : COLOR0
{	
		float diffuseLightingFactor = DotProduct(LightPosition, input.Position3D, input.Normal);
		return input.Position3D.x;
		//return diffuseLightingFactor;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float3 curLightPosition = float3(cameraPos.x, cameraPos.y+20, cameraPos.z);

	//Texturing
    //input.TexCoords.y--;//This flips teh textures on the y-axis
	float4 baseColor = tex2D(TextureSampler, input.TexCoords);

	baseColor = baseColor*(1-energy)+tex2D(PlasmaSampler, float2(input.TexCoords.x+tex_shift, input.TexCoords.y))*energy;
    //2nd Texture
	float4 bump = tex2D(BumpSampler, input.TexCoords);
	bump = bump/3+0.7;
	

	//Diffusing Lighting Source
	float diffuseLightingFactor = DotProduct(curLightPosition, input.Position3D, input.Normal);
    diffuseLightingFactor = saturate(diffuseLightingFactor)/2;	
	if(bloom)
		diffuseLightingFactor*=(diffuseLightingFactor*bloomFactor);
	float4 difTexColor = baseColor*(diffuseLightingFactor+Ambient)*Tint*bump*(1+energy*2);					
	
	
	//Distance
	float dist = distance(cameraPos, input.Position3D);
	if(dist/bright_area>.9)
	{
		difTexColor *= .4;
	}
	else
		difTexColor *= 1.3-dist/bright_area;


	//Screen Effects
	float4 finalColor;	

	finalColor = difTexColor;	

	
	//Calculate negative y-value
	float yval = input.Position3D.y;
	float yfactor = 1;
	
	if(yval<0)
	{
		yfactor = 1+yval/300;
	}
	
	finalColor += float4(contrast, contrast, contrast, 0);
    return finalColor*yfactor;	
}


float4 PixelNoDiffuseFunction(VertexShaderOutput input) : COLOR0
{
	//Texturing
    input.TexCoords.y--;
	float4 baseColor = tex2D(TextureSampler, input.TexCoords);

	//Diffusing Lighting Source
	float diffuseLightingFactor = DotProduct(LightPosition, input.Position3D, input.Normal);
    diffuseLightingFactor = saturate(diffuseLightingFactor);	
	if(bloom)
		diffuseLightingFactor*=(diffuseLightingFactor*bloomFactor);
	float4 difTexColor = baseColor*(Ambient)*Tint;					
	
	//Specular
	/*float3 light = normalize(float3(1,0,0));
    float3 normal = normalize(input.Normal);
    float3 r = normalize(2 * dot(light, normal) * normal - light);
    float3 v = normalize(mul(normalize(ViewVector), World));

    float dotProduct = dot(r, v);
    float4 specular = max(pow(dotProduct, 1), 0)/2;
	*/
	//Screen Effects
	float4 finalColor=0;	
	
	if(mode==0){
		//Black&White Or Colored?			
		if(Colored==true){finalColor = difTexColor;}
		else{  
			float color = (difTexColor.x+difTexColor.y+difTexColor.z)/3;
			finalColor = float4(color,color,color, difTexColor.w);
		}
	}
	if(mode==1) 
		finalColor = diffuseLightingFactor;	
	if(mode==2)
		finalColor = float4(input.Normal.x,input.Normal.y, input.Normal.z, 1);
	if(mode==3){
		float color = (input.Normal.x + input.Normal.y + input.Normal.z)/3;
		finalColor = float4(color, color, color, 1);
	}
	if(mode==4){
	    float4 outputColor = baseColor;
		finalColor.r = (finalColor.r * 0.393) + (finalColor.g * 0.769) + (finalColor.b * 0.189);
		finalColor.g = (finalColor.r * 0.349) + (finalColor.g * 0.686) + (finalColor.b * 0.168);    
		finalColor.b = (finalColor.r * 0.272) + (finalColor.g * 0.534) + (finalColor.b * 0.131);
	}
	
    return finalColor;	
}




// -------------------
// -------------------
// -- SIMPLE
// -------------------
// -------------------
technique Simple
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

technique NoDiffuse
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelNoDiffuseFunction();
	}
}


/*******************/
/* DEPTH MAP       */
/*******************/
VertexShaderOutput DepthV( float4 inPos : POSITION0, float3 inNormal: NORMAL0, float2 inTexCoords : TEXCOORD0)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(inPos, World);
    float4 viewPosition = mul(worldPosition, View);
	float4x4 preWorldViewProjection = mul(World, View);

    output.Position = mul(viewPosition, Projection);
	output.TexCoords = inTexCoords;
	output.Normal = normalize(mul(inNormal, (float3x3)World));  
	output.Position3D = mul(inPos, World);
    return output;
}

float4 PixelS(VertexShaderOutput input) : COLOR0
{	

		float dist = distance(cameraPos, input.Position3D);		
		return 1-dist/200;
}

technique DepthMapShader
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 DepthV();
		PixelShader = compile ps_2_0 PixelS();
	}
}

float4 alphaPS(VertexShaderOutput input) : COLOR0
{

    float3 curLightPosition = cameraPos;			

	//Texturing
    //input.TexCoords.y--;
	float4 baseColor = tex2D(TextureSampler, input.TexCoords);

	float diffuseLightingFactor = DotProduct(curLightPosition, input.Position3D, input.Normal);
    diffuseLightingFactor = saturate(diffuseLightingFactor)/2;	
	if(bloom)
		diffuseLightingFactor*=(diffuseLightingFactor*bloomFactor);
	float4 difTexColor = baseColor*(diffuseLightingFactor+Ambient)*Tint;

	//clip(baseColor.w - 0.7843f);		
	//clip(baseColor.w - 0.6843f);	
		
	//Distance
	float dist = distance(cameraPos, input.Position3D);
	if(dist/bright_area>.8)
	{
		baseColor *= .5;
	}
	else
		baseColor *= 1.3-dist/bright_area;

	baseColor.w -= 0.2f;

	return baseColor;
}

technique Alpha
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 alphaPS();
	}
}



/********************************************/

struct VertexShaderInputColor
{
    float4 Position : POSITION0;	
};

struct VertexShaderOutputColors
{
    float4 Position : POSITION0;	
	float3 Normal : TEXCOORD1;
	float3 Position3D : TEXCOORD2;
};

VertexShaderOutputColors VertexShaderFunctionColor( float4 inPos : POSITION0, float3 inNormal: NORMAL0)
{
    VertexShaderOutputColors output;

    float4 worldPosition = mul(inPos, World);
    float4 viewPosition = mul(worldPosition, View);
	float4x4 preWorldViewProjection = mul(World, View);

    output.Position = mul(viewPosition, Projection);	
	output.Normal = normalize(mul(inNormal, (float3x3)World));  
	output.Position3D = mul(inPos, World);
    return output;
}


float4 ColorPS(VertexShaderOutputColors input) : COLOR0
{

    float3 curLightPosition = cameraPos;			


	float4 baseColor = float4(0.7,0.7,0.7,1);

	float diffuseLightingFactor = DotProduct(curLightPosition, input.Position3D, input.Normal);
    diffuseLightingFactor = saturate(diffuseLightingFactor)/2;	
	if(bloom)
		diffuseLightingFactor*=(diffuseLightingFactor*bloomFactor);
	float4 difTexColor = baseColor*(diffuseLightingFactor+Ambient)*Tint;

	clip(baseColor.w - 0.7843f);		
	
	//Distance
	float dist = distance(cameraPos, input.Position3D);
	baseColor *= 1.3-dist/bright_area;


	return baseColor;
}

technique ColorOnly
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunctionColor();
		PixelShader = compile ps_2_0 ColorPS();
	}
}



float4 EnergyPS(VertexShaderOutput input) : COLOR0
{    
    return float4(energy, energy, energy, 1);	
}


technique EnergyOnly
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 EnergyPS();
	}
}