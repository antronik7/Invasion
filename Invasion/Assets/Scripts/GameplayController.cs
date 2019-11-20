using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public static GameplayController m_instance;
    [SerializeField]
    public bool m_isLaunched;

    private bool m_isDraggingCamera = false;
    private Vector3 m_previousMousePosition;

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
    }

    public void StartTurn()
    {
        m_isLaunched = false;
        AssignCharacterController(null);
    }

    // Update is called once per frame
    void Update()
    {
        float magnitude = CalculateForceMangitude();
        Vector3 direction = CalculateDirection();
        Vector3 inverseDirection = direction * -1f;

        if (m_characterController != null)
        {
            m_characterController.UpdateForceMagnitude(magnitude);
            m_characterController.UpdateLaunchDirection(direction * -1f);
        }

        if(GameManager.m_instance.GetCurrentPlayer().m_selectedPoque == null)
        {
            DragCamera();
        }
        else if (!m_isLaunched)//Replace this to a IF to allow draggin imediately after selecting the puck...
        {
            ManageInput();
        }
    }

    private void ManageInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;
            m_isDraggingCamera = false;
            aimRootPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0) && !m_isLaunched)
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

    private void DragCamera()
    {
        if (Input.GetMouseButton(0))
        {
            if (m_isDraggingCamera == false)
            {
                m_isDraggingCamera = true;
                m_previousMousePosition = Input.mousePosition;
            }

            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 dragDistance = (currentMousePosition - m_previousMousePosition) / 120f;//NEED TO FIND THE CORRECT RATIO...
            dragDistance = new Vector3(0f, dragDistance.y, 0f);
            if (CameraController.m_instance != null)
            {
                CameraController.m_instance.gameObject.transform.position = CameraController.m_instance.gameObject.transform.position - dragDistance;
            }
            m_previousMousePosition = currentMousePosition;
        }

        if(Input.GetMouseButtonUp(0))
        {
            m_isDraggingCamera = false;
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
}