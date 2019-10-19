using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI m_text;

    public void Instantiate(int greenScore, int redScore)
    {
        gameObject.SetActive(true);
        m_text.text = greenScore.ToString() + " - " + redScore.ToString();
        StartCoroutine(ClosePanel(3));

    }

    IEnumerator ClosePanel(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
