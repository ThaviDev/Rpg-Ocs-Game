using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class AbilityScOb : ScriptableObject
{
    public float[] ability; // Las características de la habilidad Siendo ejecutada en el presente
    /* elementos
    0 - Tipo de habilidad == 1 - Ataque, 2 - Curacion, 3 - Cambio de Velocidad, 4 - Cambio de Danio
    1 - Cantidad de efecto == *Numero de danio afectado, o numero de curacion, [depende del tipo de habilidad]*
    2 - Turnos necesitados == 0 - Es inmediato, 1 - Un turno extra en hacerse, 2 - Dura dos turnos en hacerse, 3 - dura tres turnos, 4...
    3 - Hay confrontacion == 0 - No, 1 - Si 
    4 - Duracion del efecto == 0 - No hay efecto(es inmediato), 1 - Dura un turno, 2 - Dura dos turnos... etc.
    5 - Tiempo del efecto == 20 - Dura veinte segundos la animacion de la habilidad
    6 - Metodo de ecuacion de la habilidad == 0 - Suma, 1 - Multiplicacion, 2 - Division
    7 - Efecto Secundario == 0 - No hay efecto Secundario, 1 - Ataque, 2 - Curacion, 3 - Cambio de Velocidad, 4 - Cambio de Danio
    8 - Objetivo del efecto Secundario == 0 = Targeted char1, 1 = Tageted char2, 2 - char3, 3 - enem1, 4 - enem2, 5 - enem3, 6 - Todos los aliados, 7 - Todos los Enemigos, 8 Él mismo
    9 - Cantidad de efecto secundario == *Numero de danio afectado, o numero de curacion, [depende del tipo de habilidad]*
    10 - Efecto Terciario == 0 - No hay efecto Secundario, 1 - Ataque, 2 - Curacion, 3 - Cambio de Velocidad, 4 - Cambio de Danio
    11 - Cantidad de efecto Terciario == *Numero de danio afectado, o numero de curacion, [depende del tipo de habilidad]*
    12 - Objetivo del efecto Terciario == 0 = Targeted char1, 1 = Tageted char2, 2 - char3, 3 - enem1, 4 - enem2, 5 - enem3, 6 - Todos los aliados, 7 - Todos los Enemigos, 8 Él mismo
    */
    /*
    public float typeAbility;
    public float abilityQuantity;
    public float neededTurns;
    public float isThereConfrontacion;
    public float effectDuration;
    public float actionTime;
    public float typeAbilitySecund;
    public float secundTarget;
    public float secundAbilityQuantity;
    public float typeAbilityThird;
    public float thirdTarget;
    public float thirdAbilityQuantity;
    */
    //public float 
}
