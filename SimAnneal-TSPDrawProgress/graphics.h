double temperature = INITIAL_TEMP, delta_e;
solution_t tempSolution;
int iteration;

void drawCities() {

	int count = 0;

	while (count < 100 && temperature > 0.0001) {
	//if (temperature > 0.0001) {

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
		count++;
	}

	glColor3f(0.0, 0.0, 0.0);
	glPointSize(4.0);
		
	glBegin(GL_POINTS);	
		for (int i = 0; i < MAX_CITIES; i++)
		{
			city_t city = curSolution.cities[i];
			glVertex2i(city.x,city.y);
		}		
	glEnd();

	glLineWidth(1);

	glBegin(GL_LINE_LOOP);
	for (int i = 0; i < MAX_CITIES; i++)
	{
		city_t city = curSolution.cities[i];
		glVertex2i(city.x, city.y);
	}
	glEnd();

	glFlush();
}


void display()
{	
	glClearColor(1.0,1.0,1.0,1.0);
	glClear(GL_COLOR_BUFFER_BIT);
	cout << "Total tour length = " << curSolution.tour_length << endl;
	drawCities();
	glFlush();
	
	if (temperature > 0.0001)
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
	//printf("GL_VERSION = %s\n", glGetString(GL_VERSION)); 


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