using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueArea : MonoBehaviour
{
    List<Enemy> listofenemyINRescued=new List<Enemy>();
    public int GetNumber_enemyRescued{get; set ;}

     void OnEnable(){
        Player.RescuedCallback+=SetNumberEnemyRescued;
     }

    void OnTriggerEnter(Collider other){
         if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy)&&enemy.inrescueArea){
                if(listofenemyINRescued.Count>=1){
                    GameObject needtoAdd=null;
                    foreach(Enemy a in listofenemyINRescued){
                        if(a.gameObject==enemy.gameObject){
                            needtoAdd=null;
                        }else{
                            needtoAdd=enemy.gameObject;
                        }
                    }
                    if(needtoAdd!=null)
                        listofenemyINRescued.Add(needtoAdd.transform.gameObject.GetComponent<Enemy>());
                }else{
                    listofenemyINRescued.Add(enemy);
                }
               
         }
    }
    void SetNumberEnemyRescued(int count){
       GetNumber_enemyRescued+=count; 
    }
}
