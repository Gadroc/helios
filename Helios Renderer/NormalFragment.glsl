#ifdef GL_ES
precision highp float;
#endif
uniform vec4 vColor;
void main(void)
{
   gl_FragColor = vColor;
}