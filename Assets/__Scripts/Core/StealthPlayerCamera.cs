﻿using UnityEngine;


public class StealthPlayerCamera : MonoBehaviour
{
    static private StealthPlayerCamera _S;

    public enum eCamMode { far, nearL, nearR };

    [Header("Inscribed")]
    [Tooltip("The instance of ThirdPersonWallCover attached to the player character.")]
    public ThirdPersonWallCover playerInstance;

    [Tooltip("[0..1] At 0, the camera will never move, at 1, the camera will " +
             "follow immediately with no lag.")]
    [Range(0, 1)]
    public float cameraEasing = 0.25f;

    [Header("Inscribed – Far Mode")]
    [Tooltip("If this is set to [0,0,0], the relative position of the camera " +
             "to the player in the scene will be used.")]
    public Vector3 relativePosFar = Vector3.zero;
    [Tooltip("The rotation about the x axis of the camera in Far mode.")]
    public float xRotationFar = 60;

    [Header("Inscribed – Near Mode")]
    public Vector3 relativePosNear = new Vector3(0, 2.5f, -2);
    [Tooltip("Determines how far the camera will lead the player in Near mode.")]
    public float relativePosNearLRShift = 1;
    [Tooltip("The rotation about the x axis of the camera in Near mode.")]
    public float xRotationNear = 25;

    [Header("Dynamic")]
    public eCamMode camMode = eCamMode.far;


    private void Awake()
    {
        S = this;

        // If the desiredRelativePos is unset, base it on where the camera starts relative to the player
        if (relativePosFar == Vector3.zero)
        {
            relativePosFar = transform.position - playerInstance.transform.position;
        }
    }


    // Update is called once per frame
    void Update()
    {
        ThirdPersonWallCover.CoverInfo coverInfo = playerInstance.GetCoverInfo();
        if (coverInfo.inCover == -1)
        {
            // When not inCover, the camMode is always eCamMode.far
            camMode = eCamMode.far;
        }
        else
        {
            // When inCover, the camMode switches to eCamMode.near_ if the player 
            //  is near the edge of cover
            if (coverInfo.zoomL && !coverInfo.zoomR)
            {
                camMode = eCamMode.nearR;
            }
            else if (!coverInfo.zoomL && coverInfo.zoomR)
            {
                camMode = eCamMode.nearL;
            }
            else
            {
                camMode = eCamMode.far;
            }
        }

        // This is initially [0,0,0] to show the issue visually by jumping the Camera
        // to the origin if the position is not set properly in the switch statement.
        Vector3 pDesired = Vector3.zero;
        Quaternion rotDesired = Quaternion.identity;
        switch (camMode)
        {
            case eCamMode.far:
                pDesired = playerInstance.transform.position + relativePosFar;
                rotDesired = Quaternion.Euler(xRotationFar, 0, 0);
                break;
            case eCamMode.nearL:
            case eCamMode.nearR:
                // Desired position should be relative to playerInstance facing and position
                Vector3 pRelative = relativePosNear;
                pRelative.x += (camMode == eCamMode.nearL) ? -relativePosNearLRShift
                    : relativePosNearLRShift;
                pDesired = playerInstance.transform.TransformPoint(pRelative);
                rotDesired = Quaternion.Euler(xRotationNear, coverInfo.inCover * 90, 0);
                break;
        }

        Vector3 pInterp = (1 - cameraEasing) * transform.position + cameraEasing * pDesired;
        transform.position = pInterp;

        Quaternion rotInterp = Quaternion.Slerp(transform.rotation, rotDesired, cameraEasing);
        transform.rotation = rotInterp;
    }


    public void JumpToFarPosition()
    {
        transform.position = playerInstance.transform.position + relativePosFar;
        transform.rotation = Quaternion.Euler(xRotationFar, 0, 0);
    }



    static private StealthPlayerCamera S
    {
        get { return _S; }
        set
        {
            if (_S != null)
            {
                Debug.LogError("StealthPlayerCamera:S - Attempt to set Singleton" +
                               " when it has already been set.");
                Debug.LogError("Old Singleton: " + _S.gameObject.name +
                               "\tNew Singleton: " + value.gameObject.name);
            }
            _S = value;
        }
    }

    static public eCamMode MODE
    {
        get
        {
            if (_S == null)
            {
                return eCamMode.far;
            }
            return _S.camMode;
        }
    }

    static public void ResetToFarPosition()
    {
        S.JumpToFarPosition();
    }

}
