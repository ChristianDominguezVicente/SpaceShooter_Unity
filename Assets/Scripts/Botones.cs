using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Botones : MonoBehaviour
{
    [SerializeField] private Button normal;
    [SerializeField] private Button ondas;
    [SerializeField] private Button cuadra;

    private Button actual;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Puntuacion") < 100)
        {
            ondas.interactable = false;
            cuadra.interactable = false;
        }
        else if (PlayerPrefs.GetInt("Puntuacion") < 200)
            cuadra.interactable = false;

        switch(PlayerPrefs.GetInt("Disparo", 1))
        {
            case 1:
                Activar(normal); break;
            case 2:
                Activar(ondas); break;
            case 3:
                Activar(cuadra); break;
        }

        normal.onClick.AddListener(() => Activar(normal, 1));
        ondas.onClick.AddListener(() => Activar(ondas, 2));
        cuadra.onClick.AddListener(() => Activar(cuadra, 3));
    }

    private void Activar(Button nuevo, int disparo = 0)
    {
        if (actual != null)
        {
            ColorBlock colors = actual.colors;
            colors.normalColor = new Color(140 / 255f, 128 / 255f, 211 / 255f);
            colors.highlightedColor = new Color(122 / 255f, 102 / 255f, 238 / 255f);
            colors.pressedColor = new Color(84 / 255f, 58 / 255f, 233 / 255f);
            actual.colors = colors;
        }

        actual = nuevo;
        ColorBlock newColors = actual.colors;
        newColors.normalColor = new Color(84 / 255f, 58 / 255f, 233 / 255f);
        newColors.highlightedColor = new Color(64 / 255f, 48 / 255f, 200 / 255f);
        newColors.pressedColor = new Color(48 / 255f, 36 / 255f, 160 / 255f);
        actual.colors = newColors;
        actual.GetComponent<Graphic>().SetAllDirty();

        if (disparo > 0)
        {
            PlayerPrefs.SetInt("Disparo", disparo);
            PlayerPrefs.Save();
        }

    }

}
