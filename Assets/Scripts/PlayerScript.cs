using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
  

    public CSVScript CSVScript;

    public weaponScript currentWeapon;
    public float power = 0f;
    public float maxPower = 50f;
    public Transform muzzle;
    public Camera playerCamera;

    [Header("Game State")]
    public string gameState = "menuinicial";

    [Header("Canvas")]
    Dictionary<string, Canvas> CanvasDic = new Dictionary<string, Canvas>();
    public Canvas canvasHUD;
    public Canvas canvasMenu;

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
    public float multTimer = 1f;
    float multTimerMax = 15f; // Limite que multTimer pode chegar 
    float DecreaseAmountTimer = 0.05f; // Usamos essa variavel para aumentar o multTimer com o tempo
    float IncreaseAmountTimer = 0.2f; // Usamos essa variavel para multiplicar o multTimer e reduzir a quantidade de tempo aumentando ao estourar um balao
    public Slider timerBar;
    public Image fillImage;
    private Color fullColor = Color.green; // Cor da barra cheia
    private Color emptyColor = Color.red; // Cor da barra vazia

    // Start is called before the first frame update
    void Start()
    {

        CanvasDic.Add("HUD", canvasHUD);
        CanvasDic.Add("Menu", canvasMenu);

        // Setando o tempo atual no maximo
        currentTime = maxTempo;

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
        
        }


    }


    void gameStatePlaying()
    {
        if(canvasHUD.enabled != true) changeCanvas("HUD");

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
            CSVScript.saving();
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
    }

    public void increasingTimeAndUpdateBar(float increase)
    {
        currentTime += increase * (multTimer * IncreaseAmountTimer); // Aumentando o tempo atual com base no produto entre o multTimer e um montante
        currentTime = Mathf.Clamp(currentTime, 0f, maxTempo); // Impedindo que o current time seja menor que 0 e maior que o numero máximo
        timerBar.value = currentTime / maxTempo; // Atualizando a barra de progresso com o valor do tempo atual 

        fillImage.color = Color.Lerp(emptyColor, fullColor, timerBar.value); // Modificando a cor da barra para efeito visual
    }


}
