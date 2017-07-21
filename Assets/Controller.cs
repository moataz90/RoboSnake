using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;


public class Controller : MonoBehaviour {

//    PythonEngine engine = new PythonEngine();
//    engine.LoadAssembly(Assembly.GetAssembly(typeof(GameObject)));
//engine.ExecuteFile("apple.py");
    
    public Light HeadLight;
    public GameObject SnakeHead;
    public GameObject SnakeNode1;
    public GameObject SnakeNode2;
    public GameObject SnakeNode3;
    public GameObject SnakeNode4;
    public GameObject SnakeNode5;
    public GameObject SnakeTail;

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
        strCmdText = "/C python main.py";
        System.Diagnostics.Process.Start("CMD.exe", strCmdText);

    }
 

    // Update is called once per frame
    void Update () {


        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.Add(SnakeHead);
        gameObjects.Add(SnakeNode1);
        gameObjects.Add(SnakeNode2);
        gameObjects.Add(SnakeNode3);
        gameObjects.Add(SnakeNode4);
        gameObjects.Add(SnakeNode5);
        //gameObjects.Add(SnakeTail);


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


        //if (Input.GetKey("up"))
        //{    
        //}
        //else if (Input.GetKey("down"))
        //{
        //}

    }
       void ResetClick()
        {
        System.IO.File.WriteAllText("joint_angles.txt", "0:0&1:0&2:0&3:0&4:0&5:0");
        System.IO.File.WriteAllText("CurrentCommand.txt", "");
        Debug.Log("Reset Pressed");
        }


        void ForwardClick()
        {
            System.IO.File.WriteAllText("CurrentCommand.txt", "Forward");
             Debug.Log("Reset Pressed");
    }


        void RollClick()
        {
        System.IO.File.WriteAllText("CurrentCommand.txt", "Roll");
        Debug.Log("Reset Pressed");
    }
    void FastForwardClick()
    {
        System.IO.File.WriteAllText("CurrentCommand.txt", "Roll");
        Debug.Log("Reset Pressed");
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
