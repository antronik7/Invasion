using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poque : MonoBehaviour
{
    [SerializeField]
    protected float m_speedMin;
    protected bool m_hasSheep;
    [SerializeField]
    protected SpriteRenderer m_sheepSprite;
    protected Vector3 m_initialPosition;

    void Start()
    {
        m_initialPosition = transform.position;
    }

    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude < m_speedMin)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        m_sheepSprite.gameObject.SetActive(m_hasSheep);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var prize = collision.gameObject.GetComponent<Prize>();

        if (prize != null)
        {
            CapturePrize(collision.gameObject.GetComponent<Prize>());
        }
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
        Respawn(3);
    }
    
    IEnumerator Respawn(float time)
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        transform.position = m_initialPosition;
        gameObject.SetActive(true);
    }

    void CapturePrize(Prize prize)
    {
        m_hasSheep = true;
        prize.GetCaptured();
    }
}
