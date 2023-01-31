#ifdef GL_ES
precision mediump float;
#endif

uniform vec2 resulution;
uniform vec2 castRaysFrom;

// the index must be set to the same value as AstroidOridginals in the AstroidLister class x y = x y, Z = size
uniform vec3 astroidOridginals[5];

int intersectAstroidCount(vec2 astroidPos, float radius, vec2 point1, vec2 point2)
{
    float dx, dy, A, B, C, det, t;

    dx = point2.x - point1.x;
    dy = point2.y - point1.y;

    A = dx * dx + dy * dy;
    B = 2 * (dx * (point1.x - astroidPos.x) + dy * (point1.y - astroidPos.y));
    C = (point1.x - astroidPos.x) * (point1.x - astroidPos.x) +
        (point1.y - astroidPos.y) * (point1.y - astroidPos.y) -
        radius * radius;

    det = B * B - 4 * A * C;
    if ((A <= 0.0000001) || (det < 0)) return 0;
    else if (det == 0) return 1;
    else return 2;
}
vec4 calculateRayCasting() {
    vec4 color;
    bool havesIntersection = false;
    bool isShadow = false;
    vec2 pixepPos = vec2(gl_FragCoord.x, gl_FragCoord.y);
    vec2 raysFrom = vec2(castRaysFrom.x, resulution.y -castRaysFrom.y);

	for (int i = 0; i < astroidOridginals.length(); i++){

        vec2 astroidPos = vec2(astroidOridginals[i].x, resulution.y -astroidOridginals[i].y);
        float astroidRadius = astroidOridginals[i].z / 2;
        int countOfIntersection = intersectAstroidCount(astroidPos, astroidRadius, pixepPos, raysFrom);

        float distensBetweenRayCasterAndAstroid = distance(astroidPos, raysFrom);
        float distensBetweenRayCasterAndPixel = distance(pixepPos, raysFrom);

        bool pixelInAstroid = distance(pixepPos, astroidPos) > astroidRadius;

        if(countOfIntersection == 2 && distensBetweenRayCasterAndAstroid < distensBetweenRayCasterAndPixel && pixelInAstroid)
            isShadow = true;
	}

    float distensBetweenRayCasterAndPixel = distance(pixepPos, raysFrom) / 3000;
    vec3 colosShade = vec3(0.698, 0.8549, 0.902);

    color += isShadow 
    ? vec4(colosShade, 0.22)
    : vec4(colosShade, 1.0 - distensBetweenRayCasterAndPixel);
    
    return color;
}
void main(){

    vec4 color;
    color += calculateRayCasting();
	gl_FragColor = color / 2.5;
}