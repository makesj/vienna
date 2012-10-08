#version 130

precision highp float;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;

in vec3 in_position;
in vec3 in_normal;
in vec2 in_texcoord;

out vec3 normal;
out vec2 texcoord;

void main(void)
{
	texcoord = in_texcoord;

	//works only for orthogonal modelview
	normal = (view_matrix * vec4(in_normal, 0)).xyz;
  
	gl_Position =  projection_matrix * view_matrix * vec4(in_position, 1);
}
