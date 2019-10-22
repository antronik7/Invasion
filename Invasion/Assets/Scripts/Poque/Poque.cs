using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poque : MonoBehaviour
{
    [SerializeField]
    protected float m_speedMin;
    protected bool m_hasPrize;
    [SerializeField]
    protected SpriteRenderer m_sheepSprite;
    protected Vector3 m_initialPosition;
    protected Prize m_prizeRef;
    [SerializeField]
    protected ETeam m_team;
    protected bool m_launched;
    [SerializeField]
    protected CharacterData m_characterData;
    [SerializeField]
    protected CharacterController m_characterController;

    public ETeam GetTeam() { return m_team; }

    void Start()
    {
        m_initialPosition = transform.position;
        if (m_characterData != null)
        {
            m_speedMin = m_characterData.m_speedMin;
            m_characterController.launchForceMultiplier = m_characterData.m_launchForceMultiplier;
            GetComponent<Transform>().localScale = new Vector3(m_characterData.m_scale, m_characterData.m_scale, m_characterData.m_scale);
            if (m_team == ETeam.Green)
                GetComponent<SpriteRenderer>().sprite = m_characterData.m_spriteGreen;
            else
                GetComponent<SpriteRenderer>().sprite = m_characterData.m_spriteRed;
        }
    }

    void Update()
    {
        if (m_launched && GetComponent<Rigidbody2D>().velocity.magnitude < m_speedMin)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            OnStopMovement();
        }
        m_sheepSprite.gameObject.SetActive(m_hasPrize);
    }

    public void Launch()
    {
        m_launched = true;
    }

    void OnStopMovement()
    {
        m_launched = false;
        GameplayController.m_instance.OnMovementPhaseEnded();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var prize = collision.gameObject.GetComponent<Prize>();

        if (prize != null)
        {
            CapturePrize(collision.gameObject.GetComponent<Prize>());
        }

        var touchdownZone = collision.GetComponent<TouchdownZone>();

        if (touchdownZone != null)
        {
            if (m_hasPrize && touchdownZone.IsSameTeam(m_team))
            {
                Touchdown();
            }
        }
    }

    void Touchdown()
    {
        GameManager.m_instance.Touchdown(m_team);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var arena = collision.gameObject.GetComponent<Arena>();
        if (arena != null)
        {
            Die();
        }
    }

    void Die()
    {
        var respawn = Respawn(3);
        StartCoroutine(respawn);
    }
    
    IEnumerator Respawn(float time)
    {
        Debug.Log("Respawning");
        ReturnSheep();
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(time);
        transform.position = m_initialPosition;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void ReturnSheep()
    {
        m_hasPrize = false;
        if (m_prizeRef != null)
            m_prizeRef.Free(true);
        m_prizeRef = null;
    }

    void CapturePrize(Prize prize)
    {
        m_hasPrize = true;
        m_prizeRef = prize;
        prize.GetCaptured();
    }
}
