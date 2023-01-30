using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class AbilityScOb : ScriptableObject
{
    public int actorID; // Para identificar a cual actor perteneces
    public int abilitySelectionSlot; // en cual de los 3 espacios de seleccion de habilidad va esta habilidad
    public float[] abilityData = new float[8]; // Las características de la habilidad Siendo ejecutada en el presente
    public int actionAmount; // La cantidad de acciones que va a tener esta habilidad
    /*
     * 0 - turnos de la habilidad
     * 1 - precision
     * 2 - criticos aleatorios
     * 3 - es apuntable (1 si, 0 no)
     * 4 - hay confrontación (1 si, 0 no)
     * 5 - cantidad de cooldown (en turnos)
     * 6 - Objetivo Posible (0 al enemigo, 1 al aliado, 2 a uno mismo)
     */
    public int[] actionObjective = new int[1];
    public int[] classification = new int[1];
    public int[] applic_Type = new int[1];
    public float[] quantity = new float[1];
    public int[] applic_Turn = new int[1];
    public int[] duration = new int[1];

    // Condiciones para que se aplique la accion
    public int conditionalSuccesionAmount;

    public int[] conditionID = new int[1];
    public int[] waitDuration = new int[1];
    public int[] actionNumber = new int[1];

    //public int[] action1 = new int[4];
    /*
     * 0 - clasificacion de la accion (Suma o resta 0, Multiplicacion o division 1)
     * 1 - tipo de aplicacion
     *     0 Curacion - Danio
     *     1 Aumento vida maxima - Reduccion de vida maxima
     *     2 Mayor velocidad - Menor velocidad
     *     3 Mayor Resistencia al daño - Menor Resistencia al daño
     *     4 Aumento de daño - Reduccion de daño
     *     5 Aumento curacion - Reduccion curacion
     *     6 Aumento mayor velocidad - Reduccion mayor velocidad
     *     7 Aumento menor velocidad - Reduccion menor velocidad
     *     8 Dormir - Despertar
     *     9 Muerte instantanea - Resurección
     *     10 Aturdir - Reaccionar
     *     11 Aumento de duracion de cooldowns - Reduccion de duracion de cooldowns
     *     12 Restauracion cooldowns
     *     13 Aumento de recovery - Reduccion de recovery (cantidad de turnos que dura la habilidad)
     *     14 Mayor cantidad de criticos - Menor cantidad de criticos
     *     15 Mayor precision - Menor precision
     *     
     * 2 - cantidad que se aplicara
     * 3 - turno para aplicar (de la cantidad de turnos de la habilidad en cual de ellos se ejecuta la accion)
     */
    //public int[] action2 = new int[4];
    //public int[] action3 = new int[4];


}
