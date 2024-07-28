using UnityEngine;

public class DTLoadFirst : MonoBehaviour
{
    private void Start()
    {
        DTControl.api.Init();
    }
}
