using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ActorScOb : ScriptableObject
{
    public Animator anim;
    public string actorName;
    //public int[] stadistics = new int[5] {0,0,0,0,0};
    /*
    Estadisticas de los personajes:
    0 - Primera Habilidad , 1 - Segunda Habilidad, 2 - Tercera Habilidad
    3 - Puntos de Vida, 4 - Puntos de Daño
    */
    public int[] ability1;
    public int[] ability2;
    public int[] ability3;
    public int maxHealth;
    public int startingSpeed;
}
