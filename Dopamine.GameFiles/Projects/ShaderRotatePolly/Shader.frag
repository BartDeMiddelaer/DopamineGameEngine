#ifdef GL_ES
precision mediump float;
#endif

// IP var
const float PI = 3.14159265359;

// Vars out of the c# code
uniform vec2 resulution;
uniform float time;

// rectShap
float rectShape(vec2 position, vec2 scale){
    scale = vec2(0.5) - scale * 0.5;
    vec2 shaper = vec2(step(scale.x, position.x), step(scale.y, position.y));
    shaper *= vec2(step(scale.x, 1.0 - position.x), step(scale.y, 1.0 - position.y));
    return shaper.x * shaper.y;
}

mat2 rotate(float angle){
    return mat2(cos(angle), -sin(angle), sin(angle), cos(angle));
}

void main()
{
    // Get middel of the screen cords
    vec2 pos = gl_FragCoord.xy / resulution;
    vec3 color = vec3(pos, 0.5);
    
    pos -= vec2(0.5);
    pos = rotate(cos(time)) * pos;
    pos += vec2(0.5);

    // make polygon object
    color += vec3(rectShape(pos,vec2(0.3)));

    // Rastorize
    gl_FragColor = vec4(color, time);
}

