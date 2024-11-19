using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Windows;

public class Disparo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Vector3 direccion;

    private ObjectPool<Disparo> mypool;

    public ObjectPool<Disparo> MyPool { get => mypool; set => mypool = value; }

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(this.gameObject, 4);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);

        timer += Time.deltaTime;

        if (timer >= 4)
        {
            timer = 0;
            mypool.Release(this);
        }
    }
}
