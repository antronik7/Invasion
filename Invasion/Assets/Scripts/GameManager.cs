using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager m_instance;
    [SerializeField]
    private ScorePanel m_scorePanel;
    [SerializeField]
    private Animator m_bluePlayerTurnPanel;
    [SerializeField]
    private Animator m_redPlayerTurnPanel;
    [SerializeField]
    private int m_turnIndex;
    [SerializeField]
    private Player m_player1;
    [SerializeField]
    private Player m_player2;

    void Awake()
    {
        if (m_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
        }
    }

    void Start()
    {
        StartTurn();
    }

    public void StartTurn()
    {
        m_turnIndex++;
        GameplayController.m_instance.AssignCharacterController(null);
        if (m_turnIndex % 2 == 1)
        {
            m_player2.EndTurn();
            m_player1.StartTurn();
            m_bluePlayerTurnPanel.SetTrigger("SlideFromLeft");
        }
        else
        {
            m_player1.EndTurn();
            m_player2.StartTurn();
            m_redPlayerTurnPanel.SetTrigger("SlideFromRight");
        }
    }

    protected int m_redScore;
    [SerializeField]
    protected int m_greenScore;

    public void Touchdown(ETeam team)
    {
        if (team == ETeam.Green)
        {
            m_greenScore++;
        }
        else
        {
            m_redScore++;
        }
        m_scorePanel.Instantiate(m_greenScore, m_redScore);
    }
}
