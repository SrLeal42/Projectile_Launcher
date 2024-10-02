using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public projectileData PData;

    [Header("Gravidade")]
    public float gravEstatica = -9.81f; // Gravidade estatica 
    public float grav_mult = 1.5f; // Multiplicador da gravidade

    [Header("Resistência do ar")]
    public float dragCoefficient = 0.47f; // Coeficiente de arrasto para uma esfera
    public float crossSectionArea = 0.01f; // Area da seção transversal em m2
    public float airDensity = 1.225f; // Densidade do ar em kg/m3

    private Vector3 velocity;
    [HideInInspector] public float vel;


    private float timeToDestroy;
    
    private bool estaEstacado = false;
    private int baloesAtingidos = 0;

    private BoxCollider hitBox;
    private Transform hitboxChild = null;
    private Dictionary<Collider, GameObject> collidersFilhos = new Dictionary<Collider, GameObject>();// dicionario para projeteis com diferentes hitbox, como machado

    private Vector3 newRotation = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //CC = GetComponent<CharacterController>();
        hitBox = GetComponent<BoxCollider>();
        timeToDestroy = PData.timeToDestroy;

        foreach (Transform child in transform)
        {
            Collider collider = child.GetComponent<Collider>();
            hitBox = child.GetComponent<BoxCollider>();
            hitboxChild = child;
            if (collider != null)
            {
                collidersFilhos.Add(collider, child.gameObject);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        movimento();

        if (timeToDestroy <= 0)
        {
            Destroy(gameObject);
        }

        timeToDestroy -= 1 * Time.deltaTime;

        
        Collider[] colliders = Physics.OverlapBox(transform.position, hitBox.size / 2, transform.rotation);
        
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++) {
                
                IDamageableScript damageable = colliders[i].GetComponent<IDamageableScript>();

                if (damageable != null)
                {
                    damageable.damage(PData.damage);
                    baloesAtingidos++;
                    if (baloesAtingidos >= PData.punchingPower)
                    {
                        destruirProjetil();
                    }
                }
            }


        }

       

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("parede"))
        {
           if (!PData.isPointed) ricochetear(collision);

            if (transform.tag == "machado") { 

                foreach (ContactPoint contact in collision.contacts)
                {
                    foreach (var entry in collidersFilhos)
                    {
                        if (entry.Key.bounds.Contains(contact.point))
                        {
                            GameObject filhoQueColidiu = entry.Value;
                            //Debug.Log("O filho que colidiu é: " + filhoQueColidiu.name);

                            if (filhoQueColidiu.transform.tag == "cabecaMachado")
                            {
                                Debug.Log("O filho que colidiu é: " + filhoQueColidiu.name);

                                estacar();
                            }
                    
                        }
                    }
                }
            }
            
        }

    }


    public Collider detectarColisaoPelaTag(string tag)
    {
        Vector3 scale = hitboxChild ? hitboxChild.localScale : transform.localScale; 
        Vector3 colliderHitBox = new Vector3(hitBox.size.x * scale.x, hitBox.size.y * scale.y, hitBox.size.z * scale.z);
        Collider[] colliders = Physics.OverlapBox(transform.position, colliderHitBox / 2, transform.rotation);

        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].transform.tag == tag) {
                    return colliders[i];
                }
            }
        }

        return null;
    }

    public void setDirection(Vector3 direction)
    {
        velocity = direction * vel;
    }

    public void destruirProjetil()
    {
        Destroy(gameObject);
    }


    public void ricochetear(Collision colisao)
    {

        Vector3 normalColisao = colisao.contacts[0].normal;

        Vector3 direcaoRicochete = Vector3.Reflect(velocity, normalColisao);

        float coeficienteDeRestituicao = 0.1f;

        // Vf = e * v ---------- Vf = velocidade final / e = coeficiente de restituição / v = velocidade inicial
        velocity = direcaoRicochete * coeficienteDeRestituicao;

    }

    public void estacar()
    {
        //Debug.Log("MAG " + velocity.magnitude);

        if (!detectarColisaoPelaTag("parede")) return; 

        float velMin = 10;

        if (velocity.magnitude > velMin || estaEstacado) {
            velocity = Vector3.zero;
            estaEstacado = true;
        } else if(velocity.magnitude <= velMin && !estaEstacado)
        {
            velocity.x = 0;
            velocity.z = 0;
        }
    }

    public void desaceleracao()
    {
        float speedMagnitude = velocity.magnitude;


        if (speedMagnitude <= 0) return;

        if (detectarColisaoPelaTag("chao"))
        {
            float groundFriction = 5f * PData.weight;
            velocity *= (1 - groundFriction * Time.deltaTime);

            if (speedMagnitude < 0.1)
            {
                velocity = Vector3.zero;
            }

        } else {

            float dragForceMagnitude = 0.5f * dragCoefficient * airDensity * crossSectionArea * speedMagnitude * speedMagnitude;
            Vector3 dragForce = -velocity.normalized * dragForceMagnitude;
            velocity += dragForce * Time.deltaTime;

        }
        
        //Debug.Log("DRAg"+dragForce);
    }

    public void gravidade()
    {
        if (detectarColisaoPelaTag("chao"))
        {
            velocity.y = 0;
            return;
        }
        
        velocity.y += gravEstatica * Time.deltaTime;
    }

    public void movimento()
    {
        gravidade();
        desaceleracao();
        if (PData.isPointed) estacar(); //else ricochetear();

        //Debug.Log("Vel" + velocity);
        transform.position += velocity * Time.deltaTime;


        if (transform.tag == "machado" && !estaEstacado)
        {
            Quaternion rotacao = Quaternion.Euler(800f * Time.deltaTime, 0, 0);
            transform.rotation *= rotacao;
        } else
        {
            transform.forward = velocity != Vector3.zero ?
                Vector3.MoveTowards(transform.forward, velocity, 100f * Time.deltaTime) : transform.forward;//olhando para o movimento
        }

        
    }


    private void OnDrawGizmos()
    {
        Vector3 scale = hitboxChild ? hitboxChild.localScale : transform.localScale;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(hitBox.size.x * scale.x, hitBox.size.y * scale.y, hitBox.size.z * scale.z));
    }


}