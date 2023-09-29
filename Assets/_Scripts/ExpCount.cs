using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//Counts the explosive that have been set to win the game and start the win cinematic.
public class ExpCount : MonoBehaviour
{
    
    public TextMeshProUGUI textExp;
    public int expCount;

 
    // Update is called once per frame
    void Update()
    {
        textExp.text = "" + expCount;
        StartCoroutine(CheckIfWin());
    }

    //Check if all the exlosives are set and hides the alive units for the win cinematics
    IEnumerator CheckIfWin()
    {
        if(expCount > 4)
        {
            yield return new WaitForSeconds(1);
            GameObject soldier1 = GameObject.Find("Soldier1");
            if (soldier1 != null)
            {
                soldier1.SetActive(false);
            }
            GameObject soldier2 = GameObject.Find("Soldier2");
            if (soldier2 != null)
            {
                soldier2.SetActive(false);
            }
            GameObject soldier3 = GameObject.Find("Soldier3");
            if (soldier3 != null)
            {
                soldier3.SetActive(false);
            }
            GameObject soldier4 = GameObject.Find("Soldier4");
            if (soldier4 != null)
            {
                soldier4.SetActive(false);
            }


            SceneManager.LoadScene(3);
        }
      
    }
}
