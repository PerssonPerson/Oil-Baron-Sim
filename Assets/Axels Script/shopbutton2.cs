using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopbutton2 : MonoBehaviour //Axel
{
    //Scriptet best�mmer vad knapparna i shopen (telefonen) g�r 

    public Button button;




    void Start()
    {

        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);


    }



    private void Update()
    {



    }
    void TaskOnClick()
    {
        FindObjectOfType<GameManager>().money += 30;
        FindObjectOfType<GameManager>().sustain -= 30;
        FindObjectOfType<GameManager>().reputation -= 30;
    }

}
