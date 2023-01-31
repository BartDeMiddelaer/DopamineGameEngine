#ifdef GL_ES
precision mediump float;
#endif

uniform vec2 resulution;
uniform float time;

const int AMOUNT = 12;

void main(){
	vec2 pos = 20.0 * (gl_FragCoord.xy - resulution / 2.0) / min(resulution.y, resulution.x);

	float len;

	for (int i = 0; i < AMOUNT; i++){
		len = length(vec2(pos.x, pos.y));

		pos.x = pos.x - cos(pos.y + sin(len)) + cos(time / 9.0);
		pos.y = pos.y + sin(pos.x + cos(len)) + sin(time / 12.0);
	}

    // the number after len adds color to the
	gl_FragColor = vec4(cos(len * 1.5), cos(len * 1.0), cos(len * 1.0), time);
}