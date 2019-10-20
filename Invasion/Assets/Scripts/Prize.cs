using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    Vector3 m_initialPosition;

    private void Start()
    {
        m_initialPosition = transform.position;
    }

    public void GetCaptured()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Free(bool returnToInitialPos)
    {
        if (returnToInitialPos)
        {
            transform.position = m_initialPosition;
        }
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
