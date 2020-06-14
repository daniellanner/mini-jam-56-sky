#define PI 3.14159265
#define HALF_PI 1.57079632
#define TWO_PI 6.28318530

float GreaterThan(float a, float b)
{
	return (sign(a - b) + 1.0) / 2.0;
}

float LessThan(float a, float b)
{
	return 1.0 - GreaterThan(a, b);
}

float Ease(float x, float a)
{
	return pow(x, a) / (pow(x, a) + pow(1.0 - x, a));
}

float SinBellEase(float x)
{
	return (sin(-HALF_PI + x * TWO_PI) + 1) * .5;
}

float SinBellEaseWithOffset(float x, float freq, float off)
{
	return (sin(-HALF_PI + x * freq * TWO_PI + off) + 1) * .5;
}

float NormalizedInRange(float value, float range_min, float range_max)
{
	return saturate((value - range_min) / (range_max - range_min));
}