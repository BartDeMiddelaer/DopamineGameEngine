#ifdef GL_ES
precision mediump float;
#endif

// Vars out of the c# code
uniform vec2 resulution;
uniform float time;

void main()
{
    // Get middel of the screen cords
    vec2 pos = gl_FragCoord.xy / resulution;
    vec3 color = vec3(pos, 0.5);
    
    // the worp
    color += sin(pos.x * cos(time / 5.0) * 60.0) + sin(pos.y * cos(time / 5.0) * 60.0);
    color += cos(pos.x * sin(time / 5.0) * 10.0) + cos(pos.y * sin(time / 5.0) * 10.0);

    // fheadering
    color *= cos(time / 1.0) * 0.2;


    // Rastorize
    gl_FragColor = vec4(color, 1.0);
}

