// XR 기기 정보 캐싱 클래스

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRInputData : MonoBehaviour
{
    public InputDevice rightController;
    public InputDevice leftController;
    public InputDevice HMD;


    void Update()
    {
        if (!rightController.isValid || !leftController.isValid || !HMD.isValid)
            InitializeInputDevices();
    }

    private void InitializeInputDevices()
    {

        if (!rightController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref rightController);
        if (!leftController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref leftController);
        if (!HMD.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref HMD);

    }

    private void InitializeInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);

        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
    }
}