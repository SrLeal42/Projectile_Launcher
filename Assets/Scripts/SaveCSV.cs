using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System.IO;

public class SaveCSV : MonoBehaviour
{
    public static void saveCSV(string linhas)
    {

        var caminho = StandaloneFileBrowser.SaveFilePanel("Salvar arquivo CSV", "", "Dados", "csv");

        if (!string.IsNullOrEmpty(caminho))
        {
            // Salvar o arquivo CSV no caminho escolhido
            File.WriteAllText(caminho, linhas);

            Debug.Log("Arquivo CSV salvo em: " + caminho);
        }
        else
        {
            Debug.Log("Nenhum caminho selecionado.");
        }
    }


}
