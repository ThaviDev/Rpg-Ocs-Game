using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationCol : MonoBehaviour
{
    public bool coliderCheck = false; // Numero que dicta que dialogo va a ser el que se va a ejecutar
    private void OnCollisionEnter2D(Collision2D collision)
    {
        coliderCheck = true;
    }
}
