using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float ratioDisparo;
    [SerializeField] private Disparo disparoPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int numeroDisparos;
    private float temporizador = 0.5f;
    private float vidas = 100;

    private ObjectPool<Disparo> pool;
    private void Awake()
    {
        pool = new ObjectPool<Disparo>(CrearDisparo, null, ReleaseDisparo, DestroyDisparo);
    }

    private Disparo CrearDisparo()
    {
        Disparo disparoCopia = Instantiate(disparoPrefab, transform.position, Quaternion.identity);
        disparoCopia.MyPool = pool;
        return disparoCopia;
    }

    private void ReleaseDisparo(Disparo disparo)
    {
        disparo.gameObject.SetActive(false);
    }
    private void DestroyDisparo(Disparo disparo)
    {
        Destroy(disparo.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        DelimitarMovimiento();
        Disparar();
    }

    void Movimiento()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector2(inputH, inputV).normalized * velocidad * Time.deltaTime);
    }

    void DelimitarMovimiento()
    {
        float xClamped = Mathf.Clamp(transform.position.x, -8.4f, 8.4f);
        float yClamped = Mathf.Clamp(transform.position.y, -4.5f, 4.5f);
        transform.position = new Vector3(xClamped, yClamped, 0);
    }

    void Disparar()
    {
        temporizador += 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && temporizador > ratioDisparo)
        {
            /* // poner en una corrutina para que haya efectos, este es espiral
            float gradosPorDisparo = 360 / numeroDisparos;
            
            for (float i = 0; i < 360; i+=gradosPorDisparo)
            {
                // Estoy pidiendo a la piscina que me de un nuevo disparo.
                Disparo disparocopia = pool.Get();
                disparocopia.gameObject.SetActive(true);
                disparocopia.transform.position = spawnPoints[i].transform.position;
                disparocopia.transform.eulerAngles =new Vector3(0f, 0f, i);
                yield return new WaitforSeconds(0.2f);
            }
            */
            for (int i = 0; i < 2; i++)
            {
                // Estoy pidiendo a la piscina que me de un nuevo disparo.
                Disparo disparocopia = pool.Get();
                disparocopia.gameObject.SetActive(true);
                disparocopia.transform.position = spawnPoints[i].transform.position;
                
            }
            temporizador = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DisparoEnemigo") || elOtro.gameObject.CompareTag("Enemigo"))
        {
            vidas -= 20;
            Destroy(elOtro.gameObject);
            if (vidas <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
