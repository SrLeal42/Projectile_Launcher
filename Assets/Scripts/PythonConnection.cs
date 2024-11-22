using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;
public class PythonConnection : MonoBehaviour
{

    public SaveCSV SCSV;

    string pythonScriptPath = System.IO.Path.Combine(Application.streamingAssetsPath, "main.py");

    public string PathEnvironment = "";

    // Start is called before the first frame update
    void Start()
    {
       //SCSV = GetComponent<SaveCSV>();
    }

    public string PythonCalcularDesempenho()
    {
        UnityEngine.Debug.Log(SaveCSV.diretorio_padrao);
        UnityEngine.Debug.Log(PathEnvironment);
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = PathEnvironment != null && PathEnvironment != ""? PathEnvironment:@"D:\Anaconda\envs\IA_TabelaCarros\python.exe";
        //start.Arguments = $"main.py calcular_desempenho {SaveCSV.dados_path}";
        start.RedirectStandardOutput = true;
        start.RedirectStandardError = true;
        start.UseShellExecute = false;

        start.ArgumentList.Add(pythonScriptPath); // O caminho para o script 
        start.ArgumentList.Add("get_dados"); // A função a ser utilizada
        start.ArgumentList.Add(SaveCSV.diretorio_padrao); // O caminho para os dados 

        string result = "";

        using (Process process = Process.Start(start))
        {
            using (System.IO.StreamReader reader = process.StandardOutput)
            {
                result = reader.ReadToEnd();
            }

            using (System.IO.StreamReader errorReader = process.StandardError)
            {
                string error = errorReader.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    UnityEngine.Debug.LogError($"Erro ao executar Python: {error}");
                }
            }
        }

        return result;
    }
}
