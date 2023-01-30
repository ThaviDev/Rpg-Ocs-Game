using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision_talk : MonoBehaviour
{
    public int[] dialogueID; // Numero que dicta que dialogo va a ser el que se va a ejecutar
    public int dialogueInteractions = 0; // Numero de veces que se a interactuado con el personaje

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MyDialogueManager.Reproduce(dialogueID[dialogueInteractions]);

        Acting_Manager am = FindObjectOfType<Acting_Manager>();
        am.StartActing(dialogueID[dialogueInteractions]);

        // Si hay mas dialogos posibles entonces aumenta la cantidad de interacciones para el siguiente dialogo
        if (dialogueID.Length < dialogueInteractions)
            dialogueInteractions++;
        //Acting_Manager.StartActing(dialogueID);
    }
}
