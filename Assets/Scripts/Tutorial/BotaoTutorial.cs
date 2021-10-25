using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BotaoTutorial : MonoBehaviour 
{
    [SerializeField]
    GameObject tutorial;
    Tutorial tutorialScript;

    void Start()
    {
        tutorialScript = tutorial.GetComponent<Tutorial>();
    }

    public void SetAnswerMsg(GameObject button)
    {
        if ("Pular".Equals(button.name))
        {
            tutorialScript.PulouTutorial();
            tutorialScript.PassoTutorial = Tutorial.Passo.PulouTutorial;
        }
        else
        {
            tutorialScript.AnswerMsg = 1;
        }

        tutorialScript.MessageBoxVisEdu(button.transform.parent.name, false);
    }
}
