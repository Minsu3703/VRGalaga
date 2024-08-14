using UnityEngine;

public class CockpitRotationIndicator : MonoBehaviour
{
    [SerializeField] Transform indicator;
    [SerializeField] Transform player;

    private void Update()
    {
        Vector3 playerRotation = player.localRotation.eulerAngles;
        indicator.localRotation = Quaternion.Euler(-playerRotation.x, 0f, -playerRotation.z);
    }
}
