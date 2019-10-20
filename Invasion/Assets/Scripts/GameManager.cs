using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager m_instance;
    [SerializeField]
    private ScorePanel m_scorePanel;
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

    void StartTurn()
    {
        m_turnIndex++;
        if (m_turnIndex % 2 == 1)
        {
            m_player1.StartTurn();
        }
    }

    public void EndTurn()
    {

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
