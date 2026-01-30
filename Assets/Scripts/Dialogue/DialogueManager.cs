using UnityEngine;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] public GameObject _dialogueBox;
    public bool _justStarted;

    [SerializeField] int _currentLine;
    [SerializeField] TMP_Text _dialogueText;
    [SerializeField] TMP_Text _nameText;
    [SerializeField] GameObject _nameBox;
    string[] _dialogueLines;
    const string _startsWithSignifierString = "n-";


    public void ContinueDialogue()
    {
        if (!_justStarted)
        {
        _currentLine++;
            if (_currentLine >= _dialogueLines.Length)
            {
                _dialogueBox.SetActive(false);
                _justStarted = true;
                //PlayerController.Instance.canMove = true;
                //PlayerController.Instance.canAttack = true;
            }
            else
            {
                CheckIfName();
                _dialogueText.text = _dialogueLines[_currentLine];
            }
        }
        else
        {
            _justStarted = false;
        }
    }
    

    // newLines is passed through from the DialogueActivator class that calls this function
    public void ShowDialogue(string[] newLines, bool isPerson) 
    {
        _justStarted = true;
        _dialogueLines = newLines;
        _currentLine = 0;
        CheckIfName();
        _dialogueText.text = _dialogueLines[_currentLine];
        _dialogueBox.SetActive(true);
        _nameBox.SetActive(isPerson);
        //PlayerController.Instance.canMove = false;
        ContinueDialogue();
    }


    // Can signify who's talking in the inspector
    public void CheckIfName() 
    {
        if (_dialogueLines[_currentLine].StartsWith(_startsWithSignifierString)) 
        {
            _nameText.text = _dialogueLines[_currentLine].Replace(_startsWithSignifierString, "");
            _currentLine++;
        }
    }
}