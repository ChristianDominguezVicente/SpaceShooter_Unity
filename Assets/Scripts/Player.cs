using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float ratioDisparo;
    [SerializeField] private Disparo disparoPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int numeroDisparos;
    [SerializeField] private Image imageVida;
    [SerializeField] private TMP_Text Puntuacion;
    private float temporizador = 0.5f;

    protected PlayerModel playerModel;
    private AudioSource audioS;
    private float widthVida;

    public PlayerModel getPlayer()
    {
        return playerModel;
    }

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
        audioS = GetComponent<AudioSource>();
        playerModel = gameObject.AddComponent<PlayerModel>();
        widthVida = imageVida.rectTransform.sizeDelta.x;
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
        transform.Translate(new Vector2(inputH, inputV).normalized * playerModel.pVelocidad * Time.deltaTime);
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
            int tipoDisparo = PlayerPrefs.GetInt("Disparo", 1);
            switch (tipoDisparo)
            {
                case 1: DisparoDoble(); break;
                case 2: DisparoOndas(); break;
                case 3: DisparoCuadra(); break;
                default: DisparoDoble(); break;
            }

            if (audioS.loop == false)
            {
                audioS.loop = true;
                audioS.Play();
            }

            temporizador = 0;
        }
        else
        {
            audioS.loop = false;
        }
    }

    private void DisparoDoble()
    {
        for (int i = 0; i < 2; i++)
        {
            // Estoy pidiendo a la piscina que me de un nuevo disparo.
            Disparo disparocopia = pool.Get();
            disparocopia.gameObject.SetActive(true);
            disparocopia.transform.position = spawnPoints[i].transform.position;

        }
    }

    private void DisparoOndas()
    {
        for (int i = 0; i < 2; i++)
        {
            // Estoy pidiendo a la piscina que me de un nuevo disparo.
            Disparo disparocopia = pool.Get();
            disparocopia.gameObject.SetActive(true);
            disparocopia.transform.position = spawnPoints[i].transform.position;
            disparocopia.AmplitudOndas = 0.05f;
            disparocopia.FrecuenciaOndas = 2f;
            if (i == 0)
                disparocopia.InversionOndas = true;
            else
                disparocopia.InversionOndas = false;
        }
    }

    private void DisparoCuadra()
    {
        for (int i = 0; i < 4; i++)
        {
            Disparo disparocopia = pool.Get();
            switch (i)
            {
                case 0:
                case 1:
                    // Estoy pidiendo a la piscina que me de un nuevo disparo.
                    disparocopia.gameObject.SetActive(true);
                    disparocopia.transform.position = spawnPoints[i].transform.position;
                    disparocopia.transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case 2:
                    disparocopia.gameObject.SetActive(true);
                    disparocopia.transform.position = spawnPoints[0].transform.position;
                    disparocopia.transform.eulerAngles = new Vector3(0, 0, 45);
                    break;
                case 3:
                    disparocopia.gameObject.SetActive(true);
                    disparocopia.transform.position = spawnPoints[1].transform.position;
                    disparocopia.transform.eulerAngles = new Vector3(0, 0, -45);
                    break;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DisparoEnemigo"))
        {
            Disparo disparoEnemigo = elOtro.GetComponent<Disparo>();
            disparoEnemigo.MyPool.Release(disparoEnemigo);
            playerModel.pVida -= 20;
            imageVida.rectTransform.sizeDelta = new Vector2((playerModel.pVida / 100f) * widthVida, imageVida.rectTransform.sizeDelta.y);
            if (playerModel.pVida <= 0)
            {
                if (playerModel.pPuntuacion > PlayerPrefs.GetInt("Puntuacion"))
                    Puntuacion.text = "Puntos: " + playerModel.pPuntuacion;
                else
                    Puntuacion.text = "Puntos: " + PlayerPrefs.GetInt("Puntuacion");
                Destroy(this.gameObject);
            }
        }
        else if (elOtro.gameObject.CompareTag("Enemigo"))
        {
            Enemigo enemigo = elOtro.GetComponent<Enemigo>();
            enemigo.PoolEnemigos.Release(enemigo);
            playerModel.pVida -= 20;
            imageVida.rectTransform.sizeDelta = new Vector2((playerModel.pVida / 100f) * widthVida, imageVida.rectTransform.sizeDelta.y);
            if (playerModel.pVida <= 0)
            {
                if (playerModel.pPuntuacion > PlayerPrefs.GetInt("Puntuacion"))
                    Puntuacion.text = "Puntos: " + playerModel.pPuntuacion;
                else
                    Puntuacion.text = "Puntos: " + PlayerPrefs.GetInt("Puntuacion");
                Destroy(this.gameObject);
            }
        }
        else if (elOtro.gameObject.CompareTag("EnemigoD"))
        {
            EnemigoD enemigod = elOtro.GetComponent<EnemigoD>();
            enemigod.PoolEnemigosD.Release(enemigod);
            playerModel.pVida -= 20;
            imageVida.rectTransform.sizeDelta = new Vector2((playerModel.pVida / 100f) * widthVida, imageVida.rectTransform.sizeDelta.y);
            if (playerModel.pVida <= 0)
            {
                if (playerModel.pPuntuacion > PlayerPrefs.GetInt("Puntuacion"))
                    Puntuacion.text = "Puntos: " + playerModel.pPuntuacion;
                else
                    Puntuacion.text = "Puntos: " + PlayerPrefs.GetInt("Puntuacion");
                Destroy(this.gameObject);
            }
        }
        else if (elOtro.gameObject.CompareTag("Life"))
        {
            Life life = elOtro.GetComponent<Life>();
            life.PoolLife.Release(life);
            if (playerModel.pVida < 100)
            {
                playerModel.pVida += 20;
            }
            imageVida.rectTransform.sizeDelta = new Vector2((playerModel.pVida / 100f) * widthVida, imageVida.rectTransform.sizeDelta.y);
        }
        else if (elOtro.gameObject.CompareTag("Speed"))
        {
            Speed speed = elOtro.GetComponent<Speed>();
            speed.PoolSpeed.Release(speed);

            StartCoroutine(SpeedUp(3f, 1.5f));
        }
    }

    private IEnumerator SpeedUp(float time, float speedUp)
    {
        float speedOriginal = playerModel.pVelocidad;
        playerModel.pVelocidad *= speedUp;
        yield return new WaitForSeconds(time);
        playerModel.pVelocidad = speedOriginal;
    }
}
