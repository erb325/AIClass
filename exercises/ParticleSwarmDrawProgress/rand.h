#include <stdlib.h>
#include <time.h>

#define RANDINIT()	srand(time(NULL))

#define RANDOM()	((float)rand() / (float)RAND_MAX)

#define RANDMAX(x)	(int)((float)(x)*rand()/(RAND_MAX+1.0))

