using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FlamableObject : MonoBehaviour, IFlame
{
     
    public bool isContact { get; set; }
    public float contactTime { get; set; }
    public float threshouldTime { get; set; }


    void Awake (){
        isContact=false;
        contactTime=0;
        threshouldTime= UnityEngine.Random.Range(0.1f,0.2f);
    }


    public void Active(){
        this.gameObject.SetActive(true);
    }
    public void Disable(){
         isContact=false;
         contactTime = 0;
         this.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (isContact&& contactTime>threshouldTime) {
            Disable();
        }

    }
   
}
