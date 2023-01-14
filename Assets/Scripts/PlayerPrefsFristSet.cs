using SecPlayerPrefs;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerPrefsFristSet : MonoBehaviour
{

    public Image _gO;
    public Text _gO2;
    public Transform _gO3;

    private void Start()
    {

        StartCoroutine(LogoIntro());

        if (!SecurePlayerPrefs.HasKey("First_Initialize"))
        {
            SecurePlayerPrefs.SetBool("First_Initialize", true);
            Debug.Log("Criamos a primeira inicialização");
        }

        if (SecurePlayerPrefs.GetBool("First_Initialize"))
        {
            SecurePlayerPrefs.SetBool("B00_comprado", true);
            SecurePlayerPrefs.SetString("bola_Equip", "B00");
            SecurePlayerPrefs.SetBool("BE00_comprado", true);
            SecurePlayerPrefs.SetString("efeito_Equip", "BE00");
            SecurePlayerPrefs.SetBool("C00_comprado", true);
            SecurePlayerPrefs.SetString("uniforme_Equip", "C00");
            SecurePlayerPrefs.SetBool("F00_comprado", true);
            SecurePlayerPrefs.SetString("campo_Equip", "F00");
            SecurePlayerPrefs.SetBool("P00_comprado", true);
            SecurePlayerPrefs.SetString("trave_Equip", "P00");
            SecurePlayerPrefs.SetBool("RD00_comprado", true);
            SecurePlayerPrefs.SetString("real_Equip", "RD00");

            Debug.Log("Arrumamos tudo na inicialização");

            SecurePlayerPrefs.SetBool("First_Initialize", false);
        }
    }

    public void ResetPLayerPrefs()
    {
        SecurePlayerPrefs.DeleteAll();
    }

    IEnumerator LogoIntro()
    {
        _gO.DOFade(1, 2.0f);
        yield return new WaitForSeconds(2f);
        _gO2.DOFade(1, 1f);
        _gO2.DOText("GAMES", 3f, true,ScrambleMode.All);
        yield return new WaitForSeconds(5f);
        _gO3.DOLocalMoveY(Screen.width * 3, 2f);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Tween Tests");
    }

}
