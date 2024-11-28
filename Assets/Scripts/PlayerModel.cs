using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    protected float vida = 100;
    protected float puntuacion = 0;
    protected float velocidad = 5;

    public float pVida
    {
        get { return vida; }
        set { vida = value; }
    }
    public float pPuntuacion
    {
        get { return puntuacion; }
        set { puntuacion = value; }
    }
    public float pVelocidad
    {
        get { return velocidad; }
        set { velocidad = value; }
    }
}
