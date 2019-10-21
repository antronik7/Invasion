using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTurnPanel : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI m_text;

    public void Instantiate(ETeam teamTurn)
    {
        gameObject.SetActive(true);
        if (teamTurn == ETeam.Green)
        {
            m_text.text = "Green player's turn";
        }
        else if (teamTurn == ETeam.Red)
        {
            m_text.text = "Red player's turn";
        }
        StartCoroutine(ClosePanel(3));

    }

    IEnumerator ClosePanel(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
