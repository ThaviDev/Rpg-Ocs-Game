using Doublsb.Dialog;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[DefaultExecutionOrder(-1)]
public class MyDialogueManager : MonoBehaviour
{
    public static MyDialogueManager Instance;

    public const string LENGUAGE_PREF = "LenguageID";

    public const string CHARACTER_DEFAULT = "DEFAULT";

    [SerializeField]
    private DialogManager manager;

    public Languages language;

    private static List<Dialogue> dialoges = new List<Dialogue>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ChangeLanguage(Languages language)
    {
        PlayerPrefs.SetInt(LENGUAGE_PREF, (int)language);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void LoadDialogues()
    {
        int language = PlayerPrefs.GetInt(LENGUAGE_PREF, (int)Languages.Spanish);
        string document = (Resources.Load("Dialogues") as TextAsset).text;

        string[] rows = document.Split('\n');

        for (int i = 1; i < rows.Length; i++)
        {
            dialoges.Add(new Dialogue(rows[i], language));
        }
    }

    public static void Reproduce(int id) 
    {
        List<Dialogue> dialogues = new List<Dialogue>();
        Dialogue dialogue;
        try
        {
            dialogue = dialoges[id];
        }
        catch (System.Exception)
        {
            Debug.LogError("No hay ese dialogo");
            return;
        }

        dialogues.Add(dialogue);
        GetAllNextDialoges(dialogue, dialogues);

        List<DialogData> datas = dialogues.Select((dialogue) => dialogue.DialogData).ToList();
        

        Instance.manager.Show(datas);
    }

    public static void GetAllNextDialoges(Dialogue initialDialogue, List<Dialogue> dialogues)
    {
        if (initialDialogue.next is null) return;
        Dialogue nextDialogue = dialoges[(int)initialDialogue.next];
        dialogues.Add(nextDialogue);
        GetAllNextDialoges(nextDialogue, dialogues);
    }

    public class Dialogue
    {
        public DialogData DialogData;

        public int? next = null;

        public Dialogue(string row, int language, System.Action callback = null, bool isSkippable = true)
        {
            string[] columns = row.Split('\t');
            //Debug.Log(language);
            var dialogue = columns[language - 1];
            string character = columns[1] != string.Empty ? columns[1] : CHARACTER_DEFAULT;

            if (int.TryParse(columns[2], out int result))
            {
                next = result;
            }
            else
                next = null;

            DialogData = new DialogData(dialogue, character, () => callback?.Invoke(), isSkippable);
        }
    }
}


public enum Languages
{
    Spanish = 4,
    English = 5
}
