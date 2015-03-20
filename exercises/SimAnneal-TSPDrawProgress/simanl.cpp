//Ember Baker
#include <stdio.h>
#include <string.h>
#include <math.h>
#include <iostream>
#include <fstream>
#include <string>
using namespace std;

#include "rand.h"
#include "glut.h"

#define MAX_CITIES	50
#define CITY_LIMIT	500
#define ALPHA 		((double)0.9999)
#define NUM_ITERATIONS	100
#define INITIAL_TEMP	((double)200.0)

typedef struct {
	int x, y;
} city_t;

typedef struct {
	city_t cities[MAX_CITIES];
	double tour_length;
} solution_t;


solution_t curSolution;


#define SQR(x)	(x*x)
#define ABS(x)	( ((x) > 0 ) ? (x) : ((x)*-1) )

double euclidean_distance(int x1, int y1, int x2, int y2)
{
	return sqrt(double(SQR(ABS(x1 - x2)) + SQR(ABS(y1 - y2))));
}


void compute_tour(solution_t *sol)
{
	int i;
	double tour_length = (double)0.0;

	for (i = 0; i < MAX_CITIES - 1; i++) {

		tour_length += euclidean_distance(
			sol->cities[i].x, sol->cities[i].y,
			sol->cities[i + 1].x, sol->cities[i + 1].y);

	}

	tour_length += euclidean_distance(
		sol->cities[MAX_CITIES - 1].x,
		sol->cities[MAX_CITIES - 1].y,
		sol->cities[0].x, sol->cities[0].y);

	sol->tour_length = tour_length;

	return;
}


void perturb_tour(solution_t *sol)
{
	int p1, p2, x, y;

	do {

		p1 = RANDMAX(MAX_CITIES);
		p2 = RANDMAX(MAX_CITIES);

	} while (p1 == p2);

	x = sol->cities[p1].x;
	y = sol->cities[p1].y;

	sol->cities[p1].x = sol->cities[p2].x;
	sol->cities[p1].y = sol->cities[p2].y;
	sol->cities[p2].x = x;
	sol->cities[p2].y = y;

	return;
}


int simulated_annealing(void)
{
	double temperature = INITIAL_TEMP, delta_e;
	solution_t tempSolution;
	int iteration;

	while (temperature > 0.0001) {

		/* Copy the current solution to a temp */
		memcpy((char *)&tempSolution, (char *)&curSolution, sizeof(solution_t));

		for (iteration = 0; iteration < NUM_ITERATIONS; iteration++) {

			perturb_tour(&tempSolution);
			compute_tour(&tempSolution);

			delta_e = tempSolution.tour_length - curSolution.tour_length;

			if (delta_e < 0.0) {

				memcpy((char *)&curSolution,
					(char *)&tempSolution, sizeof(solution_t));

			}
			else {

				if (exp((-delta_e / temperature)) > RANDOM()) {

					memcpy((char *)&curSolution,
						(char *)&tempSolution, sizeof(solution_t));

				}

			}

		}

		temperature *= ALPHA;

	}

	return 0;
}


void initWorld(void)
{
	int i;
	
	for (i = 0; i < MAX_CITIES; i++) {
		curSolution.cities[i].x = RANDMAX(CITY_LIMIT);
		curSolution.cities[i].y = RANDMAX(CITY_LIMIT);
	}
	
	compute_tour(&curSolution);

	return;
}

#include "graphics.h"

int main(int argc, char** argv)
{
	//RANDINIT();
	initWorld();

	//simulated_annealing();
	//cout << "Total tour length = " << curSolution.tour_length << endl;

	start_graphics_loop(argc, argv);

	return 0;
}

