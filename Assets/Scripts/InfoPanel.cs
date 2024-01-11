using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public static InfoPanel instance;
    private TextMeshProUGUI text;
    private void Awake()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        instance = this;
        gameObject.SetActive(false);
    }

    public void ShowInfo(string info)
    {
        text.text = info;
        gameObject.SetActive(true);
        StartCoroutine(InfoCoroutine());
    }

    public IEnumerator InfoCoroutine()
    {
        yield return new WaitForSeconds(4);
        GetComponent<Animator>().Play("Disappearing");
    }
}
