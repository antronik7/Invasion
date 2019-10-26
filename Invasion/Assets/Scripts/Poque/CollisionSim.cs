using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSim : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        var poque = collision.GetComponent<Poque>();
        if (poque != null && poque != GetComponentInParent<Poque>())
        {
            TimeManager.m_instance.BeginSlow();
        }
    }

    public void UpdatePosition(Vector2 newPos)
    {
        newPos.Normalize();
        newPos *= 0.5f;
        transform.localPosition = newPos;
    }
}
