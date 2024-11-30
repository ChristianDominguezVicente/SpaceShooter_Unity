using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class Disparo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Vector3 direccion;

    private float amplitud;
    private float frecuencia;
    private bool inversion;

    public float AmplitudOndas
    {
        get { return amplitud; }
        set { amplitud = value; }
    }

    public float FrecuenciaOndas
    {
        get { return frecuencia; }
        set { frecuencia = value; }
    }

    public bool InversionOndas
    {
        get { return inversion; }
        set { inversion = value; }
    }

    public Vector3 Direccion
    {
        get { return direccion; }
        set { direccion = value; }
    }

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
        if (CompareTag("DisparoPlayer"))
        {
            if (amplitud > 0f && frecuencia > 0f)
            {
                if (inversion)
                    transform.position += new Vector3(0, -Mathf.Sin(Time.time * frecuencia) * amplitud, 0);
                else
                    transform.position += new Vector3(0, Mathf.Sin(Time.time * frecuencia) * amplitud, 0);
            }  
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Bound"))
        {
            mypool.Release(this);
        }
    }
}
