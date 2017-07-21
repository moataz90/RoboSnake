import math


class SnakeSubstitute:
    """Class that substitutes for the real snake. Receives command parameters and returns joint angles"""

    def __init__(self, number_of_joints):
        self.num_joints = number_of_joints

    # method for calculating relative joint angles from gait; return relative angles
    def get_angles_abs(self, A_v, A_h, w, p_v, p_h, t):
        abs_angles = [0 for i in range(self.num_joints)]
        for i in range(0, self.num_joints):
            if i % 2 == 0:
                abs_angles[i] = A_h * math.sin(w * t + p_h)
            else:
                abs_angles[i] = A_v * math.sin(w * t + p_v)
        return abs_angles

