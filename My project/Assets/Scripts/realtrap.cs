using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class realtrap : MonoBehaviour
{
    public GameObject wall;
    public GameObject wallprefab;
    
   
    void Update()
    {
      if(Input.GetMouseButtonDown(1))
        {
            wall.SetActive(false);
        }
    

    }
 
}


