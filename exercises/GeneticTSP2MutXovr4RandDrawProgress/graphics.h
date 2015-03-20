
void drawCities() {

	int count = 0;
	while (count < 10 && (average < (0.999 * maximum)) && (generation<GENERATIONS)) {

		cur = perform_ga(cur);
		compute_population_fitness(cur);

		count++;
		generation++;
	}

	printf("%6d: %g %g %g\n", generation, minimum, average, maximum);
	
	glColor3f(0.0, 0.0, 0.0);
	glPointSize(4.0);
		
	glBegin(GL_POINTS);	
		for (int i = 0; i < NUM_CITIES; i++)
		{
			city_t city = curSolution_t.cities[solutions[cur][min_index].order[i]];
			glVertex2i(city.x,city.y);
		}		
	glEnd();

	glLineWidth(1);

	glBegin(GL_LINE_LOOP);
	for (int i = 0; i < NUM_CITIES; i++)
	{
		city_t city = curSolution_t.cities[solutions[cur][min_index].order[i]];
		glVertex2i(city.x, city.y);
	}
	glEnd();

	glFlush();
}


void display()
{	
	glClearColor(1.0,1.0,1.0,1.0);
	glClear(GL_COLOR_BUFFER_BIT);
	drawCities();
	glFlush();
	if ((average < (0.999 * maximum)) && (generation<GENERATIONS))
		glutPostRedisplay();
}


void start_graphics_loop(int argc, char** argv)
{
	// These are the GLUT functions to set up the window and main loop
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_SINGLE | GLUT_RGBA);
	glutInitWindowSize(700, 700);
	glutInitWindowPosition(5, 5);
	glutCreateWindow("TSP Tour");

	// set up viewing mode
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluOrtho2D(-100, 600, -100, 600);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

	// set up event handlers
	glutDisplayFunc(display);

	glutMainLoop();
}