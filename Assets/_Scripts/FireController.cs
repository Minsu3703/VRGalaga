using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(XRInputData))]
public class FireController : MonoBehaviour
{
    private XRInputData _inputData;

    [SerializeField] Transform leftFirePosition;
    [SerializeField] Transform rightFirePosition;
    [SerializeField] Transform aimTarget;
    [SerializeField] Transform aimHUD;

    [SerializeField] float fireDelay = 0.5f;
    //[SerializeField] float damage = 10f;

    [SerializeField] float autoAimDistance = 300f;
    [SerializeField] float sightAngle = 50f;

    Vector3 aimPosition;
    int enemyLayerMask;
    int aimPanelLayerMask;
    int raycastBlockerLayerMask;

    float leftFireCoolDown = 0f;
    float rightFireCoolDown = 0f;

    private void Awake()
    {
        _inputData = GetComponent<XRInputData>();
        enemyLayerMask = LayerMask.NameToLayer("Enemy");
        aimPanelLayerMask = LayerMask.NameToLayer("AimPanel");
        raycastBlockerLayerMask = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void Update()
    {
        if (leftFireCoolDown <= fireDelay)
        {
            leftFireCoolDown += Time.deltaTime;
        }
        if (rightFireCoolDown <= fireDelay)
        {
            rightFireCoolDown += Time.deltaTime;
        }

        Fire();

        SearchEnemy();
    }

    private void SearchEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position + (transform.forward * autoAimDistance / 2), autoAimDistance / 2, 1 << enemyLayerMask);

        if (enemies == null)
        {
            aimPosition = aimTarget.position;

            if (aimHUD.gameObject.activeSelf)
            {
                aimHUD.gameObject.SetActive(false);
            }

            return;
        }

        Vector3[] enemyPositionInSight = new Vector3[enemies.Length];

        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 enemyDir = (enemies[i].transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, enemyDir);

            if (dot > Mathf.Cos(sightAngle * Mathf.Deg2Rad) && !Physics.Raycast(Camera.main.transform.position, (aimPosition - transform.position).normalized, 10f, 1 << raycastBlockerLayerMask))
            {
                enemyPositionInSight[i] = enemies[i].transform.position;
            }
            else
            {
                enemyPositionInSight[i] = Vector3.zero;
            }
        }

        if (enemyPositionInSight != null)
        {
            Vector3 temp = Vector3.zero;
            foreach (Vector3 pos in enemyPositionInSight)
            {
                if (pos == Vector3.zero) continue;

                if (temp == Vector3.zero || Vector3.Distance(transform.position, pos) < Vector3.Distance(transform.position, temp))
                {
                    temp = pos;
                }
            }

            if (temp != Vector3.zero)
            {
                aimPosition = temp;

                Debug.DrawRay(Camera.main.transform.position, (aimPosition - transform.position).normalized * 10f, Color.red);
                if (Physics.Raycast(Camera.main.transform.position, (aimPosition - transform.position).normalized, out RaycastHit hit, 10f, 1 << aimPanelLayerMask))
                {
                    aimHUD.position = hit.point;
                    aimHUD.LookAt(transform.position);

                    if (!aimHUD.gameObject.activeSelf)
                    {
                        aimHUD.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                aimPosition = aimTarget.position;

                if (aimHUD.gameObject.activeSelf)
                {
                    aimHUD.gameObject.SetActive(false);
                }
            }
        }
    }

    private void Fire()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (leftFireCoolDown < fireDelay)
            {
                return;
            }
            ObjectManager.Instance.GetBullet(leftFirePosition.position, aimPosition);
            leftFireCoolDown = 0f;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (rightFireCoolDown < fireDelay)
            {
                return;
            }
            ObjectManager.Instance.GetBullet(rightFirePosition.position, aimPosition);
            rightFireCoolDown = 0f;
        }
#else
        if (_inputData.leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrib))
        {
            if (leftGrib)
            {
                if (leftFireCoolDown < fireDelay)
                {
                    return;
                }
                ObjectManager.Instance.GetBullet(leftFirePosition.position, aimPosition);
                leftFireCoolDown = 0f;
            }
        }

        if (_inputData.rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrib))
        {
            if (rightGrib)
            {
                if (rightFireCoolDown < fireDelay)
                {
                    return;
                }
                ObjectManager.Instance.GetBullet(rightFirePosition.position, aimPosition);
                rightFireCoolDown = 0f;
            }
        }
#endif
    }
}