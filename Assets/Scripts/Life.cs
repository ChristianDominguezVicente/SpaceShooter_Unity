using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Life : MonoBehaviour
{
    [SerializeField] private float velocidad;

    private ObjectPool<Life> poollife;

    public ObjectPool<Life> PoolLife { get => poollife; set => poollife = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Bound"))
        {
            poollife.Release(this);
        }
    }
}
