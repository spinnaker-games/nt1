using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Put on gameobjects that can be toggled with opening the dialogue window (currently spacebar).  If isPerson isn't toggled true, the name box window will not appear.
public class DialogueActivator : MonoBehaviour
{
    public string[] lines;
    public bool isPerson;

    [SerializeField] GameObject buttonUI;
    bool canActivate;
    InputActions _inputActions;
    const string playerString = "Player";


    void Awake() 
    {
        _inputActions = new InputActions();
    }


    void OnEnable() 
    {
        _inputActions.Enable();
    }


    void OnDisable() 
    {
        _inputActions.Disable(); 
    }


    void Start()
    {
        _inputActions.Player.Interact.performed += _ => OpenDialogue();
    }


    void OpenDialogue() 
    {
        if (canActivate) 
        {
            if(!DialogueManager.Instance.dialogueBox.activeInHierarchy) 
            {
                DialogueManager.Instance.ShowDialogue(lines, isPerson);
                //PlayerController.Instance.canAttack = false;
                //PlayerController.Instance.DialogueStopMove();
            } 
            else 
            {
                DialogueManager.Instance.ContinueDialogue();
            }
        }
    }


    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == playerString) 
        {
            buttonUI.SetActive(true);
            canActivate = true;
        }
    }


    void OnTriggerExit(Collider other) 
    {
        if(other.tag == playerString) 
        {
            buttonUI.SetActive(false);
            canActivate = false;
        }
    }
}