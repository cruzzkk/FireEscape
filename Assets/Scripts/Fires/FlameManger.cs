using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameManger : MonoBehaviour
{
    
    [SerializeField]
    float intervel = 20;
    float time;
    [SerializeField]
    List<FlamableObject> m_AllFlamableObject = new List<FlamableObject>();

    void OnEnable(){

       FireExtingParticles.CollissionTrigger += FireCollidewithExting;
        FireExtingParticles.DisableParticleTrigger += ExtingNotCollidewithFire;
    }
    void OnDisable(){
         FireExtingParticles.CollissionTrigger -= FireCollidewithExting;
        FireExtingParticles.DisableParticleTrigger -= ExtingNotCollidewithFire;
    }

    
    void Start()
    {
       // m_AllFlamableObject.AddRange(FindObjectsOfType<FlamableObject>());
        
 

    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > intervel) {
            foreach (FlamableObject a in m_AllFlamableObject)
            {
                if (!a.gameObject.activeSelf) {
                    a.Active(); ;
                }
                
            }
            time = 0;
        }
    }
    public void FireCollidewithExting(FlamableObject other ) { 
       
    if(m_AllFlamableObject.Count>0&&other!=null){

         foreach (FlamableObject a in m_AllFlamableObject) {
             Debug.Log(a.gameObject);
            if (a.transform.gameObject == other.transform.gameObject && !a.isContact)
            {
                a.isContact = true;
               
            }
            else if (a.transform.gameObject != other.transform.gameObject && a.isContact)
            {
                a.isContact = false;
                a.contactTime = 0;
            }
            else if (a.transform.gameObject == other.transform.gameObject && a.isContact) {
                a.contactTime += Time.deltaTime;
            }
        }
    }
       
    }
    public void ExtingNotCollidewithFire() {
        foreach (FlamableObject a in m_AllFlamableObject) {
            a.isContact = false;
            a.contactTime = 0;
        }
    }
}
