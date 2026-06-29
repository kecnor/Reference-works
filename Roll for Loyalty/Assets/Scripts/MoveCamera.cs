using UnityEngine;
using UnityEngine.InputSystem;

public class moveCamera : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            transform.position = new Vector3(transform.position.x + 0.01f, transform.position.y, transform.position.z);
        }
        if (Keyboard.current.downArrowKey.isPressed)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            transform.position = new Vector3(transform.position.x - 0.01f, transform.position.y, transform.position.z);
        }
    }
}
