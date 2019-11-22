using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCamera : MonoBehaviour
{
    [SerializeField]
    private Camera m_cam;
    [SerializeField]
    private List<Transform> m_inShotTransforms;
    [SerializeField]
    private float m_minimumShotSize;
    private Vector3 m_goal;
    private float m_sizeGoal;
    [SerializeField]
    private float m_lerpTime = 0.5f;
    private float m_currentLerpTime;
    private Vector3 m_initPos;
    private float m_initSize;
    private bool m_isLerping;

    public void ChangeTurn(List<Transform> transforms)
    {
        m_inShotTransforms.Clear();
        m_isLerping = true;
        m_initPos = transform.position;
        m_initSize = m_cam.orthographicSize;
        m_inShotTransforms = transforms;
        m_goal = new Vector3(0, FindYMiddle(), -10);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isLerping)
        {
            LerpToGoal();
        }
        else
        {
            transform.position = new Vector3(0, FindYMiddle(), -10);
        }
    }

    void LerpToGoal()
    {
        m_currentLerpTime += Time.deltaTime;
        if (m_currentLerpTime < m_lerpTime)
        {
            transform.position = Vector3.Lerp(m_initPos, m_goal, m_currentLerpTime/m_lerpTime);
            m_cam.orthographicSize = Mathf.Lerp(m_initSize, m_sizeGoal, m_currentLerpTime / m_lerpTime);
        }
        else
        {
            m_isLerping = false;
            m_currentLerpTime = 0f;
        }
    }

    float FindYMiddle()
    {
        if (m_inShotTransforms.Count == 0)
        {
            return 0f;
        }

        float minYValue = m_inShotTransforms[0].position.y;
        float maxYValue = m_inShotTransforms[0].position.y;

        foreach (var trans in m_inShotTransforms)
        {
            if (trans.position.y > maxYValue)
            {
                maxYValue = trans.position.y;
            }
            if (trans.position.y < minYValue)
            {
                minYValue = trans.position.y;
            }
        }
        float distance = (Mathf.Abs(minYValue - maxYValue)/2f) + 2;
        if (distance > m_minimumShotSize)
        {
            if (!m_isLerping)
                m_cam.orthographicSize = distance;
            else
                m_sizeGoal = distance;
        }
        else
        {
            if (!m_isLerping)
                m_cam.orthographicSize = m_minimumShotSize;
            else
                m_sizeGoal = m_minimumShotSize;
        }
        return (minYValue + maxYValue) / 2f;
    }
}
