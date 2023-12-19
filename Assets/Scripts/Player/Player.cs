using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Action<GrabbleObject> GrabObjectCallback;
    public static Action<GrabbleObject> DropObjectCallback;

    public static Action<GrabbleObject> FireCallback;
    public static Action<GrabbleObject> UnFireCallback;

    public static Action<int> RescuedCallback;

    [SerializeField] GameObject pickUpScreen;
    [SerializeField] GameObject FollowUpScreen;
    public InputActionReference grab,fire,follow;
    public bool grabbleobjectnear=false;
    public bool playersinRescueArea=false;
    public GrabbleObject grabbleObject = null;
    public GrabbleObject currentGrabbleObject = null;
    List<GrabbleObject> grabbedObjects =new List<GrabbleObject> ();
    List<Enemy> overlapedEnemies =new List<Enemy>();


    public Transform AttachTo;
    public Transform AttachToFront;
    
    void OnEnable() {
       
        grab.action.performed += Grab;
        fire.action.performed += Fire;
        fire.action.canceled += UnFire;
        follow.action.performed += Follow;
        Enemy.EnemyTriggerCallback+=EnemyTriggedStart;
        Enemy.EnemyTriggerExitCallback+=EnemyTriggedExit;
        Enemy.EnemyDeathCallback+=CallwhenEnemyDead;
    }
    void OnDisable() {
       
        grab.action.performed -= Grab;
        fire.action.performed -= Fire;
        fire.action.canceled -= UnFire;
        follow.action.performed -= Follow;
        Enemy.EnemyTriggerCallback-=EnemyTriggedStart;
        Enemy.EnemyTriggerExitCallback-=EnemyTriggedExit;
         Enemy.EnemyDeathCallback-=CallwhenEnemyDead;
    }

    

    void PickupScreen() {
        pickUpScreen.SetActive(true);
    }
    void PickupScreenOff()
    {
        pickUpScreen.SetActive(false);
    }
    void FollowUpScreenOnOFF(bool value){
        FollowUpScreen.SetActive(value);
    }

    void Grab(InputAction.CallbackContext obj)
    {
        if (grabbleobjectnear) {

          
            if(grabbedObjects.Count>=1)
            { 
                if(!grabbedObjects.Contains(grabbleObject)){
                    grabbedObjects.Add(grabbleObject);
                    Attach(grabbleObject.transform);
                     
                } 
            }
            else{
                  grabbedObjects.Add(grabbleObject);
                  Attach(grabbleObject.transform);
                  
            }
          
        }
        else {
            if (grabbedObjects.Count >= 1) {
                DropObjectCallback?.Invoke(grabbedObjects[grabbedObjects.Count - 1]);
                Detach(grabbedObjects[grabbedObjects.Count - 1].transform);
                grabbedObjects.Remove(grabbedObjects[grabbedObjects.Count - 1]);
                grabbleobjectnear = false;
                grabbleObject = null;
            }
        }
        if (grabbedObjects.Count >= 1) {
            currentGrabbleObject = grabbedObjects[grabbedObjects.Count - 1];
        }
        else {
            currentGrabbleObject = null;
        }
        PickupScreenOff();
    }

    void Fire(InputAction.CallbackContext obj) {
       
        if(currentGrabbleObject!=null){
            gameObject.GetComponent<StarterAssets.ThirdPersonController>().stopMoving=true;
            Attach(currentGrabbleObject.transform,AttachToFront);
            FireCallback?.Invoke(currentGrabbleObject);
        }
           
    }
    void UnFire(InputAction.CallbackContext obj) {
        
        if (currentGrabbleObject != null){
            gameObject.GetComponent<StarterAssets.ThirdPersonController>().stopMoving=false;
            Attach(currentGrabbleObject.transform,AttachTo);
            UnFireCallback?.Invoke(currentGrabbleObject);

        }
           
    }

    void Follow(InputAction.CallbackContext obj){
        if(!playersinRescueArea)
        {
            foreach(Enemy a in overlapedEnemies){
                if(!a.isfollow){
                    a.isfollow=true;
                }
            }
        }else{

            foreach(Enemy a in overlapedEnemies){
                if(a.isfollow){
                    a.isfollow=false;
                }
            }
            RescuedCallback?.Invoke(overlapedEnemies.Count);
            overlapedEnemies.Clear();
            playersinRescueArea=false;
        }
       FollowUpScreenOnOFF(false);
    }

    void Attach(Transform objecttoattach) {
        GrabObjectCallback?.Invoke(grabbleObject);
        objecttoattach.parent= AttachTo.transform;
        objecttoattach.localPosition = new Vector3(0, 0, 0);
        objecttoattach.localRotation = Quaternion.Euler(0,0,0);
        grabbleobjectnear = false;
        grabbleObject = null;
    }

    void Attach(Transform ob,Transform to){

        ob.parent= to;
        ob.localPosition = new Vector3(0, 0, 0);
        ob.localRotation = Quaternion.Euler(0,0,0);
    }

    void Detach(Transform objecttodetach) {
        objecttodetach.parent = null;
    }

    void EnemyTriggedStart(Collider other){
          
         if(other.gameObject.transform.GetComponent<IEnemy>()!=null
         &&!other.gameObject.transform.GetComponent<Enemy>().isfollow
         &&!other.gameObject.transform.GetComponent<Enemy>().inrescueArea)
         {
                 if(overlapedEnemies.Count>=1){
                   
                    if(!overlapedEnemies.Contains(other.gameObject.transform.GetComponent<Enemy>())){
                        overlapedEnemies.Add(other.gameObject.transform.GetComponent<Enemy>());
                    }
                }else{
                    overlapedEnemies.Add(other.gameObject.transform.GetComponent<Enemy>());
               }
           
            FollowUpScreenOnOFF(true);
        }
    }
    void EnemyTriggedExit(Collider other){
       
        if(other.gameObject.transform.GetComponent<IEnemy>()!=null
        &&!other.gameObject.transform.GetComponent<Enemy>().isfollow
        &&!other.gameObject.transform.GetComponent<Enemy>().inrescueArea)
        {
             Debug.Log("UNtrigged"+other);
             if(overlapedEnemies.Count>=1){
                 
               if(overlapedEnemies.Contains(other.gameObject.transform.GetComponent<Enemy>()))
                overlapedEnemies.Remove(other.gameObject.transform.GetComponent<Enemy>());

                
             } 
            FollowUpScreenOnOFF(false);
        }
    }

    void CallwhenEnemyDead(Enemy deadEnemy){

         GameObject removeFromList=null;
                foreach(Enemy a in overlapedEnemies){
                    if(a.gameObject==deadEnemy.transform.gameObject){
                        removeFromList=deadEnemy.transform.gameObject;
                        
                    }else{
                        removeFromList=null;
                    }
                }
                try{
                overlapedEnemies.Remove(removeFromList.transform.GetComponent<Enemy>());

                }catch(Exception  e){Debug.Log(e);}

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.transform.GetComponentInParent<IGrabble>()!=null&& !other.transform.GetComponentInParent<GrabbleObject>().isgrabbed) {
            grabbleobjectnear=true;
           // Debug.Log(other);
            grabbleObject = other.transform.GetComponentInParent<GrabbleObject>();
            grabbleObject.isgrabble = true;
            PickupScreen();
        }else if (other.transform.gameObject.TryGetComponent<RescueArea>(out RescueArea rescueArea)){

            if(overlapedEnemies.Count>=1){
                 foreach(Enemy a in overlapedEnemies)
                 {
                    a.inrescueArea=true;
                    playersinRescueArea=true;
                 }
                 FollowUpScreenOnOFF(true);
            }
        }
       
        
    }



    void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponentInParent<IGrabble>()!=null && !other.transform.GetComponentInParent<GrabbleObject>().isgrabbed)
        {
            grabbleobjectnear = false;
           
            grabbleObject = null;
            other.transform.GetComponentInParent<GrabbleObject>().isgrabble = false;
            PickupScreenOff();

        }else if (TryGetComponent<RescueArea>(out RescueArea rescueArea)){

            if(overlapedEnemies.Count>=1){
                 foreach(Enemy a in overlapedEnemies)
                 {
                    a.inrescueArea=false;
                    playersinRescueArea=false;
                 }
               FollowUpScreenOnOFF(false);  
            }
        }

    }
    public void StopMoving(bool value){
            gameObject.GetComponent<StarterAssets.ThirdPersonController>().stopMoving=value;
    }
}
