
// Templayt
uniform float color_shift;
uniform vec2 resulution;

void main()
{
    vec2 pos = gl_FragCoord.xy / resulution;
    vec3 color = vec3(pos, 0.5);

    // Templayt
    gl_FragColor = vec4(color, color_shift);

}

