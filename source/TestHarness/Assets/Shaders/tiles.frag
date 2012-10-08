#version 130

precision highp float;

const vec3 ambient = vec3(0.1, 0.1, 0.1);
const vec3 lightVecNormalized = normalize(vec3(0.5, 0.5, 2.0));
const vec3 lightColor = vec3(0.1, 0.1, 0.9);

uniform sampler2D tex;

in vec2 texcoord;
in vec3 normal;

out vec4 outcolor;

void main(void)
{
	vec4 result = vec4(texture2D(tex,texcoord).xyz, texture2D(tex,texcoord).w);
	outcolor = result;
}