using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemigo enemigoPrefab;
    [SerializeField] private Disparo disparoPrefab;
    [SerializeField] private TextMeshProUGUI textoOleadas;

    private ObjectPool<Enemigo> poolenemigos;
    private ObjectPool<Disparo> pooldisparos;
    private void Awake()
    {
        poolenemigos = new ObjectPool<Enemigo>(CrearEnemigo, null, ReleaseEnemigo, DestroyEnemigo);
        pooldisparos = new ObjectPool<Disparo>(CrearDisparo, null, ReleaseDisparo, DestroyDisparo);
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
        for (int i = 0; i < 5; i++) // Niveles
        {
            for (int j = 0; j < 3; j++) // Oleadas
            {
                textoOleadas.text = "Nivel " + (i + 1) + " - " + "Oleada " + (j + 1);
                yield return new WaitForSeconds(1f);
                textoOleadas.text = "";
                for (int k = 0; k < 10; k++) // Enemigos
                {
                    Vector3 puntoAleatorio = new Vector3(transform.position.x, Random.Range(-4.5f, 4.5f), 0);
                    //Instantiate(enemigoPrefab, puntoAleatorio, Quaternion.identity);
                    Enemigo enemigoCopia = poolenemigos.Get();
                    enemigoCopia.gameObject.SetActive(true);
                    enemigoCopia.transform.position = puntoAleatorio;
                    yield return new WaitForSeconds(0.5f);
                }
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(3f);
        }  
    }
}
