using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FadeScript:MonoBehaviour
{

    private static FadeScript minstance;
    public static FadeScript instance
    {
        get
        {
            if(minstance == null)
            {
                minstance = FindObjectOfType<FadeScript>();
                if(minstance == null)
                {
                    minstance = new FadeScript();
                }
            }

            return minstance;
        }
    }

    public void Fadein()
    {
        StartCoroutine(CoroutineFade(gameObject, 0, 0.5f, null));
    }
    public void Fadeout()
    {
        StartCoroutine(CoroutineFade(gameObject, 1, 0.5f, null));
    }
    public void Fade(GameObject fadeone, float aimalpha,float time, GameObject panel)
    {
        panel.SetActive(true);
        StartCoroutine(CoroutineFade(fadeone, aimalpha, time, panel));
    }
    public void Fade(GameObject fadeone, float aimalpha,float time)
    {
        StartCoroutine(CoroutineFade(fadeone, aimalpha, time, null));
    }
    public void Fadein(GameObject fadeone,float time,GameObject panel)
    {
        panel.SetActive(true);
        StartCoroutine(CoroutineFade(fadeone,0,time,panel));
    }
    public void Fadein(GameObject fadeone, float time)
    {
        StartCoroutine(CoroutineFade(fadeone, 0, time,null));
    }
    public void Fadein(GameObject fadeone)
    {
        StartCoroutine(CoroutineFade(fadeone, 0, 0.5f,null));
    }
    public void Fadeout(GameObject fadeone, float time, GameObject panel)
    {
        panel.SetActive(true);
        StartCoroutine(CoroutineFade(fadeone, 1, time, panel));
    }
    public void Fadeout(GameObject fadeone, float time)
    {
        StartCoroutine(CoroutineFade(fadeone, 1, time, null));
    }
    public void Fadeout(GameObject fadeone)
    {
        StartCoroutine(CoroutineFade(fadeone, 1, 0.5f, null));
    }

    //UI½¥Òþ½¥ÏÔÐ§¹û
    IEnumerator CoroutineFade(GameObject fadeone,float aimalpha,float time, GameObject panel) 
    {
        if (fadeone.GetComponent<CanvasGroup>() != null) 
        {
            fadeone.SetActive(true); 
            float itime = 0;
            float nowalpha = fadeone.GetComponent<CanvasGroup>().alpha;
            if (nowalpha >= aimalpha)
            {
                while (fadeone.GetComponent<CanvasGroup>().alpha > aimalpha)
                {
                    itime += Time.deltaTime;
                    fadeone.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(nowalpha, aimalpha, itime / time);
                    yield return null;
                }
                fadeone.GetComponent<CanvasGroup>().alpha=aimalpha;
                fadeone.SetActive(false);
                if (panel != null)
                {
                    panel.gameObject.SetActive(false);
                }
                //StopAllCoroutines();
                yield break;
            }
            else if(nowalpha < aimalpha)
            {
                while (fadeone.GetComponent<CanvasGroup>().alpha < aimalpha)
                {
                    itime += Time.deltaTime;
                    fadeone.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(nowalpha, aimalpha, itime / time);
                    yield return null;
                }
                fadeone.GetComponent<CanvasGroup>().alpha = aimalpha;
                if (panel != null)
                {
                    panel.gameObject.SetActive(false);
                }
                //StopAllCoroutines();
                yield break;
            }
        }
        else
        {
            Debug.Log("No CanvasGroup");
        }
    }
}
