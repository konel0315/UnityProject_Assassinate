using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public float boundary1 = -7f;
    public float boundary2 = 3f;
    public float boundary3 = 13f;
    public float area1Y = -12f;
    public float area2Y = -2f;
    public float area3Y = 8f;
    public float area4Y = 18f;
    private Vector3 camPosition;

    void Start()
    {
        camPosition = transform.position;
        camPosition.x = 0f;
        camPosition.z = -10f;
    }

    void LateUpdate()
    {
        float playerY = player.position.y;

        if (playerY < boundary1)
        {
            camPosition.y = area1Y;
        }
        else if (playerY < boundary2)
        {
            camPosition.y = area2Y;
        }
        else if (playerY < boundary3)
        {
            camPosition.y = area3Y;
        }
        else
        {
            camPosition.y = area4Y;
        }

        transform.position = camPosition;
    }
}