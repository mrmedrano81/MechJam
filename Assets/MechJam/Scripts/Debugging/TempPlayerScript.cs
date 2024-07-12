using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement

    private void Update()
    {
        // Get input from keyboard
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float moveVertical = Input.GetAxis("Vertical"); // W/S or Up/Down arrow keys

        // Calculate movement vector
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        // Normalize movement vector to maintain consistent speed in all directions
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        // Move the player
        transform.Translate(movement, Space.World);
    }
}
