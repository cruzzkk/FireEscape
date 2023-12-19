using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireExtingParticles : MonoBehaviour
{
    public static Action<FlamableObject> CollissionTrigger;
    public static Action DisableParticleTrigger;

    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {

    }

    void OnParticleCollision(GameObject other)
    {
       Debug.Log(other);
        if (other.transform.parent.TryGetComponent(out FlamableObject flamableObject))
        {

            CollissionTrigger?.Invoke(flamableObject);
        }
 

    }

    void OnDisable() {
        DisableParticleTrigger?.Invoke();
    }



   
}
