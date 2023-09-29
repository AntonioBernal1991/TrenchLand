using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Find all units on the scene so they can be transport to the new scene
public class FindPlayer : MonoBehaviour
{

    public List<Unit> soldiers;
    private int f;

    void Start()
    {
        
        DontDestroyOnLoad(this.gameObject);
       for(int i = 0; i <= 3;i++)
        {
           
            f += 4; 

           if (soldiers[i] != null)
            {
                soldiers[i].gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 
                    this.gameObject.transform.position.y, this.gameObject.transform.position.z + f );
            }
        }
    
    }
}
