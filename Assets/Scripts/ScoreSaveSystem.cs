using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CI.QuickSave;
using System;

public class ScoreSaveSystem : MonoBehaviour
{
    public string saveName;
    public float test;
    public Text loadText;
    
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private Transform[] obj;
    [SerializeField]
    private List<Button> buttons = new List<Button>();
    public string[] files;

    private void Awake()
    {
        content = this.gameObject;
        obj = content.GetComponentsInChildren<Transform>();

        for (int i = 0; i < obj.Length; i++)
        {
            if (obj[i].GetComponentInChildren<Button>() != null)
                buttons.Add(obj[i].GetComponentInChildren<Button>());
        }

        buttons.RemoveAt(0);

        files = new string[buttons.Count];


    }
    private void Start()
    {
        //files.Insert(4, ".");
        SetButtonTexts();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.D))
        {
            Delete();
        }
#endif
    }


    public void Delete()
    {
        QuickSaveRaw.Delete("s.json");
    }

    public void Save(int index)
    {
        QuickSaveWriter.Create("s")
                        .Write(saveName + index, test)
                        .Commit();

        Debug.Log("saved!" + index);
        Debug.Log(QuickSaveRaw.LoadString("s.json"));
    }

    public void Load(int index)
    {
        if (QuickSaveRaw.Exists("s.json"))
        {
            float temp = 0;
            QuickSaveReader.Create("s")
                           .Read<float>(saveName + index, (r) => { temp = r; });

            Debug.Log("Loaded!" + index);
            loadText.text = temp.ToString();
        }
        else
        {
            Debug.Log("no data");
        }
    }

    void SetButtonTexts()
    {
        if (QuickSaveRaw.Exists("s.json") == true)
        {
            //seta o tamanho da lista
            // files = new List<string>(new string[length -1]);

            //pega o save e coloca numa string 
            string t = QuickSaveRaw.LoadString("s.json");
            Debug.Log(t);

            // filtro para pegar apenas as palavras certas
            char[] separators = new char[] { '"', ':', /*'.',*/ '{', '}' };

            //com o filtro, separa a string em palavras e coloca numa array
            string[] words = t.Split(separators, StringSplitOptions.RemoveEmptyEntries);


            foreach (string word in words)
            {
                Debug.Log(word);
                //pego as palavras corretas do save
                if (word.Contains(saveName))
                {
                    //separa as palavras em letras
                    for (int i = 0; i < word.Length; i++)
                    {
                        //checa o digito da palavra
                        if (char.IsDigit(word[i]))
                        {
                            Debug.Log(word[i]);
                            string n = word[i].ToString();
                            Debug.Log(n + "essa caralha aqui");
                            //com o digito, preencho o index certo em files
                            files[int.Parse(n)] = ".";
                        }
                    }
                }
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                for (int j = 0; j < files.Length; j++)
                {
                    if (files[j] == ".")
                    {
                        //faz load no save
                        float temp = 0;
                        QuickSaveReader.Create("s")
                                        .Read<float>(saveName + j, (r) => { temp = r; });
                        //coloca o load no lugar certo
                        TimeSpan timeSpan = TimeSpan.FromSeconds(temp);
                        string niceTime = string.Format("{0}:{1}:{2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
                        buttons[j].gameObject.GetComponentInChildren<Text>().text = "Save " + j + ": " + niceTime;

                    }
                    else
                    {
                        // se o slot nao tiver save, ent coloca nada
                        buttons[i].gameObject.GetComponentInChildren<Text>().text = "Save " + i + ": empty slot";
                    }
                }

            }
        }
        else
        {
            for (int i = 0; i < buttons.Count; i++)
            {

                buttons[i].gameObject.GetComponentInChildren<Text>().text = "Save " + i + " : empty slot";
            }
        }
    }
}
