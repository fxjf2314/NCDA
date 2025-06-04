using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUI : MonoBehaviour
{
    public Button volumnon;
    public Button volumnoff;
    public Slider volumn;


    void Start()
    {
        GameSet.Instance.volumn = PlayerPrefs.GetFloat("Volumn", 100);
        volumn.value=GameSet.Instance.volumn;
    }


    void Update()
    {
        if (GameSet.Instance.openvolumn)
        {
            volumnon.gameObject.SetActive(true);
            volumnoff.gameObject.SetActive(false);
        }
        else
        {
            volumnon.gameObject.SetActive(false);
            volumnoff.gameObject.SetActive(true);
        }
    }

    public void Openvolumn()
    {
        if (PlayerPrefs.GetFloat("Volumn", 100) != 0)
        {
            GameSet.Instance.volumn = PlayerPrefs.GetFloat("Volumn", 100);
            volumn.value = GameSet.Instance.volumn;
        }
        else
        {
            GameSet.Instance.volumn = 100;
            volumn.value = 100;
            PlayerPrefs.DeleteKey("Volumn");
            PlayerPrefs.SetFloat("Volumn", GameSet.Instance.volumn);
            PlayerPrefs.Save();
        }
    }

    public void Offvolumn()
    {
        GameSet.Instance.volumn = 0;
        volumn.value = 0;
        PlayerPrefs.DeleteKey("Volumn");
        PlayerPrefs.SetFloat("Volumn", GameSet.Instance.volumn);
        PlayerPrefs.Save();
    }

    public void ChangeVolumn()
    {
        GameSet.Instance.volumn=volumn.value;
        PlayerPrefs.DeleteKey("Volumn");
        PlayerPrefs.SetFloat("Volumn", GameSet.Instance.volumn);
        PlayerPrefs.Save();
    }
}
