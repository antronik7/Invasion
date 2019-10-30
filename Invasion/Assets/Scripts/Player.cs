using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private bool m_isTurn;
    [SerializeField]
    private ETeam m_team;
    [SerializeField]
    private List<Poque> m_poques = new List<Poque>();
    public List<Poque> GetPoques() { return m_poques; }

    public Poque m_selectedPoque;

    void Update()
    {
        if (m_isTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
                if (hits.Length != 0)
                {
                    foreach (var hit in hits)
                    {
                        if (hit.collider != null)
                        {
                            var poque = hit.transform.gameObject.GetComponent<Poque>();
                            if (poque != null)
                            {
                                TrySelectPoque(poque);
                            }
                        }
                    }
                }
            }
        }
    }

    public void StartTurn()
    {
        m_isTurn = true;
        m_selectedPoque = null;
        foreach (var poque in m_poques)
        {
            poque.ResetTurn();
            poque.GetComponent<CharacterController>().ActivateSelectionFeedback(true);
        }
    }

    public void EndTurn()
    {
        m_isTurn = false;
        m_selectedPoque = null;

        foreach (var poque in m_poques)
        {
            poque.GetComponent<CharacterController>().ActivateSelectionFeedback(false);
        }
    }

    void TrySelectPoque(Poque poque)
    {
        if (m_isTurn && poque.GetTeam() == m_team && (m_selectedPoque == null || !m_selectedPoque.GetIsLaunched()))
        {
            foreach (var otherPoque in m_poques)
            {
                otherPoque.GetComponent<CharacterController>().ActivateSelectionFeedback(false);
                otherPoque.GetComponent<CharacterController>().ActivateSelectedFeedback(false);
            }

            m_selectedPoque = poque;
            GameplayController.m_instance.AssignCharacterController(poque.GetComponent<CharacterController>());

            m_selectedPoque.GetComponent<CharacterController>().ActivateSelectedFeedback(true);
        }
    }

    public void ResetCharactersPosition()
    {
        foreach (var poque in m_poques)
        {
            poque.ResetPoquePosition();
        }
    }
}
