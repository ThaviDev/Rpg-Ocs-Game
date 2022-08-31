using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScBtl_Motor : MonoBehaviour
{
    public BattleManager BtlMan;
    public UI_Manager UIMan;

    public int m_ui_butSelAbil; // boton de la selección de habilidades
    public int m_bm_actorsSel; // actor siendo seleccionado en UI manager
    public int m_ui_aimedActor; // Personaje seleccionado en la parte de apuntar a un personaje en la interfaz
    public int m_bm_possibleAimedActor;// Posible personaje seleccionado en la parte de apuntar a un personaje en la interfaz
                                       // (cuando tienes que elegir a un personaje para aplicar la habilidad)
    public int m_bm_step; // Pasos de la batalla
    //public int m_btl_actorsSel; // actor siendo seleccionado en battle manager

    void Start()
    {
        BtlMan = GetComponent<BattleManager>();
        UIMan = GetComponent<UI_Manager>();
    }

    void Update()
    {
        
    }
}
