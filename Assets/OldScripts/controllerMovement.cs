using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerMovement : MonoBehaviour
{
    public Transform touchLeft;
    public Transform touchRight;

    // Update is called once per frame
    void Update()
    {
        touchLeft.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        touchRight.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

        touchLeft.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        touchRight.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    }
}
