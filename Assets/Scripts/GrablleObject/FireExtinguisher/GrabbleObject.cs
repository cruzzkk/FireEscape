using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbleObject : MonoBehaviour , IGrabble
{
    
    public bool isgrabble { get; set; }
    public bool isgrabbed { get; set; }
    public Rigidbody rb;
    public GameObject triggerObject;
    public GameObject fog;
    void OnEnable() {
        Player.GrabObjectCallback += Attach;
        Player.FireCallback += Fire;
        Player.UnFireCallback += UnFire;
        Player.DropObjectCallback += Remove;
    }
    void OnDisable() {

        Player.GrabObjectCallback -= Attach;
        Player.FireCallback -= Fire;
        Player.UnFireCallback -= UnFire;
        Player.DropObjectCallback -= Remove;
    }
    void Start() {
        fog.SetActive(false);
        isgrabble = false;
        isgrabbed=false;
        triggerObject.SetActive(true);
        if (GetComponent<Rigidbody>()) { Destroy(GetComponent<Rigidbody>()); }
        
    }
    public void Attach(GrabbleObject ob) {
        if (ob == this) {
            fog.SetActive(false);
            if (GetComponent<Rigidbody>() != null) {
                rb = null;
                Destroy(GetComponent<Rigidbody>());
            }
            triggerObject.SetActive(false);
            isgrabbed=true;
        }
    }
    public void Remove(GrabbleObject ob) {
        if (ob == this)
        {
            fog.SetActive(false);
            if (GetComponent<Rigidbody>() == null)
            {
                rb = this.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
                rb.isKinematic = false;
                triggerObject.SetActive(true);
            }
             isgrabbed=false;
        }
    }
    public void Fire(GrabbleObject ob) {
        if (ob == this)
            fog.SetActive(true);

    }
    public void UnFire(GrabbleObject ob) {
        if (ob == this)
            fog.SetActive(false);
    }
}
