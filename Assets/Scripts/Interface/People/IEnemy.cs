using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy  
{
   public bool inrange {get;set;}
   public bool isfollow {get;set;}
   public bool inrescueArea{get;set;}
   public int health {get;set;}
    
   public void Move(Transform targettoFollow);
   public void StopMoving();
   public void Dead();
}
