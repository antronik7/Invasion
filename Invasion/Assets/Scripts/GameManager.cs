using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private float m_StartGameDelay = 1.0f;
    [SerializeField]
    private int m_turnIndex;
    [SerializeField]
    private Player m_player1;
    [SerializeField]
    private Player m_player2;
    [SerializeField]
    private List<Transform> m_flagSpawnPoints;

    [SerializeField]
    private float m_nextTurnIdleTimer;
    private float m_currentIdleTimer;
    private bool m_currentlyIdle;

    private bool m_isGameplayEnable = true;
    private bool m_scoredThisTurn = false;

    protected int m_greenScore;
    protected int m_redScore;

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
        if (m_isGameplayEnable == false)
            return;

        m_currentlyIdle = true;
        if (!GameplayController.m_instance.m_isLaunched)
            return;

        var poques = new List<Poque>();
        poques.AddRange(m_player1.GetPoques());
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
                if(m_scoredThisTurn)
                {
                    m_scoredThisTurn = false;
                    GetCurrentPlayer().m_selectedPoque.ReturnSheep();
                    m_player1.ResetCharactersPosition();
                    m_player2.ResetCharactersPosition();
                }

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
        m_scoredThisTurn = false;
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
        if (m_scoredThisTurn)
        {
            return;
        }
        m_scoredThisTurn = true;

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
        EnableGameplay(false);
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

    public Vector3 GetClosestFlagSpawn(Vector3 flagPos)
    {
        float closestDistance = -1;
        int closestIndex = -1;
        int i = 0;
        foreach (var trans in m_flagSpawnPoints)
        {
            if (closestIndex == -1)
            {
                closestDistance = Vector3.Distance(flagPos, trans.position);
                closestIndex = i;
            }
            else
            {
                if (closestDistance > Vector3.Distance(flagPos, trans.position))
                {
                    closestIndex = i;
                    closestDistance = Vector3.Distance(flagPos, trans.position);
                }
            }
            i++;
        }
        return m_flagSpawnPoints[closestIndex].position;
    }

    public void ResetGame()
    {
        Debug.Log(SceneManager.GetActiveScene());
        SceneManager.LoadScene("Assets/Scenes/Antoine", LoadSceneMode.Single);
    }

    //This should be change to a solution not based on the turn index...
    public Player GetCurrentPlayer()
    {
        if (m_turnIndex % 2 == 1)
            return m_player1;
        else
            return m_player2;
    }

    public void EnableGameplay(bool value)
    {
        m_isGameplayEnable = value;
    }
}
