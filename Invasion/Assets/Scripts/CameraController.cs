using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject TEMP_character;
    public bool followPlayer = true;
    public float followSmoothTime = 0.3f;

    public float cameraMinY = 1f;
    public float cameraMaxY = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {

    }

    void FixedUpdate()
    {
        if (followPlayer)
            FollowPlayer();
    }

    private void FollowPlayer()
    {
        float targetY = Mathf.Clamp(TEMP_character.transform.position.y, cameraMinY, cameraMaxY);

        Vector3 velocity = Vector3.zero;//FUCK YOU UNITY
        Vector3 newCameraPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition, ref velocity, followSmoothTime);
    }
}
