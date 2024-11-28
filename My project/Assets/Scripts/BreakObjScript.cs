using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObjScript : MonoBehaviour
{
    public int MaxBreakCount;
    public int NowBreakCount;

    public float maxBreakTime;
    float nowBreakTime;

    bool TimerSW;

    void Start()
    {
        NowBreakCount = MaxBreakCount;
    }

    void Update()
    {
        if (TimerSW == true)
        {
            if (nowBreakTime <= maxBreakTime)
            {
                nowBreakTime += Time.deltaTime;
            }
        }

        if (nowBreakTime >= maxBreakTime)
        {
            TimerSW = false;
        }

        if (NowBreakCount == 0)
        {
            Destroy(gameObject,0.5f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pickaxe")
        {
            if (TimerSW == false)
            {
                NowBreakCount -= 1;
                TimerSW = true;
            }
        }
    }
}
