using UnityEngine;

public class CheatButtonActive : MonoBehaviour
{
    [SerializeField] private GameObject cheating;

    void Awake()
    {
        if (cheating.CompareTag("SP_Cheat"))
        { 
            gameObject.SetActive(cheating.GetComponent<SP_Cheating>().Cheat);
        }
        else if (cheating.CompareTag("MP_Cheat"))
        {
            gameObject.SetActive(cheating.GetComponent<MP_Cheating>().Cheat);
        }
    }
}