using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character")]
public class CharacterData : ScriptableObject
{
    public EClass m_class;
    public float m_speedMin;
    public Sprite m_spriteRed;
    public Sprite m_spriteGreen;
    public float m_launchForceMultiplier;
    public float m_scale;
}

public enum EClass
{
    Regular,
    Fast,
    Beefy,
    DoubleDash,
    Archer,
    Kamikaze,
    Duplicating,
    Flipper,
    Ghost,
    Teleporter,
    ShieldGuy,
    Assassin,
    Valkyrie,
    WindSupport,
    Count
}