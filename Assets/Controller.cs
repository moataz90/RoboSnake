﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class Controller : MonoBehaviour {



    public GameObject snake7;
    public GameObject snake8;
    public GameObject snake20;

    public HingeJoint  hinge;
    public JointLimits jointLimit;

    public Button ResetButton;
    public Button ForwardButton;
    public Button FastForwardButton;
    public Button RollButton;
    public InputField SingleAngleField;
    public Slider SingleAngleSlider;



    // Use this for initialization
    void Start () {

        ResetButton.GetComponent<Button>().onClick.AddListener(ResetClick);
        ForwardButton.GetComponent<Button>().onClick.AddListener(ForwardClick);
        FastForwardButton.GetComponent<Button>().onClick.AddListener(FastForwardClick);
        RollButton.GetComponent<Button>().onClick.AddListener(RollClick);

        SingleAngleSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { SliderValueChangeCheck(); });





        string strCmdText;
        strCmdText = "/C py -3 main.py";
        System.Diagnostics.Process.Start("CMD.exe", strCmdText);

    }
 

    // Update is called once per frame
    void Update () {

        //retrive all Snake models from the scene
        Transform[] snakesTrans;
        snakesTrans = this.gameObject.GetComponentsInChildren<Transform>();
        List<GameObject> snakes = new List<GameObject>();
        foreach (Transform trans in snakesTrans)
        {

            if (Regex.IsMatch(trans.gameObject.name, @"snake[0-9]*$", RegexOptions.IgnoreCase))
                snakes.Add(trans.gameObject);
        }



        //Select the snake model depending on the number of nodes
        HingeJoint[] snakeNodes ;
        string numberofnodes = System.IO.File.ReadAllText("numberOfJoints.txt");
        snakeNodes = snake7.GetComponentsInChildren<HingeJoint>();
        foreach (GameObject snake in snakes)
        {
            if (snake.name.ToLower() == "snake" + numberofnodes.ToString())
            {
                snakeNodes = snake.GetComponentsInChildren<HingeJoint>();
                snake.SetActive(true);
                snake.transform.position = new Vector3(0, 0, 0);

            }
            else
            {
                snake.SetActive(false);
            }

        }
        

        //Get all gameobject node of the selected snake
        List<GameObject> gameObjects = new List<GameObject>();
        foreach (HingeJoint hinge in snakeNodes)
        {
            gameObjects.Add(hinge.gameObject);
        }

         gameObjects.Reverse();


        //set the joints of the snake model
        string joint_angles = System.IO.File.ReadAllText("joint_angles.txt");
        string[] angleList = joint_angles.Split('&');
        for (int i = 0; i < gameObjects.Count; i++)
        {

            jointLimit = new JointLimits();

            jointLimit.min = float.Parse(angleList[i].Split(':')[1]) / 10;
            jointLimit.max = (jointLimit.min == 0) ? jointLimit.min : jointLimit.min + (float)0.1;


            hinge = gameObjects[i].GetComponents<HingeJoint>()[0];

            hinge.limits = jointLimit;

        }


    }

    //Interface functions
    void ResetClick()
        {
        int numberofnodes = int.Parse(System.IO.File.ReadAllText("numberOfJoints.txt"));
        string joints = "";
        for (int i = 0; i < numberofnodes; i++)
        {
            joints += (i-1 == numberofnodes)? "0:" + i  : "0:" + i + "&";

        }
        System.IO.File.WriteAllText("joint_angles.txt", joints);
        System.IO.File.WriteAllText("CurrentCommand.txt", "");
        Debug.Log("Reset Pressed");
        }
    void ForwardClick()
        {
            System.IO.File.WriteAllText("CurrentCommand.txt", "Forward");
             Debug.Log("Forward Pressed");
    }
    void RollClick()
        {
        System.IO.File.WriteAllText("CurrentCommand.txt", "Roll");
        Debug.Log("Roll Pressed");
    }
    void FastForwardClick()
    {
        System.IO.File.WriteAllText("CurrentCommand.txt", "FastForward");
        Debug.Log("Fast Forward Pressed");
    }
    void SliderValueChangeCheck()
    {


        int jointID = (SingleAngleField.text == "") ? -1 : int.Parse(SingleAngleField.text);
        if (jointID > -1)
        {
            float sliderv = SingleAngleSlider.value;
            string joint_angles = System.IO.File.ReadAllText("joint_angles.txt");

            string[] angleList = joint_angles.Split('&');
            string[] angle = angleList[jointID].Split(':');
            angle[1] = SingleAngleSlider.value.ToString();
            angleList[jointID] = String.Join(":", angle);
            joint_angles = String.Join("&", angleList);
            System.IO.File.WriteAllText("joint_angles.txt", joint_angles);
        }
    }




}
