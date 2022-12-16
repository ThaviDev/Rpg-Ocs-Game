using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Btl_UI_Manager : MonoBehaviour
{

    // Variables

    // Posible personaje seleccionado en la parte de apuntar a un personaje en la interfaz
    public int possibleAimedActor;
    // Actor siendo seleccionado
    public int currentActor = 0;
    // Status de cuando el personaje tenga que apuntar a un enemigo
    public int aimStatus;
    // Si el personaje esta seleccionando una habilidad o apuntando a un personaje
    public int actorSelectionStatus;
    // la cantidad de actores que existen en la escena (Dicho por ScBtlMotor)
    public int actorsAmount;
    // la cantidad de aliados que existen en la escena (Dicho por ScBtlMotor)
    public int alliesAmount;
    // la cantidad de enemigos que existen en la escena (Dicho por ScBtlMotor)
    public int enemiesAmount;

    // Elementos

    private ScBtl_Motor sceneBTL_Motor;
    //private Battle_Manager bm; // Battle Manager
    //public int bActor_bm; // Public - Determina la seleccion de habilidades
    // Interfaz de Opciones para la selección de habilidades
    [SerializeField] RectTransform sel_Abilities_Box; // Transform of Sel_Abil
    //public int bActorSel_bm; // Public - Determina la seleccion de enemigo cuando se requiere

    // Botones para seleccionar un actor apuntado
    public Button[] b_SelActor;
    // Posicion para posicionar los botones para seleccionar un actor apuntado
    public RectTransform[] pos_SelActor;
    // Posiciones donde SelAbilities se moveria
    public RectTransform[] pos_SelAbilities; 
    // Lista de motores de la funcionalidad de los actores
    public List<ActorMotor> actorMotors = new List<ActorMotor>();
    // Lista de textos de datos de los actores
    public List<TextMeshProUGUI> dataText = new List<TextMeshProUGUI>();
    // Lista de Sliders que muestra la vida de los actores
    public List<Slider> hpSlider = new List<Slider>();
    // Lista de textos que describen la vida de los actores
    public List<TextMeshProUGUI> hpText = new List<TextMeshProUGUI>();

    // Prefabs
    public GameObject dataTextGM; // Prefab del texto de estadisticas
    public GameObject healthTextGM; // Prefap del texto de vida
    public GameObject healthBarGM; // Prefap de la barra de vida

    void Start()
    {
        // Encontrar battle motor
        sceneBTL_Motor = FindObjectOfType<ScBtl_Motor>();
        // Encontrar Objeto padre de los actores
        GameObject fatherObj_Actors = GameObject.Find("BattleAssets").gameObject.transform.Find("Actors").gameObject;
        GameObject actorsElementsUI = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.Find("ActorsElements").gameObject;



        // Encontrar Todos los actores en escena y añadirlos a la lista de ActorMotors
        for (int i = 0; i < 6; i++)
        {
            actorMotors.Add(fatherObj_Actors.gameObject.transform.Find("Actor" + i).gameObject.GetComponent<ActorMotor>());
            dataText.Add(actorsElementsUI.transform.Find("Data").gameObject.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>());
            hpSlider.Add(actorsElementsUI.transform.Find("HealthBars").gameObject.transform.GetChild(i).gameObject.GetComponent<Slider>());
            hpText.Add(actorsElementsUI.transform.Find("HealthText").gameObject.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>());
        }

        //sel_Abilities.position = new Vector3(1f, -0.5f, 0f); // Pos for char2
        //sel_Abilities.position = new Vector3(1.5f, 3f, 0f); // Pos for char0
        //sel_Abilities.position = new Vector3(3.7f, 1.3f, 0f); // Pos for char1

        // Se posiciona en una posición vacía al inicio del combate
        sel_Abilities_Box.position = new Vector3(15f, 0f, 0f); // Pos for Empty
        //bm = FindObjectOfType<Battle_Manager>();
        possibleAimedActor = -1;
        //bActor_bm = -1;
        //butSelAbil = -1;
        //aimedActor = -1;
        //canAim = false;

        // Actualizar la interfaz para 
        UpdateUI_Data();
        //dataText.Add(dataText[0]);
        //dataText[0].text = "DMG: 1\nHEAL: 1";
        //print(dataText[0].text);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) { currentActor = 0; }
        if (Input.GetKeyDown(KeyCode.T)) { currentActor = 1; }
        if (Input.GetKeyDown(KeyCode.Y)) { currentActor = 2; }
        if (Input.GetKeyDown(KeyCode.U)) { currentActor = -1; }

        if (actorSelectionStatus != 1 && actorSelectionStatus != 0)
        {
            // Apuntador no existe
            for (int i = 0; i < 3; i++)
            {
                b_SelActor[i].gameObject.transform.position = new Vector3(15f, 0f, 0f);
            }
            // Seleccionador de habilidades no existe
            sel_Abilities_Box.position = new Vector3(15f, 0f, 0f);
        } else
        {
            UpdateUI_Selections();
        }
    }

    public void UpdateUI_Data()
    {
        for (int i = 0; i < 6; i++)
        {
            hpSlider[i].value = actorMotors[i].actorsData[3];
            dataText[i].text = "Dmg: " + actorMotors[i].actorsData[4] + "\nRes: " + actorMotors[i].actorsData[5] + "\nHeal:" +
                actorMotors[i].actorsData[6] + "\nSpd: " + actorMotors[i].actorsData[7];
            hpText[i].text = "" + actorMotors[i].actorsData[3];
        }
    }

    public void UpdateUI_Selections() {
        // Actor está seleccionando una habilidad
        //print(actorsAmount + " y " + alliesAmount);
        if (actorSelectionStatus == 0)
        {
            // Apuntador no existe para que el otro si exista
            for (int i = 0; i < 3; i++)
            {
                b_SelActor[i].gameObject.transform.position = new Vector3(15f, 0f, 0f);
            }

            // Si hay un actor en este momento mover la caja a esa posición
            if (currentActor != -1)
            {
                // Mover la posición de la seleccion de habilidades al numero del actor siendo seleccionado
                sel_Abilities_Box.position = pos_SelAbilities[currentActor].position;
            }
            else
            {
                // Mover la posicion a un lugar "inexistente"
                sel_Abilities_Box.position = new Vector3(15f, 0f, 0f);
            }
        }
        // Actor está apuntando a un personaje posible
        else if (actorSelectionStatus == 1)
        {
            // Seleccionador de habilidades no existe para que el otro si exista
            sel_Abilities_Box.position = new Vector3(15f, 0f, 0f);

            // Si posible punteria es 0 entonces apunto a los aliados y no esta actuando
            if (aimStatus == 3)
            {
                // UI: Apunto a los aliados
                for (int i = 0; i < 3; i++)
                {
                    b_SelActor[i].gameObject.transform.position = pos_SelActor[i].position;
                }
            }
            // Si posible punteria es 1 entonces apunto a los enemigos y no esta actuando
            else if (aimStatus == 4)
            {
                // Apunto a todos los enemigos
                for (int i = 0; i < 3; i++)
                {
                    b_SelActor[i].gameObject.transform.position = pos_SelActor[i + 3].position;
                }
            }
            // Si ninguna de las anteriores condiciones son verdaderas entonces no apunto a ningun lado
            else
            {
                // UI: Apunto a ningun lado
                for (int i = 0; i < 3; i++)
                {
                    b_SelActor[i].gameObject.transform.position = new Vector3(15f, 0f, 0f);
                }
            }
        }
    }

    public void BtnSelection(int value)
    {
        sceneBTL_Motor.AbilInput(value);
    }

    public void BtnActor(int value)
    {
        sceneBTL_Motor.AimInput(value);
    }
    /*

    public void ButtonSelection1()
    {
        // Se presiono el boton 1
        sceneBTL_Motor.AbilInput(0);
    }
    public void ButtonSelection2()
    {
        // Se presiono el boton 2
        sceneBTL_Motor.AbilInput(1);
    }
    public void ButtonSelection3()
    {
        // Se presiono el boton 3
        sceneBTL_Motor.AbilInput(2);
    }

    public void ButtonActor1()
    {
        // Se presiono el boton 1
        sceneBTL_Motor.AimInput(0);
    }
    public void ButtonActor2()
    {
        // Se presiono el boton 2
        sceneBTL_Motor.AimInput(1);
    }
    public void ButtonActor3()
    {
        // Se presiono el boton 3
        sceneBTL_Motor.AimInput(2);
    }
    */
}
