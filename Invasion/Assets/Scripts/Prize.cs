using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    Vector3 m_initialPosition;
    bool m_isRoaming = true;
    [SerializeField]
    Vector3 m_roamingPosition;
    [SerializeField]
    protected float m_speed;
    [SerializeField]
    protected float m_roamingRadius;
    public bool m_isCaptured;

    private void Update()
    {
        WalkTowardRoamingPoint();
    }

    void WalkTowardRoamingPoint()
    {
        if (m_isRoaming)
        {
            float step = m_speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, m_roamingPosition, step);
        }
        if (Vector3.Distance(transform.position, m_roamingPosition) < 0.05f)
        {
            GenerateRoamingPoint();
        }
    }

    void GenerateRoamingPoint()
    {
        float randomX = Random.Range(-m_roamingRadius, m_roamingRadius);
        float randomY = Random.Range(-m_roamingRadius, m_roamingRadius);
        var vec = new Vector2(randomX, randomY);

        m_roamingPosition = vec;
    }

    private void Start()
    {
        m_initialPosition = transform.position;
    }

    public void GetCaptured()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        m_isRoaming = false;
        m_isCaptured = true;
    }

    public void Free(bool returnToInitialPos)
    {
        if (returnToInitialPos)
        {
            transform.position = m_initialPosition;
        }
        else
        {
            transform.position = GameManager.m_instance.GetClosestFlagSpawn(transform.position);
        }


        GetComponent<SpriteRenderer>().enabled = true;
        m_isRoaming = true;
        m_isCaptured = false;
    }
}
