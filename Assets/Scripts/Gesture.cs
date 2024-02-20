// IMDM Course material
// Author: Myungin Lee
// Date: Fall 2023
// This code demonstrates the general applications of landmark information
// Pose + Left, Right hand landmarks data avaiable. Facial landmark need custom work
// Landmarks label reference: 
// https://developers.google.com/mediapipe/solutions/vision/pose_landmarker
// https://developers.google.com/mediapipe/solutions/vision/hand_landmarker

using Mediapipe.Unity.Holistic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gesture : MonoBehaviour
{
    static int poseLandmark_number = 32;
    static int handLandmark_number = 20;
    // Declare landmark vectors 
    public Vector3[] pose = new Vector3[poseLandmark_number];
    public Vector3[] righthandpos = new Vector3[handLandmark_number];
    public Vector3[] lefthandpos = new Vector3[handLandmark_number];
    public GameObject[] PoseLandmarks, LeftHandLandmarks, RightHandLandmarks;
    public static Gesture gen; // singleton
    public bool drawLandmarks = false;
    public bool trigger = false;
    private float distance;
    int totalNumberofLandmark;
    public Transform rightWristTransform;
    public Transform leftWristTransform;
    public GameObject rBox, lBox;
    public float rBoxZ, lBoxZ;
    private void Awake()
    {
        if (Gesture.gen == null)
        {
            Gesture.gen = this;
        }
        totalNumberofLandmark = poseLandmark_number + handLandmark_number + handLandmark_number;
        PoseLandmarks = new GameObject[poseLandmark_number];
        LeftHandLandmarks = new GameObject[handLandmark_number];
        RightHandLandmarks = new GameObject[handLandmark_number];
    }
    // Start is called before the first frame update
    void Start()
    {
         rBoxZ = rBox.transform.position.z;
         lBoxZ = lBox.transform.position.z;


        if (drawLandmarks)
        {
            // Initiate pose landmarks as spheres
            // for (int i = 0; i < poseLandmark_number; i++)
            // {
            //     PoseLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // }
            // Initiate R+L hands landmarks as spheres
            for (int i = 0; i < handLandmark_number; i++)
            {
                //LeftHandLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                RightHandLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        // Case 0. Draw holistic shape
        // Assign Pose landmarks position
        int idx = 0;
        if (drawLandmarks)
        {
            // foreach (GameObject pl in PoseLandmarks)
            // {
            //     pl.transform.transform.position = -pose[idx];
            //     Color customColor = new Color(idx * 10 / 255, idx * 7 / 255, idx * 3 / 255, 1); // Color of pose landmarks
            //     pl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
            //     pl.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            //     idx++;
            // }
            // // Assign Left hand landmarks position
            // idx = 0;
            // foreach (GameObject lhl in LeftHandLandmarks)
            // {
            //     lhl.transform.transform.position = -lefthandpos[idx];
            //     Color customColor = new Color(idx * 4 / 255, idx * 15f / 255, idx * 30f / 255, 1); // Color of left hand landmarks
            //     lhl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
            //     lhl.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            //     idx++;
            // }
            // Assign Right hand landmarks position
            idx = 0;
            foreach (GameObject rhl in RightHandLandmarks)
            {
                rhl.transform.transform.position = -righthandpos[idx];
                Color customColor = new Color(idx * 4f / 255, idx * 15f / 255, idx * 30f / 255, 1); // Color of right hand landmarks
                rhl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
                rhl.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                idx++;
            }
        }


        rightWristTransform.position = -righthandpos[0];

        RotateWristTowardsTargetPosition(-righthandpos[2],-righthandpos[12],rightWristTransform);

        leftWristTransform.position = -lefthandpos[0];

        RotateWristTowardsTargetPosition(-lefthandpos[2],-lefthandpos[12],leftWristTransform);


        Vector3 tkRposition = new Vector3(0, 0, 0);
        Vector3 tkLposition = new Vector3(0, 0, 0);
        for (int i = 0; i < 20; i++)
        { 
            tkRposition = tkRposition - Gesture.gen.righthandpos[i];
            tkLposition = tkLposition - Gesture.gen.lefthandpos[i];
        }
        tkRposition = tkRposition / 20;
        tkLposition = tkLposition / 20;

        rBox.transform.position = new Vector3(tkRposition.x,tkRposition.y,rBoxZ);
        lBox.transform.position = new Vector3(tkLposition.x,tkLposition.y,lBoxZ);
    }

//this function used three points to define a direction the wrist is treated as the parent and
//the  middletip is positive y and the thumbase is negative x, if you'll need to flip your thumbs X component
//for the opposite hand
void RotateWristTowardsTargetPosition(Vector3 thumbBase, Vector3 middleTip, Transform wristTransform)
{
    // Direction from wrist to middleTip defines the local y-axis direction
    Vector3 yDirection = (middleTip - wristTransform.position).normalized;
    
    // Calculate a rough xDirection pointing towards the thumb
    // First, find a vector pointing from the wrist towards the thumb
    Vector3 towardsThumb = (thumbBase - wristTransform.position).normalized;
    
    // Use the cross product to find a vector perpendicular to yDirection and towardsThumb
    // This helps ensure that zDirection is orthogonal to the plane defined by yDirection and towardsThumb
    Vector3 zDirection = Vector3.Cross(yDirection, towardsThumb).normalized;
    
    // Recalculate xDirection to ensure it's perfectly orthogonal to yDirection and zDirection
    // This corrects any deviations due to the initial approximation
    Vector3 xDirection = Vector3.Cross(zDirection, yDirection).normalized;
    
    // Now, use the x, y, and z directions to construct a rotation matrix
    Quaternion targetRotation = Quaternion.LookRotation(zDirection, yDirection);
    
    // Apply the calculated rotation to the wrist
    wristTransform.rotation = targetRotation;
}


}
