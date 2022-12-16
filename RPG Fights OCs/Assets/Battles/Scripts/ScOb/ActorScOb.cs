using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ActorScOb : ScriptableObject
{
    public RuntimeAnimatorController animControl;
    public int actorID;
    public bool isEnemy;
    //public int[] stadistics = new int[5] {0,0,0,0,0};
    /*
    Estadisticas de los personajes:
    0 - Primera Habilidad , 1 - Segunda Habilidad, 2 - Tercera Habilidad
    3 - Puntos de Vida, 4 - Puntos de Daño
    */
    /*
    public int[] ability1;
    public int[] ability2;
    public int[] ability3;
    public int maxHealth;
    public int startingHealth;
    public int startingSpeed;
    public int damageModif;
    public int healingModif;
    public int accelSpeedModif;
    public int desaccelSpeedModif;
    public int precisionModif;
    public int critModif;
    */

    public float[] actorsData = new float[11];
    // Actors data
    /*
     * 0 - vida Maxima
     * 1 - vida Inicial
     * 2 - velocidad Inicial
     * 3 - modif de Danio (Resistencia al danio)
     * 4 - modif de Curacion
     * 5 - modif de aceleracion (aumento de velocidad)
     * 6 - modif de desaceleracion (desenzo de velocidad)
     */
    // Abils data
    /*
     * 0 - turnos de la habilidad
     * 1 - posible punteria
     * 2 - hay confrontación
     * 3 - cantidad de cooldown
     */
    // Actions data
    /*
     * 0 - clasificacion de la accion 
     * (Vida, Velocidad, ModifDanio, ModifCuracion
     * AcelVelocidad, DesacVelocidad, ModifPrecision
     * ModifRandomCritical, MaximaVida)
     * 1 - tipo de aplicacion
     * 2 - cantidad que se aplicara
     * 3 - turno para aplicar
     */

    public AbilityScOb[] abilities = new AbilityScOb[3];
    /*
    public int[] actorAbility1 = new int[6];
    public int[] actorAbility2 = new int[6];
    public int[] actorAbility3 = new int[6];

    public int[] action1OfAbil1 = new int[4];
    public int[] action2OfAbil1 = new int[4];
    public int[] action3OfAbil1 = new int[4];
    public int[] action1OfAbil2 = new int[4];
    public int[] action2OfAbil2 = new int[4];
    public int[] action3OfAbil2 = new int[4];
    public int[] action1OfAbil3 = new int[4];
    public int[] action2OfAbil3 = new int[4];
    public int[] action3OfAbil3 = new int[4];
    */
}
