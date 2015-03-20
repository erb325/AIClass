#include<stdlib.h>
#include <stdio.h>
//#include <string.h>
#include <math.h>
#include <time.h>

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>
using namespace std;

#include "rand.h"

#define CROSSOVER_PROB		0.7
#define MUTATION_PROB		0.1
#define NUM_CITIES	4
#define CITY_LIMIT	500       // integer bounds on x and y coordinates

const int POPULATION_SIZE=8;      // population size
const int GENERATIONS=8;          // generations
const int TRACE_OFF=0;  
const int TRACE_ON=1; 

struct city_t{
  int x, y;
};

struct solution_t{
  city_t cities[NUM_CITIES];
  double fitness;
} ;

ostream& operator<<(ostream &out, const solution_t &s)
{  for (int i=0; i<NUM_CITIES; i++)
     out << s.cities[i].x << " ";
   out << s.fitness << endl;
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

solution_t curSolution,tempSolution,childSolution;

solution_t solutions[2][POPULATION_SIZE];

clock_t start, finish;
double  cpuTotal; 


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
  string input;
  bool save;
  
  cout << "Save world [y/n]? " << flush;
  cin >> input;
  save = input == "y";
  if (save)
  {
	  cout << "File name? " << flush;
      cin >> input;
	  ofstream out(input.c_str());
      for (i = 0 ; i < NUM_CITIES ; i++) {
             curSolution.cities[i].x = RANDMAX(CITY_LIMIT);
             curSolution.cities[i].y = RANDMAX(CITY_LIMIT);
			 out << curSolution.cities[i].x << " " << curSolution.cities[i].y << endl;
      }
	  out.close();
  }
  else 
  {
	  for (i = 0 ; i < NUM_CITIES ; i++) {
		curSolution.cities[i].x = RANDMAX(CITY_LIMIT);
		curSolution.cities[i].y = RANDMAX(CITY_LIMIT);
	  }
  }
  compute_tour( &curSolution );

  return; 
}

double compute_fitness( int cur_pop, int member, int trace )
{
	if (trace) printf("\n");

    compute_tour(&solutions[cur_pop][member]);
	//solutions[cur_pop][member].fitness = 999999.0 - solutions[cur_pop][member].fitness;
	return solutions[cur_pop][member].fitness;
}

void compute_population_fitness( int cur_pop )
{
  int i;
  double fitness;

  sum = 0.0;
  minimum = 999999.0;
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
  
  for (int i = 0 ; i < POPULATION_SIZE ; i++) {
      solutions[cur_pop][i] = curSolution;
      random_shuffle( curSolution.cities,curSolution.cities+NUM_CITIES );
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


int perform_ga( int cur_pop )
{
  int i, j, new_pop;
  int parent_1, parent_2;
  int crossover;

  new_pop = (cur_pop == 0) ? 1 : 0;

  for ( i = 0 ; i < POPULATION_SIZE ; i+=2 ) {

    /* i is child_1, i+1 is child_2 */

    parent_1 = select_parent(cur_pop);  cout << "P1 = " << solutions[cur_pop][parent_1]<< endl;
    parent_2 = select_parent(cur_pop);  cout << "P2 = " << solutions[cur_pop][parent_2]<< endl;
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
		cout << "Map between " << p1 << " and " << p2 << endl;
        //if (p1 != p2)
		{   int sz = p2-p1+1;
			vector<city_t> mappingSection1(sz);
			vector<city_t> mappingSection2(sz);
			//cout << "Size of mapping section is " << mappingSection1.size() << endl;
			for (int k=0; k<sz; k++) {				
				mappingSection1[k]=solutions[cur_pop][parent_1].cities[p1+k];
				mappingSection2[k]=solutions[cur_pop][parent_2].cities[p1+k];
			}
		
			// copy/map section before crossover pt 1
            for (int k=0; k<p1; k++) {	
				vector<city_t>::iterator itr = find(mappingSection2.begin(),mappingSection2.end(), 
					           solutions[cur_pop][parent_1].cities[k]);
				if ( !(itr == mappingSection2.end()) )							  
					solutions[new_pop][i].cities[k] = *itr;
				else               
					solutions[new_pop][i].cities[k] = solutions[cur_pop][parent_1].cities[k];	
				itr = find(mappingSection1.begin(),mappingSection1.end(), 
					           solutions[cur_pop][parent_2].cities[k]);
				if ( !(itr == mappingSection1.end())  )							  
					solutions[new_pop][i+1].cities[k] = *itr;
				else               
					solutions[new_pop][i+1].cities[k] = solutions[cur_pop][parent_2].cities[k];
			}
			// copy mapped section
			for (int k=p1; k<=p2; k++) {	
				solutions[new_pop][i].cities[k] = mappingSection2[k-p1];
				solutions[new_pop][i+1].cities[k] = mappingSection1[k-p1];
			}
			// copy/map section after crossover pt 2
            for (int k=p2+1; k<NUM_CITIES; k++) {	
				vector<city_t>::iterator itr = find(mappingSection2.begin(),mappingSection2.end(), 
					           solutions[cur_pop][parent_1].cities[k]);
				if ( !(itr == mappingSection2.end())  )							  
					solutions[new_pop][i].cities[k] = *itr;
				else               
					solutions[new_pop][i].cities[k] = solutions[cur_pop][parent_1].cities[k];	
				itr = find(mappingSection1.begin(),mappingSection1.end(), 
					           solutions[cur_pop][parent_2].cities[k]);
				if ( !(itr == mappingSection1.end())  )							  
					solutions[new_pop][i+1].cities[k] = *itr;
				else               
					solutions[new_pop][i+1].cities[k] = solutions[cur_pop][parent_2].cities[k];
			}
		}

		cout << "C1 = " << solutions[new_pop][i]<< endl;
		cout << "C2 = " << solutions[new_pop][i+1]<< endl;
	}


    //solutions[new_pop][i] = solutions[cur_pop][parent_1];
	//if (i==0)
	{
	//	solutions[new_pop][i+1] = solutions[cur_pop][min_index];  // keep best solution in prev. generation
	//	solutions[new_pop][i] = solutions[cur_pop][min_index];  // keep best solution in prev. generation
	}
	//else
    //   solutions[new_pop][i+1] = solutions[cur_pop][parent_2];
	//   solutions[new_pop][i] = solutions[cur_pop][parent_1];

	if (RANDOM() < MUTATION_PROB)
	{
        int p1 = RANDMAX( NUM_CITIES );
		int p2 = RANDMAX( NUM_CITIES );
        int x = solutions[new_pop][i].cities[p1].x;
		int y = solutions[new_pop][i].cities[p1].y;
		solutions[new_pop][i].cities[p1].x = solutions[new_pop][i].cities[p2].x;
		solutions[new_pop][i].cities[p1].y = solutions[new_pop][i].cities[p2].y;
		solutions[new_pop][i].cities[p2].x = x;
		solutions[new_pop][i].cities[p2].y = y;
	}
	if (RANDOM() < MUTATION_PROB/10)
	{
        
		random_shuffle(solutions[new_pop][i].cities,solutions[new_pop][i].cities+NUM_CITIES);
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

int main( void )
{
	/* Perform operation using C++ on the CPU */
		printf("starting CPU calculation ...\n");
		start = clock();

  int cur = 0;
  int generation = 0;

  // RANDINIT();

  initialize_population( cur );
  compute_population_fitness( cur );

  while ((average < (0.999 * maximum)) && (generation<GENERATIONS)) {

    cur = perform_ga( cur );
    compute_population_fitness( cur );

    if (((generation++) % 50) == 0) {
      printf("%6d: %g %g %g\n", generation, minimum, average, maximum);
    }

  }
  printf("%6d: %g %g %g\n", generation, minimum, average, maximum);
  trace_best( cur );

    finish = clock();
    cpuTotal = (double)(finish - start) / CLOCKS_PER_SEC ;
    printf( "%4.3f seconds using CPU\n", cpuTotal ); 

	for (int i = 0 ; i < NUM_CITIES ; i++) {
    printf("%d %d\n", solutions[cur][min_index].cities[i].x, solutions[cur][min_index].cities[i].y);
  }
  printf("%d %d\n", solutions[cur][min_index].cities[0].x, solutions[cur][min_index].cities[0].y);
  printf("\n");

    cin.get();
	cin.get();

  return 0;
}

