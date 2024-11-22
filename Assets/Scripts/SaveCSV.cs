using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System.IO;

public class SaveCSV : MonoBehaviour
{

    public static string diretorio_padrao = Directory.GetCurrentDirectory() + @"\Dados.csv"; //@"Assets\Dados.csv"; //"\"C:\\Users\\Cliente\\Desktop\\Unity Projetos\\A3\\Assets\\Dados.csv\""; //@"C:\Users\Cliente\Desktop\Unity Projetos\A3\Assets\Dados.csv"; // Salvando o caminho que player salvou os dados

    public static string dados_path = diretorio_padrao; 

    public IEnumerator saveCSV(string linhas, bool escolherDiretorio = false)
    {
        if (!escolherDiretorio) {
            // Salvar o arquivo CSV
            File.WriteAllText(diretorio_padrao, linhas);

            Debug.Log("Arquivo CSV salvo em: " + diretorio_padrao);
            //C:/Users/Cliente/AppData/LocalLow/GrupoA3/ProjetoChina

            yield return null;

        } else {

            // Este código serve para o player escolher o local de save dos dados
            var caminho = StandaloneFileBrowser.SaveFilePanel("Salvar arquivo CSV", "", "Dados", "csv");

            if (!string.IsNullOrEmpty(caminho))
            {
                // Salvar o arquivo CSV no caminho escolhido
                File.WriteAllText(caminho, linhas);
                dados_path = caminho;
                Debug.Log("Arquivo CSV salvo em: " + caminho);
            }
            else
            {
                Debug.Log("Nenhum caminho selecionado.");
            }

        }

    }


}
