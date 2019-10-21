using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnPanel : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI m_text;
    [SerializeField]
    protected Color m_redColor;
    [SerializeField]
    protected Color m_greenColor;

    public void Instantiate(ETeam teamTurn)
    {
        gameObject.SetActive(true);
        if (teamTurn == ETeam.Green)
        {
            m_text.text = "Green player's turn";
            GetComponent<Image>().color = m_greenColor;
        }
        else if (teamTurn == ETeam.Red)
        {
            m_text.text = "Red player's turn";
            GetComponent<Image>().color = m_redColor;
        }
        StartCoroutine(ClosePanel(3));

    }

    IEnumerator ClosePanel(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
