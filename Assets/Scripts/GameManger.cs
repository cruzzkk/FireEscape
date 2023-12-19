using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    [SerializeField]
    int totalEnemies,savedEnemies;
    [SerializeField]
    RescueArea rescueArea;
    List<Enemy> enemyList=new List<Enemy>();
    [SerializeField]
    TMPro.TextMeshProUGUI totalEnemiesText,savedEnemiesText;

    [SerializeField]
    GameObject mainPanel,inGamePanel,player;
    
    
      void Start()
      {
          mainPanel.SetActive(true);
          inGamePanel.SetActive(false);
          for (int j = 0; j < inGamePanel.transform.childCount; j++)
            {
                inGamePanel.transform.GetChild(j).gameObject.SetActive(false);
            }

          enemyList.AddRange(FindObjectsOfType<Enemy>());
          totalEnemies=enemyList.Count;
          Debug.Log(enemyList.Count);
      }
      void Update(){
          savedEnemies=rescueArea.GetNumber_enemyRescued;
          totalEnemiesText.text="Peoples in Area: "+totalEnemies;
          savedEnemiesText.text="Saved Peoples: "+savedEnemies;

         if (Input.GetKeyDown(KeyCode.Escape)){
          player.GetComponent<StarterAssets.StarterAssetsInputs>().cursorLocked=false;
          player.GetComponent<StarterAssets.StarterAssetsInputs>().cursorInputForLook=false;
          Cursor.lockState = CursorLockMode.None;
          mainPanel.SetActive(true);
          inGamePanel.SetActive(false);
          for (int j = 0; j < inGamePanel.transform.childCount; j++)
            {
                inGamePanel.transform.GetChild(j).gameObject.SetActive(false);
            }
         }

      }
      public void StartGame(){
           
      player.GetComponent<StarterAssets.StarterAssetsInputs>().cursorLocked=true;
      player.GetComponent<StarterAssets.StarterAssetsInputs>().cursorInputForLook=true;
      Cursor.lockState = CursorLockMode.Locked;

      }
      public void ResetGame(){
        SceneManager.LoadScene(0);
      }
      public void StopPlayerMove(bool value){
        player.GetComponent<Player>().StopMoving(value);
      }

}
