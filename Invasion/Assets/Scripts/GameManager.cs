using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager m_instance;
    [SerializeField]
    private ScorePanel m_scorePanel;
    [SerializeField]
    private GameObject m_bluePlayerTurnPanel;
    [SerializeField]
    private GameObject m_redPlayerTurnPanel;
    [SerializeField]
    private GameObject m_bluePlayerScorePanel;
    [SerializeField]
    private GameObject m_redPlayerScorePanel;
    [SerializeField]
    private int m_scoreToWin = 3;
    [SerializeField]
    private float m_ScoreUpdateDelay = 0.5f;
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

    private bool m_isGameplayStopped = false;

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
        if (m_isGameplayStopped)
            return;

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
            m_bluePlayerTurnPanel.GetComponent<Animator>().SetTrigger("SlideFromLeft");
        }
        else
        {
            m_player1.EndTurn();
            m_player2.StartTurn();
            if (m_redPlayerTurnPanel != null)
                m_redPlayerTurnPanel.GetComponent<Animator>().SetTrigger("SlideFromRight");
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

        if(m_greenScore >= m_scoreToWin || m_redScore >= m_scoreToWin)
        {
            EndGame();
        }
        else
        {
            m_bluePlayerScorePanel.GetComponent<Animator>().SetTrigger("ScoreLeft");
            m_redPlayerScorePanel.GetComponent<Animator>().SetTrigger("ScoreRight");

            StartCoroutine(UpdateScoreUI(team));
        }
    }

    private IEnumerator UpdateScoreUI(ETeam team)
    {
        yield return new WaitForSeconds(m_ScoreUpdateDelay);

        if (team == ETeam.Green)
        {
            m_bluePlayerScorePanel.GetComponent<Text>().text = m_greenScore.ToString();
            m_bluePlayerScorePanel.GetComponent<Animator>().SetTrigger("JumpUp");
        }
        else
        {
            m_redPlayerScorePanel.GetComponent<Text>().text = m_redScore.ToString();
            m_redPlayerScorePanel.GetComponent<Animator>().SetTrigger("JumpDown");
        }

        yield return new WaitForSeconds(m_ScoreUpdateDelay);

        m_bluePlayerScorePanel.GetComponent<Animator>().SetTrigger("GoDown");
        m_redPlayerScorePanel.GetComponent<Animator>().SetTrigger("GoUp");
    }

    public void EndGame()
    {
        m_isGameplayStopped = true;
        m_bluePlayerTurnPanel.GetComponent<Animator>().SetTrigger("SlideFromLeft");
        m_redPlayerTurnPanel.GetComponent<Animator>().SetTrigger("SlideFromRight");

        if(m_greenScore > m_redScore)
        {
            m_bluePlayerTurnPanel.GetComponent<Animator>().SetTrigger("Win");
            m_bluePlayerTurnPanel.GetComponent<Text>().text = "Win";
            m_bluePlayerTurnPanel.GetComponent<Text>().color = new Color(255f / 255f, 236f / 255f, 39f / 255f);
            m_redPlayerTurnPanel.GetComponent<Animator>().SetTrigger("Lose");
            m_redPlayerTurnPanel.GetComponent<Text>().text = "Lose";
            m_redPlayerTurnPanel.GetComponent<Text>().color = new Color(194f / 255f, 195f / 255f, 199f / 255f);
        }
        else
        {
            m_bluePlayerTurnPanel.GetComponent<Animator>().SetTrigger("Lose");
            m_bluePlayerTurnPanel.GetComponent<Text>().text = "Lose";
            m_bluePlayerTurnPanel.GetComponent<Text>().color = new Color(194f / 255f, 195f / 255f, 199f / 255f);
            m_redPlayerTurnPanel.GetComponent<Animator>().SetTrigger("Win");
            m_redPlayerTurnPanel.GetComponent<Text>().text = "Win";
            m_redPlayerTurnPanel.GetComponent<Text>().color = new Color(255f / 255f, 236f / 255f, 39f / 255f);
        }
    }
}
