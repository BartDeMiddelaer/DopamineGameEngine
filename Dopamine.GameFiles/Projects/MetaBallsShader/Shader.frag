#ifdef GL_ES
precision mediump float;
#endif

// max amount of balls to alokate memory on the gpu
const int maxIndex = 100;

// vars to controle in the c# file
uniform int ballCount;
uniform vec2 resulution;
uniform float xMetaBallPositions[maxIndex];
uniform float yMetaBallPositions[maxIndex];
uniform float metaBallRadius[maxIndex];
uniform float modIntesety;
uniform float hsvMultiplayer;
uniform float colorMultiplayer;
uniform float time;

// color convertion
vec3 rgb2hsv(vec3 c)
{
    vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
    vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
// ----------------
void main(){

	// Give alle the pixelsColor
	vec2 pos = gl_FragCoord.xy / resulution;
    vec3 color = vec3(pos, 0.5);

	// Some off the distens between all the balls
	float colorSom = 0.0;

	// loop true alle the balls to get the colosSom
	for (int i = 0; i < ballCount; i++){
		
		// position of the pixel dat will be rendert
		vec2 pixepPos = vec2(gl_FragCoord.x,gl_FragCoord.y);

		// the center off the ball selectid in the arry
		// resulution.y -yMetaBallPositions[i] is to flip the Y coords 
		// GLSL renders from bottom-left
		vec2 ballPos = vec2(xMetaBallPositions[i], resulution.y -yMetaBallPositions[i]);

		// get the distens between the 2 points
		float distensBetween = distance(pixepPos, ballPos);

		// eqwasion of the algorytme
		colorSom += metaBallRadius[i] / distensBetween;
	
	    // color to set the HUE to convertid to HSV
		vec3 hsv = rgb2hsv(vec3(0.8,0.8 ,0.8 ));

		// chap the algorytme eqwasion to not overshoot over 1.0
		float laessDanOne = colorSom / modIntesety > 1.0 ? 1.0 : colorSom / modIntesety;

		// set hue
		hsv.x = laessDanOne;
		hsv.y = laessDanOne;
		hsv.z = laessDanOne;

		// convert back to RBG
		color += hsv2rgb(hsv * hsvMultiplayer);

		// Softens the serounding pixels (bleds)
 		color *= colorMultiplayer;
	}


    // the number after len adds color to the
	gl_FragColor = vec4(color, time);

}