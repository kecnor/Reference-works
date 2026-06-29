using UnityEngine;

public class MoveChat : MonoBehaviour
{
    public void MoveChatLog(float x)
    {
        transform.position = new Vector3(x ,transform.position.y, 0);
    }
}
