#ifdef GL_ES
precision mediump float;
#endif

// Templayt
uniform float color_shift;

void main()
{
    // Templayt
    gl_FragColor = vec4(color_shift, 0.0, 1.0, 1.0);
}

