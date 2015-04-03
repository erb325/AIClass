//Ember Baker

#include <stdio.h>
#include <math.h>
#include <iostream>
using namespace std;

#include "rand.h"
#include "glut.h"

typedef struct {
  double x;
  double y;
} vec_t;

typedef struct {
  vec_t coord;
  vec_t velocity;
  double fitness;
  vec_t best_coord;
  double fitness_best;
} particle_t;


#define MAX_PARTICLES		20

#define MAX_ITERATIONS		50


particle_t particles[MAX_PARTICLES];
particle_t gbest;

double c1 = (double)2.05;
double c2 = (double)2.05;
double dt = (double)1;
double w = .729;


double compute_fitness( vec_t *vec_p )
{
  double x,y;
  double fitness;

  /* Cache the coordinates to simplify the function. */
  x = vec_p->x;
  y = vec_p->y;

  /* Bound the location of the particle */
  if ((x < -10.0) || (x > 10.0) ||
      (y < -10.0) || (y > 10.0)) fitness = 0.0;
  else {

    /* Equation 9.5 */
    fitness = 
        (sin(x)/x) * (sin(y)/y) * (double)10.0;

  }

  return fitness;
}


void update_particle( particle_t *particle_p )
{

  /* Update the particle's position (Equation 9.6) */
  particle_p->coord.x += (particle_p->velocity.x * dt);
  particle_p->coord.y += (particle_p->velocity.y * dt);

  /* Evaluate the particle's fitness */
  particle_p->fitness = compute_fitness( &particle_p->coord );

  /* If the fitness is better than the personal best, then save it. */
  if (particle_p->fitness > particle_p->fitness_best) {

    particle_p->fitness_best = particle_p->fitness;
    particle_p->best_coord.x = particle_p->coord.x;
    particle_p->best_coord.y = particle_p->coord.y;


    /* If the fitness is better than the global best, then save it. */
    if (particle_p->fitness_best > gbest.fitness) {

      gbest.fitness = particle_p->fitness_best;
      gbest.coord.x = particle_p->coord.x;
      gbest.coord.y = particle_p->coord.y;

    }

  }

  /* Update the velocity vector (Equation 9.7) */
  particle_p->velocity.x =
	  w*(particle_p->velocity.x +
	  c1 * RANDOM() *
	  (gbest.coord.x - particle_p->coord.x) +
	  c2 * RANDOM() *
	  (particle_p->best_coord.x - particle_p->coord.x));

  particle_p->velocity.y =
	  w*(particle_p->velocity.y +
	  c1 * RANDOM() *
	  (gbest.coord.y - particle_p->coord.y) +
	  c2 * RANDOM() *
	  (particle_p->best_coord.y - particle_p->coord.y));

  return;
}


void init_population( void )
{
  int i;

  for (i = 0 ; i < MAX_PARTICLES ; i++) {

    particles[i].coord.x = (RANDOM() * 20.0 - 10.0);
    particles[i].coord.y = (RANDOM() * 20.0 - 10.0);

    particles[i].fitness = compute_fitness( &particles[i].coord );

    /* Initialize the particle's velocity */
    particles[i].velocity.x = (RANDOM()/10.0);
    particles[i].velocity.y = (RANDOM()/10.0);

    /* Store the current best for this particle */
    particles[i].best_coord.x = particles[i].coord.x;
    particles[i].best_coord.y = particles[i].coord.y;
    particles[i].fitness_best = particles[i].fitness;

  }

  gbest.fitness = 0.0;

  return;
}


#include "graphics.h"

int main(int argc, char** argv)
{
  int i, j;

 // RANDINIT();

  init_population();

  /*for (i = 0 ; i < MAX_ITERATIONS ; i++) {

    for (j = 0 ; j < MAX_PARTICLES ; j++) {

      update_particle( &particles[j] );

    } 

    //printf("Current Best: %g %g = %g\n", 
    //         gbest.coord.x, gbest.coord.y, gbest.fitness);

  } */

  start_graphics_loop(argc, argv);

  cin.get();
  return 0;
}

