using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Manager : MonoBehaviour
{
    [SerializeField] RectTransform sel_Abilities; // Transform of Sel_Abil
    public RectTransform[] pos_SelAbilities; // Posiciones donde SelAbilities se moveria
    private Battle_Manager bm; // Battle Manager
    public int bActor_bm; // Public - Determina la seleccion de habilidades
    public int bSel_bm; // Public - Determina la seleccion de habilidades
    public int bActorSel_bm; // Public - Determina la seleccion de enemigo cuando se requiere
    public Slider[] hpActor_bm; // Public - Slider de la vida de los actores
    public Button[] b_SelActor; // Public - Boton para seleccionar un actor
    public RectTransform[] pos_SelActor; // Public - posicion para elegir a un actor

    public int actorSel_State_bm; // Public - Decide si se puede seleccionar a los aliados o enemigos o ninguno
    void Start()
    {
        //sel_Abilities.position = new Vector3(1f, -0.5f, 0f); // Pos for char2
        //sel_Abilities.position = new Vector3(1.5f, 3f, 0f); // Pos for char0
        //sel_Abilities.position = new Vector3(3.7f, 1.3f, 0f); // Pos for char1
        sel_Abilities.position = new Vector3(15f, 0f, 0f); // Pos for Empty
        bm = FindObjectOfType<Battle_Manager>();
        bSel_bm = -1;
        actorSel_State_bm = 0;
        bActor_bm = -1;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bm.selAbilCharPosition_uiMan_gm == 0){
            sel_Abilities.position = pos_SelAbilities[0].position;
            //sel_Abilities.position = new Vector3(1.5f, 3f, 0f); // Pos for char0
        }
        else if (bm.selAbilCharPosition_uiMan_gm == 1){
            sel_Abilities.position = pos_SelAbilities[1].position;
            //sel_Abilities.position = new Vector3(3.7f, 1.3f, 0f); // Pos for char1
        } 
        else if (bm.selAbilCharPosition_uiMan_gm == 2){
            sel_Abilities.position = pos_SelAbilities[2].position;
            //sel_Abilities.position = new Vector3(1f, -0.5f, 0f); // Pos for char2
        }
        else {
            sel_Abilities.position = new Vector3(15f, 0f, 0f); // Pos for Empty
        }

        if (actorSel_State_bm == 1){
            for (int i = 0; i < 3; i++)
            {
                b_SelActor[i].gameObject.transform.position = pos_SelActor[i].position;
            }
        }
        else if (actorSel_State_bm == 2){
            for (int i = 0; i < 3; i++)
            {
                b_SelActor[i + 3].gameObject.transform.position = pos_SelActor[i + 3].position;
            }
        } else {
            for (int i = 0; i < 6; i++)
            {
                b_SelActor[i].gameObject.transform.position = new Vector3(15f,0f,0f);
            }
        }

        /*
        - Volver a checar la habilidad de hacer algo cuando el mouse esta encima de un boton
        - Botones para las habilidades
            - Obtener que habilidades son cada boton dependiendo de charStadistics de Battle_Manager
            - Cambiar el texto y el color dependiendo de la habilidad
        
        - Botones de Seleccion de personajes
            - Si no hay seleccion de personajes que las posiciones de los botones sea fuera del escenario
            - Encontrar y acomodar las posiciones de los botones cuando son seleccionables
            - Separarlos por Chars y Enems cuando Battle_Manager lo necesite
        */
    }

    public void ButtonSelection1(){
        // Se presiono el boton 1
        bSel_bm = 0;
    }
    public void ButtonSelection2(){
        // Se presiono el boton 2
        bSel_bm = 1;
    }
    public void ButtonSelection3(){
        // Se presiono el boton 3
        bSel_bm = 2;
    }

    public void ButtonActor1(){
        // Se presiono el boton 1
        bActor_bm = 0;
    }
    public void ButtonActor2(){
        // Se presiono el boton 2
        bActor_bm = 1;
    }
    public void ButtonActor3(){
        // Se presiono el boton 3
        bActor_bm = 2;
    }
}
