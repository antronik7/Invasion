﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private bool m_isTurn;
    [SerializeField]
    private Poque m_selectedPoque;
    [SerializeField]
    private ETeam m_team;

    void Update()
    {
        if (m_isTurn)
        {
            if (m_selectedPoque == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.up);
                    if (hits.Length != 0)
                    {
                        foreach (var hit in hits)
                        {
                            if (hit.collider != null)
                            {
                                Debug.Log(hit.transform.gameObject.name);
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
    }

    public void StartTurn()
    {
        m_isTurn = true;
        m_selectedPoque = null;
    }

    public void EndTurn()
    {
        m_isTurn = false;
        m_selectedPoque = null;
    }

    void TrySelectPoque(Poque poque)
    {
        if (m_selectedPoque == null && m_isTurn && poque.GetTeam() == m_team)
        {
            m_selectedPoque = poque;
            GameplayController.m_instance.AssignCharacterController(poque.GetComponent<CharacterController>());
        }
    }
}
