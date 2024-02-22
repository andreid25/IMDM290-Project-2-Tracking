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
    static int handLandmark_number = 20;
    // Declare landmark vectors 
    public Vector3[] righthandpos = new Vector3[handLandmark_number];
    public Vector3[] lefthandpos = new Vector3[handLandmark_number];
    public GameObject[] PoseLandmarks, LeftHandLandmarks, RightHandLandmarks;
    public static Gesture gen; // singleton
    public bool drawLandmarks = false;
    
    //positioning hand
    public Transform rightWristTransform;
    public Transform leftWristTransform;
    //smoothing hand movement
    public float smoothTime = 0.1f; // Adjust this value to control the smoothing speed
    private Vector3 rightWristVelocity = Vector3.zero;
    private Vector3 leftWristVelocity = Vector3.zero;
    public float rotationSmoothTime = 0.1f; // Adjust this value to control the rotation smoothing speed
    private Quaternion rightWristRotationVelocity = Quaternion.identity;
    private Quaternion leftWristRotationVelocity = Quaternion.identity;

    //hitbox
    public GameObject rBox, lBox;
    public float rBoxZ, lBoxZ;

    private void Awake()
    {
        if (Gesture.gen == null)
        {
            Gesture.gen = this;
        }
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

            // // Assign Left hand landmarks position
            idx = 0;
            foreach (GameObject lhl in LeftHandLandmarks)
            {
                lhl.transform.transform.position = -lefthandpos[idx];
                Color customColor = new Color(idx * 4 / 255, idx * 15f / 255, idx * 30f / 255, 1); // Color of left hand landmarks
                lhl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
                lhl.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                idx++;
            }
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


        // Target positions are the negative of the hand positions
        Vector3 rightTargetPosition = -righthandpos[0];
        Vector3 leftTargetPosition = -lefthandpos[0];

        // Smoothly move the right wrist to the new position
        rightWristTransform.position = Vector3.SmoothDamp(rightWristTransform.position, rightTargetPosition, ref rightWristVelocity, smoothTime);

        // Smoothly move the left wrist to the new position
        leftWristTransform.position = Vector3.SmoothDamp(leftWristTransform.position, leftTargetPosition, ref leftWristVelocity, smoothTime);
        
        
        RotateWristTowardsTargetPosition(-righthandpos[2],-righthandpos[12],rightWristTransform);
        RotateWristTowardsTargetPosition(-lefthandpos[2],-lefthandpos[12],leftWristTransform);


        Vector3 tkRposition = new Vector3(0, 0, 0);
        Vector3 tkLposition = new Vector3(0, 0, 0);
        for (int i = 0; i < 20; i++)
        { 
            tkRposition = tkRposition - righthandpos[i];
            tkLposition = tkLposition - lefthandpos[i];
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
    // This helps ensure that xDirection is orthogonal to the plane defined by yDirection and towardsThumb
    Vector3 xDirection = Vector3.Cross(yDirection, towardsThumb).normalized;
    
    
    // Now, use the x, y, and z directions to construct a rotation matrix
    Quaternion targetRotation = Quaternion.LookRotation(xDirection, yDirection);
    
    // Smoothly interpolate the wrist's rotation towards the target rotation
    wristTransform.rotation = Quaternion.Slerp(wristTransform.rotation, targetRotation, Time.deltaTime / rotationSmoothTime);
}


}
