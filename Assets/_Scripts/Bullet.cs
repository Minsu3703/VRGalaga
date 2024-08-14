using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float destroyTime = 5.0f;
    [SerializeField] private float moveSpeed = 150f;
    [SerializeField] private Vector3 aimTargetPosition;

    private float countTime;

    public void AimTargetPosition(Vector3 target)
    {
        aimTargetPosition = target;
    }

    Transform refTransform;

    private void Awake()
    {
        refTransform = transform;
    }

    private void OnEnable()
    {
        countTime = 0f;
    }

    private void Update()
    {
        refTransform.position = Vector3.MoveTowards(refTransform.position, aimTargetPosition, moveSpeed * Time.deltaTime);
        //refTransform.LookAt(aimTargetPosition);
        //refTransform.localPosition += new Vector3(0, 0, moveSpeed) * Time.deltaTime;

        countTime += Time.deltaTime;

        if (countTime >= destroyTime)
        {
            gameObject.SetActive(false);
        }
    }
}
