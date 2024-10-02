using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DamageableScript : MonoBehaviour, IDamageableScript
{
    private BallonScript BS;
    private float health = 0;

    private void Awake()
    {

    }

    private void LateUpdate()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void damage(float dano)
    {

        health -= dano;
        //Debug.Log("HEL" + health);

       
    }

    // Start is called before the first frame update
    void Start()
    {
        BS = GetComponent<BallonScript>();
        health = BS.vida;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
