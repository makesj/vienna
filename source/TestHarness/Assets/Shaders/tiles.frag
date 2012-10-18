#version 130

precision highp float;

uniform sampler2D tex;

in vec2 texcoord;

out vec4 outcolor;

void main(void)
{
	vec4 result = vec4(texture2D(tex,texcoord).xyz, texture2D(tex,texcoord).w);
	outcolor = result;
}