using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
  

    public CSVScript CSVScript;

    // PROVISORIO
    public PythonConnection PC;
    public TMP_Text precisao;
    public TMP_Text distancia;
    public TMP_Text acertosPorSegundo;
    //PROVISORIO

    public weaponScript currentWeapon;
    public float power = 0f;
    public float maxPower = 50f;
    public Transform muzzle;
    public Camera playerCamera;

    public SpawnerScript spawner;

    [HideInInspector] public float tempo_jogo = 0;

    [Header("Game State")]
    public string gameState = "menuinicial";

    [Header("Canvas")]
    Dictionary<string, Canvas> CanvasDic = new Dictionary<string, Canvas>();
    public Canvas canvasHUD;
    public Canvas canvasMenu;
    public Canvas canvasGameOver;
    public Canvas canvasEstatistica;
    public Canvas canvasOpcoes;

    [Header("Gravidade")]
    public float gravEstatica = -9.81f; // Gravidade estatica 

    [Header("Trajetória")]
    public LineRenderer lineRenderer;
    private float lineAlpha = 0.5f;
    private int lineResolution = 100; // Quantos pontos compõem a linha
    private float timeIntervalinPoints = 0.01f; // Intervalo de tempo entre cada ponto

    [Header("Timer")]
    float maxTempo = 120f;
    float currentTime = 120f;

    private float multTimerInit = 1f; // Valor inicial para o mutiplicador do timer
    public float multTimer;
    float multTimerMax = 15f; // Limite que multTimer pode chegar 
    float DecreaseAmountTimer = 0.05f; // Usamos essa variavel para aumentar o multTimer com o tempo
    float IncreaseAmountTimer = 0.2f; // Usamos essa variavel para multiplicar o multTimer e reduzir a quantidade de tempo aumentando ao estourar um balao

    [Header("Opçoes")]
    public TMP_InputField inputEnvironment;

    public Slider timerBar;
    public Image fillImage;
    private Color fullColor = Color.green; // Cor da barra cheia
    private Color emptyColor = Color.red; // Cor da barra vazia

    // Start is called before the first frame update
    void Start()
    {

        CanvasDic.Add("HUD", canvasHUD);
        CanvasDic.Add("Menu", canvasMenu);
        CanvasDic.Add("GameOver", canvasGameOver);
        CanvasDic.Add("Estatistica", canvasEstatistica);
        CanvasDic.Add("Opcoes", canvasOpcoes);

        // Setando o tempo atual no maximo
        currentTime = maxTempo;

        multTimer = multTimerInit;

        // Alterando o alpha da linha
        Color startColor = lineRenderer.startColor;
        Color endColor = lineRenderer.endColor;
        startColor.a = lineAlpha;
        endColor.a = lineAlpha;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

    }

    // Update is called once per frame
    void Update()
    {

        switch (gameState) {

            case "playing": gameStatePlaying();
                break;

            case "menuinicial":
                if (canvasMenu.enabled != true) changeCanvas("Menu");
                break;

            case "gameover":
                if (canvasGameOver.enabled != true) changeCanvas("GameOver");
                break;
            
            case "estatistica":
                if (canvasEstatistica.enabled != true) changeCanvas("Estatistica");
                break;

            case "opcoes":
                if (canvasOpcoes.enabled != true) changeCanvas("Opcoes");
                break;

        }


    }


    void gameStatePlaying()
    {
        if(canvasHUD.enabled != true) changeCanvas("HUD");

        tempo_jogo += Time.deltaTime;
        //Debug.Log(tempo_jogo);
        
        decreasingTimeAndUpdateBar();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            power += currentWeapon.WData.powerIncrease;
            power = power > currentWeapon.WData.maxPower ? currentWeapon.WData.maxPower : power;

            // Lança um Raycast a partir da posição do mouse na tela
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float maxRayDistance = 26f; // Distância máxima do Raycast

            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                //Debug.Log(hit.transform.name

                //Debug.DrawLine(muzzle.position, hit.point, Color.red, 1f);
                Vector3 directionToHit = (hit.point - muzzle.position).normalized;

                drawTrajectory(directionToHit);
            }
            else
            {
                //Debug.DrawLine(muzzle.position, ray.GetPoint(maxRayDistance), Color.red, 1f);
                drawTrajectory(ray.direction);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //Debug.Log(power);


            // Lança um Raycast a partir da posição do mouse na tela
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float maxRayDistance = 26f; // Distância máxima do Raycast

            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                //Debug.Log(hit.transform.name

                Debug.DrawLine(muzzle.position, hit.point, Color.red, 1f);
                Vector3 directionToHit = (hit.point - muzzle.position).normalized;

                currentWeapon.shoot(directionToHit, power);
            }
            else
            {
                Debug.DrawLine(muzzle.position, ray.GetPoint(maxRayDistance), Color.red, 1f);
                currentWeapon.shoot(ray.direction, power);
            }


            //currentWeapon.shoot(power);
            stopDrawTrajectory();
            power = 0f;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            changeGameState("gameover");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void changeGameState (string state)
    {
        gameState = state;
    } 

    public void changeCanvas(string canvas)
    {
        foreach (KeyValuePair<string, Canvas> c in CanvasDic)
        {
            c.Value.enabled = c.Key == canvas;
        }
     
    }

    public void drawTrajectory(Vector3 _dir)
    {
        lineRenderer.enabled = true;

        Vector3 origin = muzzle.transform.position;
        Vector3 startVelocity = power * _dir;
        lineRenderer.positionCount = lineResolution;
        float time = 0;

        for (int i = 0; i < lineResolution; i++)
        {
            // s = u*t + 1/2*g*t*t
            var x = (startVelocity.x * time);
            var y = (startVelocity.y * time) + (gravEstatica/2 * time * time);
            var z = (startVelocity.z * time);

            Vector3 point = new Vector3(x, y, z);

            lineRenderer.SetPosition(i, origin + point);

            time += timeIntervalinPoints;

        }

    }

    public void stopDrawTrajectory()
    {
        lineRenderer.enabled = false;
    }


    public void decreasingTimeAndUpdateBar()
    {
        currentTime -= Time.deltaTime * multTimer; // Diminuindo o tempo usando um mutiplicador para aumentar o decrecimento 
        multTimer += multTimer >= multTimerMax? 0 : DecreaseAmountTimer * Time.deltaTime; // Aumentando o mutiplicador do decrecimento do tempo utilizando um montante pré-definido
        currentTime = Mathf.Clamp(currentTime, 0f, maxTempo); // Impedindo que o current time seja menor que 0 e maior que o numero máximo 
        timerBar.value = currentTime / maxTempo; // Atualizando a barra de progresso com o valor do tempo atual 
        
        fillImage.color = Color.Lerp(emptyColor, fullColor, timerBar.value); // Modificando a cor da barra para efeito visual

        if (currentTime <= 0)
        {
            changeGameState("gameover");
        }
    }

    public void increasingTimeAndUpdateBar(float increase)
    {
        currentTime += increase * (multTimer * IncreaseAmountTimer); // Aumentando o tempo atual com base no produto entre o multTimer e um montante
        currentTime = Mathf.Clamp(currentTime, 0f, maxTempo); // Impedindo que o current time seja menor que 0 e maior que o numero máximo
        timerBar.value = currentTime / maxTempo; // Atualizando a barra de progresso com o valor do tempo atual 

        fillImage.color = Color.Lerp(emptyColor, fullColor, timerBar.value); // Modificando a cor da barra para efeito visual
    }


    public void ResetParamter()
    {
        multTimer = multTimerInit; // Resetando o mutiplicador do tempo

        currentTime = maxTempo; // Resetando o tempo do timer

        timerBar.value = currentTime / maxTempo; // Resetando a barra de progresso com o valor do máximo 
        fillImage.color = fullColor; // Resetando a cor da barra

        tempo_jogo = 0; // Resetando o tempo de jogo
    }

    public void DestroyAllBallons()
    {
        BallonScript[] ballons = FindObjectsOfType<BallonScript>();

        foreach (BallonScript b in ballons)
        {
            Destroy(b.gameObject);
        }
    }

    public void ResetGame()
    {
        ResetParamter();
        spawner.ResetParamter();
        CSVScript.ResetLinhas();
        DestroyAllBallons();
        changeGameState("playing");
    }

    public void ExitGame()
    {
        Debug.Log("O jogo está sendo encerrado."); // Apenas para testes no Editor
        Application.Quit();
    }

    public void Estatisticas()
    {
        CSVScript.saving(escolherDiretorio: false);
        string result = PC.PythonCalcularDesempenho();

        UnityEngine.Debug.Log(result);

        string[] resultSplit = result.Split(' ');

        precisao.text = resultSplit[0];
        distancia.text = resultSplit[1];
        acertosPorSegundo.text = resultSplit[2];

        changeGameState("estatistica");
    }


    public void saveOpcoes()
    {
        //PC.PathEnvironment = inputEnvironment.text.Trim();
        string PATH = inputEnvironment.text.Trim();
        string[] PATHsplit = PATH.Split("\\");
        PATH += PATHsplit[PATHsplit.Length - 1] != "python.exe" ? "\\python.exe" : "";
        PC.PathEnvironment = PATH;

        Debug.Log(PC.PathEnvironment);
    }

}
