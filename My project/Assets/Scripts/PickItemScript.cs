using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItemScript : MonoBehaviour
{
    public GameObject Pickitem_Timer;
    public GameObject Pickitem_Pickaxe;
    public GameObject LeftHand;
    public GameObject RightHand;

    public GameObject Player;

    bool PickSW_L;
    bool PickSW_R;

    void Update()
    {
        if (PickSW_L == true)
        {
            if (Input.GetKey(KeyCode.JoystickButton4))
            {
                Debug.Log("Left Push");
                Pickitem_Timer.transform.position = LeftHand.transform.position;
                Pickitem_Timer.transform.rotation = LeftHand.transform.rotation;
                Pickitem_Timer.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Debug.Log(Pickitem_Timer.transform.position);
            }
        }

        if (PickSW_R == true)
        {
            if (Input.GetKey(KeyCode.JoystickButton5))
            {
                Debug.Log("Right Push");
                Pickitem_Pickaxe.transform.position = RightHand.transform.position;
                Pickitem_Pickaxe.transform.rotation = RightHand.transform.rotation;
                Pickitem_Pickaxe.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LeftHand")
        {
            LeftHand = other.gameObject;
            PickSW_L = true;
            Debug.Log("Left IN");
        }

        if (other.gameObject.tag == "RightHand")
        {
            RightHand = other.gameObject;
            PickSW_R = true;
            Debug.Log("Right IN");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LeftHand")
        {
            PickSW_L = false;
            Debug.Log("Left OUT");
        }

        if (other.gameObject.tag == "RightHand")
        {
            PickSW_R = false;
            Debug.Log("Right OUT");
        }
    }
}
