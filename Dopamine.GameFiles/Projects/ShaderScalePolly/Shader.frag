#ifdef GL_ES
precision mediump float;
#endif

// IP var
const float PI = 3.14159265359;

// Vars out of the c# code
uniform vec2 resulution;
uniform float time;

mat2 scale(vec2 scale){
    return mat2(scale.x, 0.0, 0.0, scale.y);
}

// make a Circle shape
float circleShape(vec2 position, float radius){
    return step(radius, length(position - vec2(0.5)));
}

void main()
{
    // Get middel of the screen cords
    vec2 pos = gl_FragCoord.xy / resulution;
    vec3 color = vec3(0);

    // Add scale fuction
    pos -= vec2(0.5);
    pos = scale(vec2(sin(time) + 2.0)) * pos;
    pos += vec2(0.5);

    // make polygon object
    float circle = circleShape(pos,0.35);

    // Draw polygon object
    color = vec3(circle);

    // Rastorize
    gl_FragColor = vec4(color, time);
}

