using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class EnemigoD : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Transform[] spawnPoints;

    private ObjectPool<EnemigoD> poolenemigosd;
    private ObjectPool<Disparo> globalpooldisparos;
    private Coroutine disparosCoroutine;
    private bool isReleased = false;
    private Vector3 direccion = new Vector3(-1, 1, 0);

    public ObjectPool<EnemigoD> PoolEnemigosD { get => poolenemigosd; set => poolenemigosd = value; }

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

    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);

        if (transform.position.y > 4.5f || transform.position.y < -4.5f)
        {
            direccion.y *= -1; 
        }
    }

    IEnumerator SpawnearDisparos()
    {
        // Disparo inicial inmediato
        if (globalpooldisparos != null)
        {
            for (int i = 0; i < 2; i++)
            {
                Disparo disparocopia = globalpooldisparos.Get();
                disparocopia.gameObject.SetActive(true);
                disparocopia.transform.position = spawnPoints[i].transform.position;
            }
        }

        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            if (globalpooldisparos != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    Disparo disparocopia = globalpooldisparos.Get();
                    disparocopia.gameObject.SetActive(true);
                    disparocopia.transform.position = spawnPoints[i].transform.position;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void ReleaseEnemigo()
    {
        if (!isReleased)
        {
            isReleased = true;
            poolenemigosd.Release(this);
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

            Player player;
            player = GameObject.Find("Player").GetComponent<Player>();
            player.getPlayer().pPuntuacion += 10;
            TMP_Text textPuntuacion = (TMP_Text)GameObject.Find("Puntos").GetComponent<TMP_Text>();
            textPuntuacion.text = "Puntos: " + player.getPlayer().pPuntuacion;
        }
        else if (elOtro.gameObject.CompareTag("Bound"))
        {
            ReleaseEnemigo();
        }
    }
}
