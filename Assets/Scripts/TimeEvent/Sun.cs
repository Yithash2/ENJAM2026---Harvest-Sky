using System;
using UnityEngine;

public class Sun : MonoBehaviour
{

    [SerializeField] public TimeCycle TimeCycle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.right, TimeCycle.dtime / TimeCycle.timeForDayCycle * 360);
    }
}
