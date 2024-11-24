using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    //[SerializeField] private Disparo disparoPrefab;
    [SerializeField] private Transform spawnPoint;

    private ObjectPool<Enemigo> poolenemigos;
    private ObjectPool<Disparo> globalpooldisparos;
    private Coroutine disparosCoroutine;
    private bool isReleased = false;

    public ObjectPool<Enemigo> PoolEnemigos { get => poolenemigos; set => poolenemigos = value; }

    public void SetGlobalDisparoPool(ObjectPool<Disparo> pool)
    {
        globalpooldisparos = pool;
    }
    private void OnEnable()
    {
        isReleased = false; // Resetear el flag al activar el enemigo
        // Reinicia el coroutine de disparos cada vez que el enemigo se activa
        if (disparosCoroutine != null)
        {
            StopCoroutine(disparosCoroutine);
        }
        disparosCoroutine = StartCoroutine(SpawnearDisparos());
    }

    private void OnDisable()
    {
        // Detén el coroutine al desactivar el enemigo
        if (disparosCoroutine != null)
        {
            StopCoroutine(disparosCoroutine);
            disparosCoroutine = null;
        }
    }

    /*
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
        StartCoroutine(SpawnearDisparos());
    }
    */
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);
    }

    IEnumerator SpawnearDisparos()
    {
        // Disparo inicial inmediato
        if (globalpooldisparos != null)
        {
            Disparo disparoCopia = globalpooldisparos.Get();
            disparoCopia.gameObject.SetActive(true);
            disparoCopia.transform.position = spawnPoint.position;
        }

        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            if (globalpooldisparos != null)
            {
                Disparo disparoCopia = globalpooldisparos.Get();
                disparoCopia.gameObject.SetActive(true);
                disparoCopia.transform.position = spawnPoint.position;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void ReleaseEnemigo()
    {
        if (!isReleased)
        {
            isReleased = true;
            poolenemigos.Release(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (isReleased) return;

        if (elOtro.gameObject.CompareTag("DisparoPlayer"))
        {
            // Libera el disparo del jugador al pool
            Disparo disparoPlayer = elOtro.GetComponent<Disparo>();
            if (disparoPlayer != null && disparoPlayer.MyPool != null)
            {
                disparoPlayer.MyPool.Release(disparoPlayer);
            }
            ReleaseEnemigo();
        }
        else if (elOtro.gameObject.CompareTag("Bound"))
        {
            ReleaseEnemigo();
        }
    }
}
