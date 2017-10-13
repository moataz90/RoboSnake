//ReadMe File for the SnakeRobot Simulation by Mirko Gschwindt & Moataz Ibrahim (2017)

//Running the program:
Python 3 needs to be installed.
To start the simulation run the snake.exe file. A command window should open up and tell you which mode you are in. 
You can now start controlling the snake movement. There are two possible modes: controller and mouse

//Using the controller mode:
You need to install the PyGame library for python (we used PyGame version 1.9.3): pygame.org
When starting, the program checks for the pygame library and a connected controller. If it finds both, it will show the message "mode: controller"

//Controls:
mode: controller
Hat to front: Forward
Hat to the right: Roll
Left Joystick forward: FastForward
x: terminate program

mode: mouse
Use the buttons in the top left of the screen.
Control single joints by entering their joint number and moving the slider.

//Change number of joints:
Change the number in the txt file "numberOfJoints" to the number of joints you want to create.
Possible joint range: 7-20
