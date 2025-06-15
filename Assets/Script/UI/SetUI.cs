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
        DontDestroyOnLoad(gameObject);
        GameSet.volume = PlayerPrefs.GetFloat("Volumn", 100);
        volumn.value=GameSet.volume;
    }


    void Update()
    {
        if (GameSet.volume == 0)
        {
            GameSet.openvolume = false;
        }
        else
        {
            GameSet.openvolume = true;
        }

        if (GameSet.openvolume)
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
            GameSet.volume = PlayerPrefs.GetFloat("Volumn", 100);
            volumn.value = GameSet.volume;
        }
        else
        {
            GameSet.volume = 100;
            volumn.value = 100;
            PlayerPrefs.DeleteKey("Volumn");
            PlayerPrefs.SetFloat("Volumn", GameSet.volume);
            PlayerPrefs.Save();
        }
    }

    public void Offvolumn()
    {
        GameSet.volume = 0;
        volumn.value = 0;
        PlayerPrefs.DeleteKey("Volumn");
        PlayerPrefs.SetFloat("Volumn", GameSet.volume);
        PlayerPrefs.Save();
    }

    public void ChangeVolumn()
    {
        GameSet.volume =volumn.value;
        PlayerPrefs.DeleteKey("Volumn");
        PlayerPrefs.SetFloat("Volumn", GameSet.volume);
        PlayerPrefs.Save();
    }
}
