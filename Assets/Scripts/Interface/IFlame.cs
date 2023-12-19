using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlame  
{
    public bool isContact { get; set; }
    public float contactTime { get; set; }
    public float threshouldTime { get; set; }
    public void Active();
    public void Disable();
}
