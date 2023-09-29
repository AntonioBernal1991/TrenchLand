using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Explodes columns in the win cinematic

public class DestructibleColumn : MonoBehaviour
{
    public Transform center;
    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        rigid.AddExplosionForce(600f, center.position, 30f);
    }

 
    
}
