using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControllerTest : MonoBehaviour
{
    public Vector2 LeftStick;
    public Vector2 RightStick;

    int currentConnectionCount;
    string[] cName;

    void Start()
    {
        cName = Input.GetJoystickNames();
        for (int i = 0; i < cName.Length; i++)
        {
            if (cName[i] != "")
            {
                currentConnectionCount++;
                Debug.Log(cName[0]);
                //Debug.Log(cName[1]);
            }
        }
    }
    void Update()
    {
        DownKeyCheck();
        StickCheck();
    }


    void DownKeyCheck()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    //ˆ—‚ð‘‚­
                    Debug.Log(code);
                    break;
                }
            }
        }
    }

    void StickCheck()
    {
        LeftStick  = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        RightStick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
    }
}
