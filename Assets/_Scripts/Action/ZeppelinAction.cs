using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class ZeppelinAction : MonoBehaviour
{

    public float speed;
   //Moves zeppelin.
   void Update()
    {
    transform.Translate(Vector3.back * Time.deltaTime * speed);
    }

}
