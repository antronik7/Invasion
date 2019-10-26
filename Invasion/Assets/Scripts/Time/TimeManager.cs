using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager m_instance;
    [SerializeField]
    private AnimationCurve m_slowAmount;
    [SerializeField]
    private float m_slowDuration;
    private float m_currentSlowTime;
    private bool m_inSlowMo;

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
        }
    }

    void Update()
    {
        if (m_inSlowMo)
        {
            m_currentSlowTime += Time.deltaTime;
            Time.timeScale = m_slowAmount.Evaluate(m_currentSlowTime / m_slowDuration);
        }
        if (m_currentSlowTime >= m_slowDuration)
        {
            m_inSlowMo = false;
        }
    }

    public void BeginSlow()
    {
        if (!m_inSlowMo)
        {
            m_inSlowMo = true;
            m_currentSlowTime = 0f;
        }
    }
}
