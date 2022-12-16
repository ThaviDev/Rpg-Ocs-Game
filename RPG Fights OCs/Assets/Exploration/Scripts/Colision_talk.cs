using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision_talk : MonoBehaviour
{
    public int dialogueID; // Numero que dicta que dialogo va a ser el que se va a ejecutar

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MyDialogueManager.Reproduce(dialogueID);
        Acting_Manager am = FindObjectOfType<Acting_Manager>();
        am.StartActing(dialogueID);
        //Acting_Manager.StartActing(dialogueID);
    }
}
