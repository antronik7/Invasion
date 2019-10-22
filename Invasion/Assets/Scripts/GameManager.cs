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
    private List<Poque> m_movingPoques = new List<Poque>();

    [SerializeField]
    private float m_nextTurnIdleTimer;
    private float m_currentIdleTimer;
    private bool m_currentlyIdle;

    protected int m_redScore;
    protected int m_greenScore;

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

    private void Update()
    {
        m_currentlyIdle = true;
        if (!GameplayController.m_instance.m_isLaunched)
            return;

        var poques = m_player1.GetPoques();
        poques.AddRange(m_player2.GetPoques());
        foreach (var poque in poques)
        {
            if (poque.GetComponent<Rigidbody2D>().velocity.magnitude >= 0.05)
            {
                m_currentlyIdle = false;
            }
        }
        if (m_currentlyIdle)
        {
            m_currentIdleTimer += Time.deltaTime;
            if (m_currentIdleTimer > m_nextTurnIdleTimer)
            {
                StartTurn();
                m_currentIdleTimer = 0;
            }
        }
        else
        {
            m_currentIdleTimer = 0;
        }
    }

    public void StartTurn()
    {
        m_turnIndex++;
        GameplayController.m_instance.StartTurn();
        if (m_turnIndex % 2 == 1)
        {
            m_player2.EndTurn();
            m_player1.StartTurn();
            if (m_bluePlayerTurnPanel != null)
            m_bluePlayerTurnPanel.SetTrigger("SlideFromLeft");
        }
        else
        {
            m_player1.EndTurn();
            m_player2.StartTurn();
            if (m_redPlayerTurnPanel != null)
                m_redPlayerTurnPanel.SetTrigger("SlideFromRight");
        }
    }

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
