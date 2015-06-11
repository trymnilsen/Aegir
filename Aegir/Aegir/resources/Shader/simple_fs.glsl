#version 130
flat in vec3 color;
smooth in vec3 normal_smooth;
smooth in vec3 v;
smooth in vec3 l;
out vec4 out_color;

void main() {
    vec3 h = normalize(v+l);
    vec3 n = normalize(normal_smooth);
    float diff = max(0.1f, dot(n, l));
    float spec = pow(max(0.0f, dot(n, h)), 128.0f);
    out_color = diff*vec4(color, 1.0f) + vec4(spec);
}