using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public weaponScript currentWeapon;
    public float power = 0f;
    public float maxPower = 50f;
    public Transform muzzle;
    public Camera playerCamera;

    [Header("Gravidade")]
    public float gravEstatica = -9.81f; // Gravidade estatica 

    [Header("Trajetória")]
    public LineRenderer lineRenderer;
    private float lineAlpha = 0.5f;
    private int lineResolution = 100; // Quantos pontos compõem a linha
    private float timeIntervalinPoints = 0.01f; // Intervalo de tempo entre cada ponto

    // Start is called before the first frame update
    void Start()
    {
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


}
