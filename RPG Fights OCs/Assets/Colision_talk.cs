using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision_talk : MonoBehaviour
{
    public int dialNumberIdentification; // Numero que dicta que dialogo va a ser el que se va a ejecutar y la identificación del personaje
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MyDialogueManager.Reproduce(3);
    }
}
