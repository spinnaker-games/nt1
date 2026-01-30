using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Put on gameobjects that can be toggled with opening the dialogue window (currently spacebar).  If isPerson isn't toggled true, the name box window will not appear.
public class DialogueActivator : MonoBehaviour
{
    public string[] _lines;
    public bool _isPerson;

    [SerializeField] GameObject _buttonUI;
    bool _canActivate;
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
        if (_canActivate) 
        {
            if(!DialogueManager.Instance._dialogueBox.activeInHierarchy) 
            {
                DialogueManager.Instance.ShowDialogue(_lines, _isPerson);
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
            _buttonUI.SetActive(true);
            _canActivate = true;
        }
    }


    void OnTriggerExit(Collider other) 
    {
        if(other.tag == playerString) 
        {
            _buttonUI.SetActive(false);
            _canActivate = false;
        }
    }
}