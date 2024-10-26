using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVScript : MonoBehaviour
{
    public List<string> LinhasLista = new List<string>() { "projetil,tempo_pouso,qtd_baloes_acertados,forca,distancia" };
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
        string linha  = p + "," + TP + "," + QBA + "," + F + "," + dis + "\n";
        
        LinhasLista.Add(linha);
    }

    public void saving()
    {
        string linhas = "";

        int i = 0;

        foreach (string line in LinhasLista)
        {
            // Adicionando um \n caso seja a primeira linha
            linhas += i == 1 ? "\n" + line : line;
            
            
             i ++;
        }

        SaveCSV.saveCSV(linhas);
    }


}

