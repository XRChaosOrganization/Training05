using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillComponent : MonoBehaviour
{
    public float rotationSpeed;
    
    void Update()
    {
       
        transform.RotateAround(transform.position,Vector3.forward, Time.deltaTime * rotationSpeed);
    }
}
