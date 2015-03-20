// This version uses tournament selection and elitism

#include<stdlib.h>
#include <stdio.h>
#include <math.h>
#include <time.h>

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
using namespace std;

#include "rand.h"
#include "glut.h"

#define CROSSOVER_PROB		0.9
#define MUTATION_PROB		0.3
#define NUM_CITIES	50
#define CITY_LIMIT	500       // integer bounds on x and y coordinates

const int POPULATION_SIZE = 256*256 * 2;      // population size
const int GENERATIONS = 200;          // generations
const int TRACE_OFF=0;  
const int TRACE_ON=1; 

struct city_t{
  int x, y;
};

struct solution_t{
 city_t cities[NUM_CITIES];  
  double fitness;
} ;

struct solution{
  int order[NUM_CITIES];  // a permutation of 1 -- N
  double fitness;
} ;

ostream& operator<<(ostream &out, const solution_t &s)
{  for (int i=0; i<NUM_CITIES; i++)
     out << s.cities[i].x << " ";
   out << s.fitness << endl;
   return out;
}

ostream& operator<<(ostream &out, const solution &s)
{  for (int i=0; i<NUM_CITIES; i++)
     out << s.order[i] << " ";
out << " : " << s.fitness << endl;
   return out;
}

double euclidean_distance( int x1, int y1, int x2, int y2 ); // forward declaration

double fitness(solution_t* chromosome);

bool operator<(const city_t &s1, const city_t &s2)
{
	return euclidean_distance(s1.x,s1.y,0,0) < euclidean_distance(s2.x,s2.y,0,0);
}

bool operator==(const city_t &s1, const city_t &s2)
{
	return s1.x == s2.x && s1.y == s2.y;
}

/* Statistics */
double maximum, average, minimum, sum;
int min_index;

solution_t curSolution_t,tempSolution,childSolution;
solution curSolution;

double distince[NUM_CITIES][NUM_CITIES];
int order[NUM_CITIES];
  
solution solutions[2][POPULATION_SIZE];

clock_t start, finish;
double  cpuTotal; 
int cur = 0;
int generation = 0;



#define SQR(x)	(x*x)
#define ABS(x)	( ((x) > 0 ) ? (x) : ((x)*-1) )

double euclidean_distance( int x1, int y1, int x2, int y2 )
{
  return sqrt( double(SQR(ABS(x1-x2)) + SQR(ABS(y1-y2))) );
}


void compute_tour( solution_t *sol )
{
  int i;
  double fitness = (double)0.0;

  for (i = 0 ; i < NUM_CITIES-1 ; i++) {

    fitness += euclidean_distance( 
                           sol->cities[i].x, sol->cities[i].y,
                           sol->cities[i+1].x, sol->cities[i+1].y );
    
  }

  fitness += euclidean_distance( 
                           sol->cities[NUM_CITIES-1].x, 
                           sol->cities[NUM_CITIES-1].y,
                           sol->cities[0].x, sol->cities[0].y );

  sol->fitness = fitness;

  return;
}


void initWorld( void )
{
	int i;
	string input = "";
	bool save;


	for (i = 0; i < NUM_CITIES; i++) {
		curSolution_t.cities[i].x = cos(i * 2 * 3.14159 / NUM_CITIES)*CITY_LIMIT / 2 + CITY_LIMIT / 2; // RANDMAX(CITY_LIMIT);
		curSolution_t.cities[i].y = sin(i * 2 * 3.14159 / NUM_CITIES)*CITY_LIMIT / 2 + CITY_LIMIT / 2; //RANDMAX(CITY_LIMIT);
		curSolution.order[i] = i;
	}
	random_shuffle(curSolution_t.cities, curSolution_t.cities + NUM_CITIES);

	compute_tour(&curSolution_t);
	curSolution.fitness = curSolution_t.fitness;
	return;
}

double compute_fitness( int cur_pop, int member, int trace )
{
	if (trace) printf("\n");

    solution &s=solutions[cur_pop][member];
	double dist = distince[s.order[NUM_CITIES-1]][s.order[0]];
	      for (int j = 1 ; j < NUM_CITIES ; j++) 
		  {  int i = j-1;
			 dist += distince[s.order[i]][s.order[j]];
		  }
    s.fitness = dist; return solutions[cur_pop][member].fitness;
}

void compute_population_fitness( int cur_pop )
{
  int i;
  double fitness;

  sum = 0.0;
  minimum = 1e12;
  maximum = 0.0;

  for (i = 0 ; i < POPULATION_SIZE ; i++) {

    fitness = compute_fitness( cur_pop, i, TRACE_OFF );
    sum += fitness;

    if (fitness > maximum) maximum = fitness;
	if (fitness < minimum) {minimum = fitness; min_index = i;}

    solutions[cur_pop][i].fitness = fitness;

  }

  average = sum / (double)POPULATION_SIZE;

  return;
}


void initialize_population( int cur_pop )
{
  initWorld();
  
  // initialize distance matrix
  for (int i = 0 ; i < NUM_CITIES ; i++) {
      order[i]=i;
	  for (int j = 0 ; j < NUM_CITIES ; j++) {
		distince[i][j] = euclidean_distance( 
                           curSolution_t.cities[i].x, curSolution_t.cities[i].y,
                           curSolution_t.cities[j].x, curSolution_t.cities[j].y);
	  }
  }

  for (int i = 0 ; i < POPULATION_SIZE ; i++) {
      solutions[cur_pop][i] = curSolution;
      random_shuffle( curSolution.order,curSolution.order+NUM_CITIES );
	  //cout << "init: " << curSolution << endl;
  }
}

int select_parent( int cur_pop )
{
  int i = RANDMAX(POPULATION_SIZE);
  int count = POPULATION_SIZE;
  double select=0.0;

  while (count--) {

    select = solutions[cur_pop][i].fitness;

    if (RANDOM() < ((maximum-select) / sum)) return i;

    if (++i >= POPULATION_SIZE) i = 0;

  }

  return( RANDMAX(POPULATION_SIZE) );
}

int select_parent2( int cur_pop )
{ // tournament selection
  int i = RANDMAX(POPULATION_SIZE);
  int j = RANDMAX(POPULATION_SIZE);
  int k = RANDMAX(POPULATION_SIZE);

  if ( solutions[cur_pop][i].fitness <= solutions[cur_pop][j].fitness &&
	   solutions[cur_pop][i].fitness <= solutions[cur_pop][k].fitness )
	   return i;
  else if (  solutions[cur_pop][j].fitness <= solutions[cur_pop][i].fitness &&
	   solutions[cur_pop][j].fitness <= solutions[cur_pop][k].fitness )
	   return j;
  else
	   return k;

}

int perform_ga( int cur_pop )
{
  int i, j, new_pop;
  int parent_1, parent_2;
  int crossover;

  new_pop = (cur_pop == 0) ? 1 : 0;

  solutions[new_pop][0] = solutions[cur_pop][min_index];    // keep best solution in prev. generation
  solutions[new_pop][1] = solutions[cur_pop][min_index];    // keep best solution in prev. generation
	
	
  for ( i = 2 ; i < POPULATION_SIZE ; i+=2 ) {

    /* i is child_1, i+1 is child_2 */

    parent_1 = select_parent2(cur_pop);  
    parent_2 = select_parent2(cur_pop);  
    solutions[new_pop][i+1] = solutions[cur_pop][parent_2];
	solutions[new_pop][i] = solutions[cur_pop][parent_1];

	if (RANDOM() < CROSSOVER_PROB)
	{   
		int p1 = RANDMAX( NUM_CITIES );
		int p2 = RANDMAX( NUM_CITIES );
		if (p1 > p2) // put in order
		{
			int tmp = p1;
			p1 = p2;
			p2 = tmp;
		}
		
		{   int sz = p2-p1+1;
			int map1[NUM_CITIES];
			int map2[NUM_CITIES];
			
			solution &parent1 = solutions[cur_pop][parent_1];
			solution &parent2 = solutions[cur_pop][parent_2];
            solution &child1 = solutions[new_pop][i];
			solution &child2 = solutions[new_pop][i+1];

			// create maps
			for (int k=0; k<NUM_CITIES; k++) {	
				if (k>= p1 && k<= p2)
				{
					map1[parent1.order[k]] = parent2.order[k];
					map2[parent2.order[k]] = parent1.order[k];
				}
				else
				{   
					map1[parent1.order[k]] = -1;
					map2[parent2.order[k]] = -1;

				}
			}

			// perform crossover
			for (int k=0; k<NUM_CITIES; k++) {	
				if (k>= p1 && k<= p2)
				{
					child1.order[k] = map1[parent1.order[k]];
					child2.order[k] = map2[parent2.order[k]];
				}
				else
				{   
					if (map2[parent1.order[k]] == -1)
						child1.order[k] = parent1.order[k];
					else
					{	int index = parent1.order[k];
						while ( map2[index] != -1) {
							child1.order[k] = map2[index];
							index = map2[index];
						}
					}
					if (map1[parent2.order[k]] == -1)
						child2.order[k] = parent2.order[k];
					else
					{	int index = parent2.order[k];
						while ( map1[index] != -1) {
							child2.order[k] = map1[index];
							index = map1[index];
						}
					}
					

				}
			}
			
		}

		
	}


    
	if (RANDOM() < MUTATION_PROB)
	{
        int p1 = RANDMAX( NUM_CITIES );
		int p2 = RANDMAX( NUM_CITIES );

		
		// swap em
        int tmp = solutions[new_pop][i].order[p1];
		solutions[new_pop][i].order[p1] = solutions[new_pop][i].order[p2];
		solutions[new_pop][i].order[p2] = tmp;
		
	}

	if (RANDOM() < MUTATION_PROB/10)
	{
        
		random_shuffle(solutions[new_pop][i].order,solutions[new_pop][i].order+NUM_CITIES);
	}

  }

  return new_pop;
}


void trace_best( int cur )
{
  int best = 0;
  double best_fitness = 0.0;

  for (int i = 0 ; i < POPULATION_SIZE ; i++) {

    if (solutions[cur][i].fitness < best_fitness) {
      best_fitness = solutions[cur][i].fitness;
      best = i;
    }

  }

  (void)compute_fitness( cur, best, TRACE_ON );
  
  return;
}

#include "graphics.h"

int main(int argc, char** argv)
{
	
  cout << "Initializing population." << endl;
  //RANDINIT();
  initialize_population( cur );
  compute_population_fitness( cur );

 
  start_graphics_loop(argc, argv);
  
  return 0;
}

