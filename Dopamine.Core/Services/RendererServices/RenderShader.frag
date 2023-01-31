#ifdef GL_ES
precision mediump float;
#endif

// Vars out of the c# code
uniform vec2 resulution;
uniform sampler2D renderTexture;

void main()
{
    vec4 texel = texture2D(renderTexture, gl_TexCoord[0].st);
    gl_FragColor = texel;
}

