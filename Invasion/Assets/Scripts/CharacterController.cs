using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public GameObject TEMP_arrow;
    public GameObject TEMP_pivot;

    public float launchForceMultiplier = 10f;

    private float forceMagnitude = 0f;
    private Vector3 launchDirection = Vector3.zero;

    private Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(forceMagnitude);
        UpdateArrow();
    }

    public void UpdateForceMagnitude(float value)
    {
        forceMagnitude = value;
    }

    public void UpdateLaunchDirection(Vector3 value)
    {
        launchDirection = value;
    }

    public void LaunchCharacter()
    {
        if (forceMagnitude <= 0f)
            return;

        rigidBody.AddForce(launchDirection * forceMagnitude * launchForceMultiplier);
    }

    private void UpdateArrow()
    {
        if(forceMagnitude != 0)
        {
            UpdateArrowScale();
            UpdateArrowRotation();
            TEMP_arrow.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            TEMP_arrow.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void UpdateArrowScale()
    {
        TEMP_arrow.transform.localScale = new Vector3(1 + forceMagnitude, 1 + forceMagnitude, TEMP_arrow.transform.localScale.z);
    }

    private void UpdateArrowRotation()
    {
        float rotZ = (Mathf.Atan2(launchDirection.y, launchDirection.x) * Mathf.Rad2Deg) - 90f;
        TEMP_pivot.transform.rotation = Quaternion.AngleAxis(rotZ, Vector3.forward);
    }
}
