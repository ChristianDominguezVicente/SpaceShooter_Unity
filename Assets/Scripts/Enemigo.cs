using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float velocidad;
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
        // Det�n el coroutine al desactivar el enemigo
        if (disparosCoroutine != null)
        {
            StopCoroutine(disparosCoroutine);
            disparosCoroutine = null;
        }
    }

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

            Player player;
            player = GameObject.Find("Player").GetComponent<Player>();
            player.getPlayer().pPuntuacion += 5;
            TMP_Text textPuntuacion = (TMP_Text)GameObject.Find("Puntos").GetComponent<TMP_Text>();
            textPuntuacion.text = "Puntos: " + player.getPlayer().pPuntuacion;
        }
        else if (elOtro.gameObject.CompareTag("Bound"))
        {
            ReleaseEnemigo();
        }
    }
}
