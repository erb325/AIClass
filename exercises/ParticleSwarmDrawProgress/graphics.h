int iteration = 0;

void drawParticles() {

	for (int j = 0; j < MAX_PARTICLES; j++) {

		update_particle(&particles[j]);

	}

	glColor3f(0.0, 0.0, 0.0);
	glPointSize(3.0);
		
	glBegin(GL_POINTS);	
	for (int j = 0; j < MAX_PARTICLES; j++) {

		glVertex2d(particles[j].coord.x, particles[j].coord.y);

	}
	glPointSize(4.0);
	glVertex2d(gbest.coord.x, gbest.coord.y);
	glEnd();

	glLineWidth(1);

	//glFlush();
	glutSwapBuffers();

	iteration++;
}


void display()
{	
	glClearColor(1.0,1.0,1.0,1.0);
	glClear(GL_COLOR_BUFFER_BIT);
	
	drawParticles();
	glFlush();

	
	printf("Current Best: %g %g = %g\n",
		         gbest.coord.x, gbest.coord.y, gbest.fitness);

	if (iteration < MAX_ITERATIONS)
		glutPostRedisplay();
	
	
}


void start_graphics_loop(int argc, char** argv)
{
	// These are the GLUT functions to set up the window and main loop
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA);
	glutInitWindowSize(700, 700);
	glutInitWindowPosition(5, 5);
	glutCreateWindow("PSO");
	//printf("GL_VERSION = %s\n", glGetString(GL_VERSION)); 


	// set up viewing mode
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluOrtho2D(-10, 10, -10, 10);
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

	// set up event handlers
	glutDisplayFunc(display);

	glutMainLoop();
}