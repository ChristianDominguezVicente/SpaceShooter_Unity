using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject menuOp;
    [SerializeField] private GameObject Fin;
    private bool juegoPausado = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Fin.activeSelf == false)
        {
            if (juegoPausado)
                Reanudar();
            else
                Pausa();
        }
    }

    public void Pausa()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        menuPausa.SetActive(true);
    }

    public void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
        menuOp.SetActive(false);
    }

    public void Salir()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Inicio");
        PlayerPrefs.SetInt("SalirJuego", 1);
    }
}
