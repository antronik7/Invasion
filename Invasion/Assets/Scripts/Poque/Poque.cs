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
    protected Animator m_animator;
    public bool GetIsLaunched() { return m_launched; }
    [SerializeField]
    protected CharacterData m_characterData;
    [SerializeField]
    protected CharacterController m_characterController;
    [SerializeField]
    protected CollisionSim m_collisionSimulator;
    [SerializeField]
    protected float m_fallingTime;
    [SerializeField]
    protected Collider2D m_centerCollider;
    private bool m_isDying = false;

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
        if (GetComponent<Rigidbody2D>().velocity.magnitude < m_speedMin)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        if (m_launched)
        {
            m_collisionSimulator.UpdatePosition(GetComponent<Rigidbody2D>().velocity);
            if (m_animator != null)
            {
                m_animator.SetFloat("Velocity", Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.magnitude / 5.0f, 0.3f, 0.8f));
            }
        }
        else
        {
            if (m_animator != null)
            {
                m_animator.SetFloat("Velocity", 0);
            }
        }
        m_sheepSprite.gameObject.SetActive(m_hasPrize);
    }

    public void Launch()
    {
        m_launched = true;
        m_collisionSimulator.enabled = true;
    }

    public void ResetTurn()
    {
        m_launched = false;
        m_collisionSimulator.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var prize = collision.gameObject.GetComponent<Prize>();

        if (prize != null && !prize.m_isCaptured)
        {
            CapturePrize(collision.gameObject.GetComponent<Prize>());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;

        if (other.GetComponents<Hole>() != null)
        {
            List<Collider2D> cols = new List<Collider2D>();
            if (m_centerCollider.OverlapCollider(filter, cols) > 0)
            {
                foreach (var col in cols)
                {
                    if (col.GetComponent<Hole>() != null)
                    {
                        Die();
                    }
                }
            }
        }

        var touchdownZone = other.GetComponent<TouchdownZone>();

        if (touchdownZone != null)
        {
            List<Collider2D> cols = new List<Collider2D>();
            if (m_centerCollider.OverlapCollider(filter, cols) > 0)
            {
                foreach (var col in cols)
                {
                    if (col.GetComponent<TouchdownZone>() != null)
                    {
                        if (m_hasPrize && touchdownZone.IsSameTeam(m_team))
                        {
                            Touchdown();
                        }
                    }
                }
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
        if (!m_isDying)
        {
            m_isDying = true;
            StartCoroutine(StopAndFall(m_fallingTime));
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

    IEnumerator StopAndFall(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(Respawn(0));
    }

    IEnumerator Respawn(float time)
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Respawning");
        ReturnSheep();
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(time);
        transform.position = m_initialPosition;
        GetComponent<SpriteRenderer>().enabled = true;
        m_isDying = false;
    }

    public void ReturnSheep()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var otherPoque = collision.collider.GetComponent<Poque>();
        if (otherPoque != null)
        {
            if (m_launched)
            {
                if (m_characterData != null)
                {
                    ExecuteOnCollisionAbility(m_characterData.m_class, otherPoque);
                }
            }
            StartCoroutine(CameraController.m_instance.CameraShake(0.1f));
        }
    }

    void ExecuteOnCollisionAbility(EClass poqueClasse, Poque otherPoque)
    {
        switch (poqueClasse)
        {
            case EClass.Archer:
                break;
            case EClass.Assassin:
                break;
            case EClass.Beefy:
                break;
            case EClass.DoubleDash:
                break;
            case EClass.Duplicating:
                break;
            case EClass.Fast:
                break;
            case EClass.Flipper:
                Flip(otherPoque);
                break;
            case EClass.Ghost:
                break;
            case EClass.Kamikaze:
                break;
            case EClass.Regular:
                break;
            case EClass.ShieldGuy:
                break;
            case EClass.Teleporter:
                break;
            case EClass.Valkyrie:
                break;
            case EClass.WindSupport:
                break;

        }
    }

    void Flip(Poque otherPoque)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        otherPoque.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        var distance = transform.position - otherPoque.transform.position;
        otherPoque.transform.position = transform.position + distance;
    }

    public void ResetPoquePosition()
    {
        transform.position = m_initialPosition;
    }
}
