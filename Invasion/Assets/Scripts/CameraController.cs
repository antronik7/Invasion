using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController m_instance;
    public GameObject TEMP_character;
    public bool followPlayer = true;
    public float followSmoothTime = 0.3f;
    public float cameraMinY = 1f;
    public float cameraMaxY = 10f;

    public float m_camShakeDuration = 0.3f;
    [SerializeField]
    private AnimationCurve m_camShakeIntensity;

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
        }
    }
    
    void Update()
    {

    }

    private void LateUpdate()
    {
        float clampedY = Mathf.Clamp(transform.position.y, cameraMinY, cameraMaxY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }

    void FixedUpdate()
    {
        if (followPlayer)
            FollowPlayer();
    }

    private void FollowPlayer()
    {
        Poque currentTarget = GameManager.m_instance.GetCurrentPlayer().m_selectedPoque;
        if (currentTarget != null)
        {
            float targetY = currentTarget.transform.position.y;

            Vector3 velocity = Vector3.zero;//FUCK YOU UNITY
            Vector3 newCameraPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, newCameraPosition, ref velocity, followSmoothTime);
        }
    }
    
    public IEnumerator CameraShake(float intensity)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < m_camShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * intensity * m_camShakeIntensity.Evaluate(elapsed / m_camShakeDuration);
            float y = Random.Range(-1f, 1f) * intensity * m_camShakeIntensity.Evaluate(elapsed / m_camShakeDuration);

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
