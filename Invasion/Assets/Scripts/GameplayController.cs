using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public CharacterController TEMP_characterController;

    public float pullMinThreshold = 0.1f;
    public float pullMaxThreshold = 1f;

    private Vector3 aimRootPosition;
    private bool isAiming = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float magnitude = CalculateForceMangitude();
        Vector3 direction = CalculateDirection();
        Vector3 inverseDirection = direction * -1f;

        //Debug.Log(magnitude);

        TEMP_characterController.UpdateForceMagnitude(magnitude);
        TEMP_characterController.UpdateLaunchDirection(direction * -1f);

        ManageInput();
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
            TEMP_characterController.LaunchCharacter();
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
