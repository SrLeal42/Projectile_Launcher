using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    [Header("Limites Areas")]
    public Vector3 area1Min = new Vector3(15,0,-3);
    public Vector3 area1Max = new Vector3(16, 5, 7);
    public Vector3 area2Min = new Vector3(-15, 0, -3);
    public Vector3 area2Max = new Vector3(-16, 5, 7);

    [Header("Balões")]
    public Transform[] ballons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            spawnarBalao();
        }

    }

    private void OnDrawGizmos()
    {
        // Definir a cor para a primeira área e desenhar o cubo
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((area1Min + area1Max) / 2, area1Max - area1Min);

        // Definir a cor para a segunda área e desenhar o cubo
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((area2Min + area2Max) / 2, area2Max - area2Min);
    }


    public void spawnarBalao(Transform balao = null, int area = -1)
    {
        balao = balao == null ? ballons[Random.Range(0, ballons.Length)] : balao;
        area = area == -1 ? Random.Range(0, 2) : area;

        Vector3 randomPosition = new Vector3(0,0,0);

        if (area == 0)
        {
            randomPosition = new Vector3(
                   Random.Range(area1Min.x, area1Max.x),
                   Random.Range(area1Min.y, area1Max.y),
                   Random.Range(area1Min.z, area1Max.z)
               );
        } else if (area == 1)
        {
            randomPosition = new Vector3(
                   Random.Range(area2Min.x, area2Max.x),
                   Random.Range(area2Min.y, area2Max.y),
                   Random.Range(area2Min.z, area2Max.z)
               );
        }


        Transform ballon = Instantiate(balao, randomPosition, Quaternion.identity);
        ballon.GetComponent<BallonScript>().vel *= area == 0? -1 : 1;

    }


}