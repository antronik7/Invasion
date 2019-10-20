using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool m_isTurn;
    private Poque m_selectedPoque;
    [SerializeField]
    private ETeam m_team;

    public void StartTurn()
    {
        if (m_isTurn)
        {
            if (m_selectedPoque != null)
            {
                for (var i = 0; i < Input.touchCount; ++i)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
                        // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
                        if (hitInfo)
                        {
                            if (hitInfo.transform.GetComponent<Poque>())
                            {
                                TrySelectPoque(hitInfo.transform.GetComponent<Poque>());
                            }
                        }
                    }
                }
            }
            else
            {

            }
        }
    }

    void TrySelectPoque(Poque poque)
    {
        if (m_selectedPoque == null && m_isTurn && m_selectedPoque.GetTeam() == m_team)
        {
            m_selectedPoque = poque;
        }
    }
}
