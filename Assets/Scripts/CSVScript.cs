using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVScript : MonoBehaviour
{
    public List<string> LinhasLista = new List<string>() { "tempo_jogo,projetil,tempo_pouso,qtd_baloes_acertados,forca,distancia" };
    public PlayerScript PlayerScript;
    private SaveCSV SaveCSV;

    // Start is called before the first frame update
    void Start()
    {
        SaveCSV = GetComponent<SaveCSV>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addNewLine(int p, int TP, int QBA, int F, int dis)
    {

        int tj = (int)PlayerScript.tempo_jogo;// Adicionando o tempo de jogo

        string linha  = tj + "," + p + "," + TP + "," + QBA + "," + F + "," + dis + "\n";
        
        LinhasLista.Add(linha);
    }

    public void saving(bool escolherDiretorio = false)
    {
        string linhas = "";

        int i = 0;

        foreach (string line in LinhasLista)
        {
            // Adicionando um \n caso seja a primeira linha
            linhas += i == 1 ? "\n" + line : line;
            
            
             i ++;
        }

       StartCoroutine(SaveCSV.saveCSV(linhas,escolherDiretorio));
    }

    public void ResetLinhas()
    {
        LinhasLista = new List<string>() { "tempo_jogo,projetil,tempo_pouso,qtd_baloes_acertados,forca,distancia" };
    }

}

