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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, FindYMiddle(), -10);
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
        float distance = Mathf.Abs(minYValue - maxYValue);
        if (distance > m_minimumShotSize)
        {
            m_cam.orthographicSize = distance;
        }
        else
        {
            m_cam.orthographicSize = m_minimumShotSize;
        }
        return (minYValue + maxYValue) / 2f;
    }
}
