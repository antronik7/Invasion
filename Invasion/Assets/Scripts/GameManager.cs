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
    private GameObject m_bluePlayerWinPanel;
    [SerializeField]
    private GameObject m_redPlayerWinPanel;
    [SerializeField]
    private int m_scoreToWin = 3;
    [SerializeField]
    private float m_ScoreUpdateDelay = 0.5f;
    [SerializeField]
    private float m_StartGameDelay = 1.0f;
    [SerializeField]
    private float m_LoadSceneDelay = 1.0f;
    [SerializeField]
    private int m_turnIndex;
    [SerializeField]
    private Player m_player1;
    [SerializeField]
    private Player m_player2;
    [SerializeField]
    private List<Transform> m_flagSpawnPoints;
    [SerializeField]
    private Button m_restartBtn;

    [SerializeField]
    private float m_nextTurnIdleTimer;
    private float m_currentIdleTimer;
    private bool m_currentlyIdle;

    [SerializeField]
    private bool m_isGameplayEnable = true;
    private bool m_scoredThisTurn = false;

    protected int m_greenScore;
    protected int m_redScore;

    [SerializeField]
    private Transform m_curtainUpAnchor;
    [SerializeField]
    private Transform m_curtainBotAnchor;
    [SerializeField]
    private Transform m_curtainLeftAnchor;
    [SerializeField]
    private Transform m_curtainRightAnchor;
    [SerializeField]
    private GameObject m_curtain;
    [SerializeField]
    private Transform m_curtainMidAnchor;
    [SerializeField]
    private float m_curtainCallDuration;

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
        if (m_isGameplayEnable)
            Invoke("StartTurn", m_StartGameDelay);

        CurtainCall(EDirection.Right, true);
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
        m_bluePlayerWinPanel.GetComponent<Animator>().SetTrigger("SlideLeft");
        m_redPlayerWinPanel.GetComponent<Animator>().SetTrigger("SlideRight");

        if(m_greenScore > m_redScore)
        {
            m_bluePlayerWinPanel.GetComponent<Animator>().SetTrigger("Win");
            m_bluePlayerWinPanel.GetComponent<Text>().text = "Win";
            m_bluePlayerWinPanel.GetComponent<Text>().color = new Color(255f / 255f, 236f / 255f, 39f / 255f);
            m_redPlayerWinPanel.GetComponent<Animator>().SetTrigger("Lose");
            m_redPlayerWinPanel.GetComponent<Text>().text = "Lose";
            m_redPlayerWinPanel.GetComponent<Text>().color = new Color(194f / 255f, 195f / 255f, 199f / 255f);
        }
        else
        {
            m_bluePlayerWinPanel.GetComponent<Animator>().SetTrigger("Lose");
            m_bluePlayerWinPanel.GetComponent<Text>().text = "Lose";
            m_bluePlayerWinPanel.GetComponent<Text>().color = new Color(194f / 255f, 195f / 255f, 199f / 255f);
            m_redPlayerWinPanel.GetComponent<Animator>().SetTrigger("Win");
            m_redPlayerWinPanel.GetComponent<Text>().text = "Win";
            m_redPlayerWinPanel.GetComponent<Text>().color = new Color(255f / 255f, 236f / 255f, 39f / 255f);
        }
        m_restartBtn.gameObject.SetActive(true);
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
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
    }
    
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    private void LaunchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    IEnumerator LoadScene(string sceneName)
    {
        CurtainCall(EDirection.Left);
        yield return new WaitForSeconds(m_LoadSceneDelay);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CurtainCall(EDirection direction, bool startFromMid = false)
    {
        if (m_curtain == null)
            return;

        switch (direction)
        {
            case EDirection.Down:
                m_curtain.transform.eulerAngles = new Vector3(0f, 0f, 90f);
                m_curtain.transform.position = m_curtainBotAnchor.position;
                break;
            case EDirection.Up:
                m_curtain.transform.eulerAngles = new Vector3(0f, 0f, 270);
                m_curtain.transform.position = m_curtainUpAnchor.position;
                break;
            case EDirection.Right:
                m_curtain.transform.eulerAngles = new Vector3(0f, 0f, 180f);
                m_curtain.transform.position = m_curtainRightAnchor.position;
                break;
            case EDirection.Left:
                m_curtain.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                m_curtain.transform.position = m_curtainLeftAnchor.position;
                break;
        }
        StartCoroutine(CurtainCall(startFromMid));
    }

    IEnumerator CurtainCall(bool startFromMid)
    {
        Vector3 targetPosition = m_curtainMidAnchor.position;
        if(startFromMid)
        {
            targetPosition = m_curtain.transform.position;
            m_curtain.transform.position = m_curtainMidAnchor.position;
        }

        float curtainCurrentTime = 0;
        Vector3 curtainMovement = new Vector3();
        curtainMovement = targetPosition - m_curtain.transform.position;
        while (curtainCurrentTime < m_curtainCallDuration)
        {
            m_curtain.transform.position += (curtainMovement / m_curtainCallDuration) * Time.deltaTime;
            curtainCurrentTime += Time.deltaTime;
            yield return null;
        }
    }

    public enum EDirection
    {
        Up,
        Down,
        Right,
        Left
    }
}
