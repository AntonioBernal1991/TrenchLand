using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Doesnt destroy the object when the scene changes
public class DontDestroy : MonoBehaviour
{
  
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

  
 
}
