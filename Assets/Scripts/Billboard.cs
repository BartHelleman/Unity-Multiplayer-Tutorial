using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        if (Camera.main)
            transform.LookAt(Camera.main.transform);
    }
}
