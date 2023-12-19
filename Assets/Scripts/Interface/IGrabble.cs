using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabble  
{
    public bool isgrabble { get; set; }
    public bool isgrabbed { get; set; }
    public void Attach(GrabbleObject grabbleObject);
    public void Remove(GrabbleObject grabbleObject);
    public void Fire(GrabbleObject grabbleObject); public void UnFire(GrabbleObject grabbleObject);
}
