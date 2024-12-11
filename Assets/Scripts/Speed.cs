using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Speed : MonoBehaviour
{
    [SerializeField] private float velocidad;

    private ObjectPool<Speed> poolspeed;

    public ObjectPool<Speed> PoolSpeed { get => poolspeed; set => poolspeed = value; }

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
            poolspeed.Release(this);
        }
    }
}
