""" Main file of the snake robot simulation
 tasks: get commands from an x-Box-Controller and calculate according snake joint positions; write joint positions to a
 text file """

import time

# substitute class for real snake robot
from SnakeSubstitute import SnakeSubstitute


# get command parameters from text gait
def get_command_params(gait):
    command_params = [0 for i in range(5)]
    if gait == 'Forward':
        amplitude_vertical = 15
        amplitude_horizontal = 0
        frequency = 5
        phase_vertical = 120
        phase_horizontal = 60
    elif gait == 'Roll':
        amplitude_vertical = 15
        amplitude_horizontal = 15
        frequency = 5
        phase_vertical = 0
        phase_horizontal = 0
    elif gait == 'FastForward':
        amplitude_vertical = 15
        amplitude_horizontal = 0
        frequency = 9
        phase_vertical = 120
        phase_horizontal = 60
    else:
        amplitude_vertical = 0
        amplitude_horizontal = 0
        frequency = 0
        phase_vertical = 0
        phase_horizontal = 0
    command_params[0] = amplitude_vertical
    command_params[1] = amplitude_horizontal
    command_params[2] = frequency
    command_params[3] = phase_vertical
    command_params[4] = phase_horizontal
    return command_params


def main():
    # read number of joints from text file
    number_file = open('numberOfJoints.txt', 'r')
    num_joints = int(number_file.read())
    number_file.close()

    # check for controller. if no controller found, switch to mouse control
    try:
        # using pygame python library (pygame.org)
        import pygame
        # pygame initialization stuff
        pygame.init()
        pygame.joystick.init()
        controller = pygame.joystick.Joystick(0)
        controller.init()
        mode = "controller"
    except:
        mode = "mouse"

    # initialize angles
    angles = [0 for i in range(num_joints)]
    for i in range(0, num_joints):
        angles[i] = 0

    start_time = time.clock()

    # loop to check for button commands
    done = False
    gait = ''
    while not done:
        if mode == "controller":
            pygame.event.get()
            hat = controller.get_hat(0)
            # possible gaits: Forward, Roll
            if hat == (0, 1):
                gait = 'Forward'
            elif hat == (1, 0):
                gait = 'Roll'
            elif controller.get_axis(1) < -0.7:
                gait = 'FastForward'
            else:
                gait = ''
            # quit, if x is pressed
            if controller.get_button(0) == 1:
                done = True
        elif mode == "mouse":
            command_file = open('CurrentCommand.txt', 'r')
            gait = command_file.read()
            command_file.close()
        else:
            print("Error, mode not found.")
            done = True

        gait_params = get_command_params(gait)

        # get the update for old joint angles
        t = time.clock()-start_time
        snake = SnakeSubstitute(num_joints)
        absolute_angles = snake.get_angles_abs(gait_params[0], gait_params[1], gait_params[2], gait_params[3], gait_params[4], t)

        # round angles
        for j in range(0, num_joints):
            angles[j] = round(absolute_angles[j])

        # update joint angles file
        if gait != '':
            try:
                angle_file = open('joint_angles.txt','w')
                for k in range(0, num_joints-1):
                    angle_file.write(str(k)+":"+str(angles[k]*10)+"&")
                angle_file.write(str(num_joints-1)+":"+str(angles[num_joints-1]*10))
                angle_file.close()
            except:
                pass

        # short time delay for smoothness of motion
        time.sleep(0.01)

    pygame.quit()

if __name__ == "__main__":
    main()