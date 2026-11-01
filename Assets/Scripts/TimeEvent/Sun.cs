using System;
using UnityEngine;

public class Sun : MonoBehaviour
{

    [SerializeField] public TimeCycle TimeCycle;
    [SerializeField] public Light light;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.forward, TimeCycle.dtime / TimeCycle.timeForDayCycle * 360);
        
        var currentRotation = transform.rotation.eulerAngles.x;
        if (currentRotation >= 150 || currentRotation <= 30)
        {
            light.colorTemperature = 1500;
        }
        else
        {
            if (currentRotation <= 90)
            {
                light.colorTemperature = Mathf.Lerp(1500f, 6000f, 
                    Scale(currentRotation, new Vector2(30, 90), new Vector2(0,1)));
            }
            else
            {
                light.colorTemperature = Mathf.Lerp(6000f, 1500f, Scale(currentRotation, new Vector2(90, 150), new Vector2(0,1)));
            }
        }
    }

    float Scale(float val, Vector2 src,Vector2 dst)
    {
        return ((val - src.x) / (src.y - src.x)) * (dst.y - dst.x) + dst.x;
    }

}
