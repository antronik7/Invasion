using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchdownZone : MonoBehaviour
{
    [SerializeField]
    protected ETeam m_team;

    public bool IsSameTeam(ETeam team)
    {
        return m_team == team;
    }
}

public enum ETeam
{
    Green,
    Red,
    Count
}