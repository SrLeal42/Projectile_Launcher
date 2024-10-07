using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonScript : MonoBehaviour
{

    private Vector3 direction;
    public int vida = 1;
    public float vel = 1.5f;
    public float timeIncrease = 10f;

    public float timeToDestroy = 10f;

    // Start is called before the first frame update
    void Start()
    {
        direction.x = vel;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * Time.deltaTime;

        timeToDestroy -= Time.deltaTime;

        if (timeToDestroy <= 0 ) Destroy(gameObject);
    }
}
