#ifdef GL_ES
precision mediump float;
#endif

// IP var
const float PI = 3.14159265359;

// Vars out of the c# code
uniform vec2 resulution;
uniform float time;

void main()
{
    // 6.0 is the zoom fuction
    vec2 pos = 6.0 * gl_FragCoord.xy / resulution;

    for (int n = 1; n < 20; n++){
        float i = float(n);
        pos += vec2(0.7 / i * sin(i * pos.y + time + 0.3 * i) + 0.8, 0.4 / i * sin(pos.x + time + 0.3 * i) + 1.6);
    }

    // dont love this  
    //pos *= vec2(0.7 / sin(pos.y + time + 0.3) + 0.8, 0.4 / sin(pos.x + time + 0.3) + 1.6);

    vec3 color = vec3(0.5 * sin(pos.x) + 0.5, 0.5 * sin(pos.y) + 0.5, sin(pos.x + pos.y));

    gl_FragColor = vec4(color, time);
}

