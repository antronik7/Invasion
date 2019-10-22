using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController m_instance;
    private bool m_isLaunched;

    void Awake()
    {
        if (m_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
        }
    }

    [SerializeField]
    private CharacterController m_characterController;

    [SerializeField]
    private float pullMinThreshold = 0.1f;
    public float GetPullMinThreshold() { return pullMinThreshold; }
    [SerializeField]
    private float pullMaxThreshold = 1f;

    private Vector3 aimRootPosition;
    private bool isAiming = false;

    public void AssignCharacterController(CharacterController controller)
    {
        m_characterController = controller;
        m_isLaunched = false;
    }

    // Update is called once per frame
    void Update()
    {
        float magnitude = CalculateForceMangitude();
        Vector3 direction = CalculateDirection();
        Vector3 inverseDirection = direction * -1f;

        //Debug.Log(magnitude);

        if (m_characterController != null)
        {
            m_characterController.UpdateForceMagnitude(magnitude);
            m_characterController.UpdateLaunchDirection(direction * -1f);
        }

        if (!m_isLaunched)
        {
            ManageInput();
        }
    }

    private void ManageInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;
            aimRootPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            isAiming = false;
            if (m_characterController != null)
            {
                if(m_characterController.LaunchCharacter())
                {
                    m_isLaunched = true;
                }
            }
        }
    }

    private float CalculateForceMangitude()
    {
        if (!isAiming)
            return 0f;

        float currentMagnitude = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - aimRootPosition).magnitude;
        currentMagnitude = Mathf.Clamp(currentMagnitude - pullMinThreshold, 0f, pullMaxThreshold + pullMinThreshold);
        return currentMagnitude;
    }

    private Vector3 CalculateDirection()
    {
        return (Camera.main.ScreenToWorldPoint(Input.mousePosition) - aimRootPosition).normalized;
    }

    public void OnMovementPhaseEnded()
    {
        GameManager.m_instance.StartTurn();
    }
}