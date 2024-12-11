using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemigo enemigoPrefab;
    [SerializeField] private EnemigoD enemigoDiagonalPrefab;
    [SerializeField] private Disparo disparoPrefab;
    [SerializeField] private Life lifePrefab;
    [SerializeField] private Speed speedPrefab;
    [SerializeField] private TextMeshProUGUI textoOleadas;

    private ObjectPool<Enemigo> poolenemigos;
    private ObjectPool<EnemigoD> poolenemigosd;
    private ObjectPool<Disparo> pooldisparos;
    private ObjectPool<Life> poollife;
    private ObjectPool<Speed> poolspeed;
    private void Awake()
    {
        poolenemigosd = new ObjectPool<EnemigoD>(CrearEnemigoD, null, ReleaseEnemigoD, DestroyEnemigoD);
        poolenemigos = new ObjectPool<Enemigo>(CrearEnemigo, null, ReleaseEnemigo, DestroyEnemigo);
        pooldisparos = new ObjectPool<Disparo>(CrearDisparo, null, ReleaseDisparo, DestroyDisparo);
        poollife = new ObjectPool<Life>(CrearLife, null, RealeaseLife, DestroyLife);
        poolspeed = new ObjectPool<Speed>(CrearSpeed, null, RealeaseSpeed, DestroySpeed);
    }

    private EnemigoD CrearEnemigoD()
    {
        EnemigoD enemigoDCopia = Instantiate(enemigoDiagonalPrefab, transform.position, Quaternion.identity);
        enemigoDCopia.PoolEnemigosD = poolenemigosd;
        enemigoDCopia.SetGlobalDisparoPool(pooldisparos);
        return enemigoDCopia;
    }

    private void ReleaseEnemigoD(EnemigoD enemigod)
    {
        enemigod.gameObject.SetActive(false);
    }
    private void DestroyEnemigoD(EnemigoD enemigod)
    {
        Destroy(enemigod.gameObject);
    }

    private Enemigo CrearEnemigo()
    {
        Enemigo enemigoCopia = Instantiate(enemigoPrefab, transform.position, Quaternion.identity);
        enemigoCopia.PoolEnemigos = poolenemigos;
        enemigoCopia.SetGlobalDisparoPool(pooldisparos);
        return enemigoCopia;
    }

    private void ReleaseEnemigo(Enemigo enemigo)
    {
        enemigo.gameObject.SetActive(false);
    }
    private void DestroyEnemigo(Enemigo enemigo)
    {
        Destroy(enemigo.gameObject);
    }

    private Disparo CrearDisparo()
    {
        Disparo disparoCopia = Instantiate(disparoPrefab, transform.position, Quaternion.identity);
        disparoCopia.MyPool = pooldisparos;
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

    private Life CrearLife()
    {
        Life lifeCopia = Instantiate(lifePrefab, transform.position, Quaternion.identity);
        lifeCopia.PoolLife = poollife;
        return lifeCopia;
    }

    private void RealeaseLife(Life life)
    {
        life.gameObject.SetActive(false);
    }
    private void DestroyLife(Life life)
    {
        Destroy(life.gameObject);
    }

    private Speed CrearSpeed()
    {
        Speed speedCopia = Instantiate(speedPrefab, transform.position, Quaternion.identity);
        speedCopia.PoolSpeed = poolspeed;
        return speedCopia;
    }

    private void RealeaseSpeed(Speed speed)
    {
        speed.gameObject.SetActive(false);
    }
    private void DestroySpeed(Speed speed)
    {
        Destroy(speed.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnearEnemigos());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnearEnemigos()
    {
        int nivel = 1;
        while (true) // Niveles
        {
            for (int j = 0; j < 3; j++) // Oleadas
            {
                textoOleadas.text = "Nivel " + nivel + " - " + "Oleada " + (j + 1);
                yield return new WaitForSeconds(1f);
                textoOleadas.text = "";
                for (int k = 0; k < 10; k++) // Enemigos
                {
                    Vector3 puntoAleatorio = new Vector3(transform.position.x, Random.Range(-4.5f, 4.5f), 0);

                    if (Random.value < 0.7f)
                    {
                        Enemigo enemigoCopia = poolenemigos.Get();
                        enemigoCopia.gameObject.SetActive(true);
                        enemigoCopia.transform.position = puntoAleatorio;
                        if (Random.value < 0.33f)
                        {
                            Vector3 puntoAleatorio2 = new Vector3(transform.position.x, Random.Range(-4.5f, 4.5f), 0);
                            if (Random.value < 0.5f)
                            {
                                Life lifeCopia = poollife.Get();
                                lifeCopia.gameObject.SetActive(true);
                                lifeCopia.transform.position = puntoAleatorio2;
                            }
                            else
                            {
                                Speed speedCopia = poolspeed.Get();
                                speedCopia.gameObject.SetActive(true);
                                speedCopia.transform.position = puntoAleatorio2;
                            }
                        }
                    }
                    else
                    {
                        EnemigoD enemigoDCopia = poolenemigosd.Get();
                        enemigoDCopia.gameObject.SetActive(true);
                        enemigoDCopia.transform.position = puntoAleatorio;
                        if (Random.value < 0.33f)
                        {
                            Vector3 puntoAleatorio2 = new Vector3(transform.position.x, Random.Range(-4.5f, 4.5f), 0);
                            if (Random.value < 0.5f)
                            {
                                Life lifeCopia = poollife.Get();
                                lifeCopia.gameObject.SetActive(true);
                                lifeCopia.transform.position = puntoAleatorio2;
                            }
                            else
                            {
                                Speed speedCopia = poolspeed.Get();
                                speedCopia.gameObject.SetActive(true);
                                speedCopia.transform.position = puntoAleatorio2;
                            }
                        }
                    }

                    yield return new WaitForSeconds(0.5f);
                }
                yield return new WaitForSeconds(2f);
            }
            nivel++;
            yield return new WaitForSeconds(3f);
        }
    }
}
