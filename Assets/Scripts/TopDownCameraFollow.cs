using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public float height = 10.0f; // Height of the camera above the player
    public float distance = 7.0f; // Distance behind the player
    public float angle = 45.0f; // Angle of the camera relative to the ground

    private Vector3 offset;

    private void Start()
    {
        // Calculate the initial offset at the start
        offset = Quaternion.Euler(angle, 0, 0) * new Vector3(0, 0, -distance);
        offset += Vector3.up * height;
        Debug.LogFormat("Camera offset: {0}", offset);
    }


    private void LateUpdate()
    {
        // Update the position of the camera to follow the player with the offset
        transform.position = player.position + offset;

        // Always look at the player
        transform.LookAt(player);
    }
}
