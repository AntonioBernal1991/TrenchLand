using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class ChangeScene: MonoBehaviour, Iinteractable
{
    //When the unit interacts with the object with this script, it changes the scene and silences the Hotwizer misile.
    public GameObject box;

    private static ChangeScene instance = null;
    public static ChangeScene Instance
    {
        get { return instance; }
    }
    private GridPosition gridPosition;
    [SerializeField] private Image transitionPanel;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;
    public bool isHowizer;
    public MeshRenderer rend;
    private GameObject doos;
    public GameObject find;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        isHowizer = true;
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
       

        

    }
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {

            isActive = false;
            onInteractionComplete();
            
        }


    }
    
    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        StartCoroutine(LoadingScene());
        isHowizer = false;
    }

    IEnumerator LoadingScene()
    {
        yield return new WaitForSeconds(1);
        transitionPanel.gameObject.SetActive(true);
        transitionPanel.DOFade(1.0f, 1.0f).WaitForCompletion();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
        find.SetActive(true);
        rend.enabled = false;
   
        
    }

}





