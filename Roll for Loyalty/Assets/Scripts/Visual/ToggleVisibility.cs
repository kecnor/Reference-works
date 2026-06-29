using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    // Changes the gameobject visibility
    public void AppearObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void DisappearObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void ToggleObject(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    //Change the object from one to another
    public void ChangeObjects(GameObject disappear, GameObject appear)
    {
        DisappearObject(disappear);
        AppearObject(appear);
    }
}
