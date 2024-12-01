using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Toggle pantalla;
    [SerializeField] private TMP_Dropdown calidad;
    [SerializeField] private GameObject menuInicio;
    [SerializeField] private GameObject menuNiveles;
    [SerializeField] private GameObject menuOpciones;

    private void Start()
    {
        if (PlayerPrefs.HasKey("VolumenMusica"))
            CargarMusica();
        else
            CambiarMusica();

        if (PlayerPrefs.HasKey("VolumenSFX"))
            CargarSFX();
        else
            CambiarSFX();

        if (PlayerPrefs.HasKey("Pantalla"))
            CargarPantalla();
        else
            PantallaCompleta();

        if (PlayerPrefs.HasKey("Calidad"))
            CargarCalidad();
        else
            CambiarCalidad();
    }

    public void PantallaCompleta()
    {
        Screen.fullScreen = pantalla.isOn;
        if (pantalla.isOn == false)
            PlayerPrefs.SetInt("Pantalla", 1);
        else
            PlayerPrefs.SetInt("Pantalla", 0);
    }

    public void CambiarMusica()
    {
        float volumen = musicSlider.value;
        audioMixer.SetFloat("Volumen", Mathf.Log10(volumen) * 20);
        PlayerPrefs.SetFloat("VolumenMusica", volumen);
    }

    public void CambiarSFX()
    {
        float volumen = SFXSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volumen) * 20);
        PlayerPrefs.SetFloat("VolumenSFX", volumen);
    }

    public void CambiarCalidad()
    {
        int index = calidad.value;
        PlayerPrefs.SetInt("Calidad", index);
        QualitySettings.SetQualityLevel(index);
    }

    private void CargarMusica()
    {
        musicSlider.value = PlayerPrefs.GetFloat("VolumenMusica");
        CambiarMusica();
    }

    private void CargarSFX()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("VolumenSFX");
        CambiarSFX();
    }

    private void CargarPantalla()
    {
        if (PlayerPrefs.GetInt("Pantalla") == 1)
            pantalla.isOn = false;
        else
            pantalla.isOn = true;
        PantallaCompleta();
    }

    private void CargarCalidad()
    {
        calidad.value = PlayerPrefs.GetInt("Calidad");
        CambiarCalidad();
    }

    public void MenuOp()
    {
        if (PlayerPrefs.GetInt("Menu") == 0)
        {
            menuInicio.SetActive(true);
            menuOpciones.SetActive(false);
        }
        else
        {
            menuNiveles.SetActive(true);
            menuOpciones.SetActive(false);
        }
    }

    public void CargarMenuOpIni()
    {
        PlayerPrefs.SetInt("Menu", 0);
    }

    public void CargarMenuOpNi()
    {
        PlayerPrefs.SetInt("Menu", 1);
    }
}
