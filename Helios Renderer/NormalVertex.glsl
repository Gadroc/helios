uniform mat4 uProjection;
attribute vec3 vPosition;
void main() {
   vec4 position = vec4(vPosition.xyz, 1.);
   gl_Position = uProjection * position;
}