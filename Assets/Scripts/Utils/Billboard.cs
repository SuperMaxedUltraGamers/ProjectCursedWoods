using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camT;

    private void Awake()
    {
        camT = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.forward = camT.forward;
    }
}
