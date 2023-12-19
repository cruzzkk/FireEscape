using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Enemy : MonoBehaviour,IEnemy
{
    public static Action<Collider> EnemyTriggerCallback;
    public static Action<Collider> EnemyTriggerExitCallback;
    public static Action<Enemy> EnemyDeathCallback;
    public bool inrange {get;set;}
    public bool isfollow {get;set;}
    public bool inrescueArea {get;set;}
    public int health {get;set;}
    public Transform targettoFollow;
    public float inrangeDistance;
    NavMeshAgent agent;
    Animator anim;
   
    void Start()
    {
        health=20;
        targettoFollow = GameObject.FindObjectsOfType<Player>()[0].transform;
        agent = GetComponent<NavMeshAgent>();
        anim=GetComponent<Animator>();
    }
    
    void Update()
    {
       anim.SetBool("inRescueArea",inrescueArea); 
        anim.SetBool("isFollow",isfollow); 
        if(isfollow){

            var distance = Vector3.Distance(targettoFollow.position, transform.position);
        
            if(distance <= inrangeDistance) {
                Move(targettoFollow);
            }
            else  {
            StopMoving();
            
            }
        }

       if(health<=0)
       {
        Dead();
       }
       
    }
   public void Move(Transform targettoFollow)
   {
        anim.SetFloat("Speed",agent.velocity.magnitude);
        inrange=true;
        agent.enabled = true;
        agent.SetDestination (targettoFollow.position);
   }
    public void StopMoving()
    {
        inrange=false;
        this.agent.enabled = false;
    }

    public void Dead(){
        EnemyDeathCallback?.Invoke(this);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.TryGetComponent<Player>(out Player player))
            EnemyTriggerCallback?.Invoke(this.GetComponent<CapsuleCollider>());

        
    }
     void OnTriggerExit(Collider other){
         if (other.gameObject.TryGetComponent<Player>(out Player player))
             EnemyTriggerExitCallback?.Invoke(this.GetComponent<CapsuleCollider>());
     }

       void OnParticleCollision(GameObject other)
    {
       Debug.Log("In");
        if (other.transform.parent.TryGetComponent(out FlamableObject flamableObject))
        {

            health-=1;
        }
 

    }

    void OnFootstep(){}
   
}
