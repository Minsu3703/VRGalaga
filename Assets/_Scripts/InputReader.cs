using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// XR ��� �Է� ó�� ����
/// </summary>
[RequireComponent(typeof(XRInputData))]
public class InputReader : MonoBehaviour
{
    private XRInputData _inputData;

    // ������ �� �߰��� ���ϴ� ��.
    [SerializeField] float thrustForce = 1f;
    // ȸ���ϴ� �ӵ�.
    [SerializeField] float rotationSpeed = 20f;
    // �ִ� �̵� �ӵ�.
    [SerializeField] float maxMoveSpeed = 100f;
    // �ִ� ȸ�� �ӵ�.
    [SerializeField] float maxRotateSpeed = 10f;
    // ���� ����.
    [SerializeField] float deceleationRate = 0.333f;

    [SerializeField] Transform leftEngine;
    [SerializeField] Transform rightEngine;

    [SerializeField] TMPro.TextMeshPro testText;

    Rigidbody rb;

    bool isMoving = false;


    private void Start()
    {
        _inputData = GetComponent<XRInputData>();
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        //if (rb.velocity != Vector3.zero)
        //{
        //    isMoving = true;
        //}
        //else
        //{
        //    isMoving = false;
        //}
        isMoving = false;
        // FOR TEST
        testText.text = rb.velocity.magnitude.ToString();

        

        if (_inputData.leftController.TryGetFeatureValue(CommonUsages.trigger, out float leftTriggerValue))
        {
            if (leftTriggerValue >= 0.2f)
            {
                
                ApplyThrust(leftEngine.transform.position, rotationSpeed);
            }
            // test
            else if (_inputData.leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftStick))
            {
                if (leftStick !=  Vector2.zero)
                {
                    return;
                }
            }
        }

        if (_inputData.rightController.TryGetFeatureValue(CommonUsages.trigger, out float rightTriggerValue))
        {
            if (rightTriggerValue >= 0.2f)
            {
                ApplyThrust(rightEngine.transform.position, -rotationSpeed);
            }
            // test
            else if (_inputData.rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightStick))
            {
                if (rightStick != Vector2.zero)
                {
                    return;
                }
            }
        }

        if (rb.velocity.magnitude > maxMoveSpeed)
        {
            // �ʿ��ϸ� �߰��� ��, �ִ��̵��ӵ� ����.
        }
        if (rb.angularVelocity.magnitude > maxRotateSpeed)
        {
            // �ʿ��ϸ� �߰��� ��, �ִ�ȸ���ӵ� ����.
        }

#if UNITY_EDITOR
        // for test on PC
        if (Input.GetButton("Jump"))
        {
            ApplyFoward(thrustForce);
            isMoving = true;
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                ApplyThrust(rightEngine.transform.position, -rotationSpeed);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                ApplyThrust(leftEngine.transform.position, rotationSpeed);
            }
            isMoving = true;
        }
#endif

        if (leftTriggerValue >= 0.2f && rightTriggerValue >= 0.2f)
        {
            ApplyFoward(thrustForce);
            isMoving = true;
            //if (!isMoving)
            //{
            //    isMoving = true;
            //}
        }

        if (!isMoving)
        {
            rb.velocity *= deceleationRate;
            rb.angularVelocity *= deceleationRate;
        }




        //if (_inputData.leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool grib))
        //{
        //}

        //if (_inputData.rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool Abutton))
        //{
        //}

        //if (_inputData.rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool Bbutton))
        //{
        //}
    }

    void ApplyThrust(Vector3 enginePosition, float torque)
    {
        Vector3 forceDirection = transform.up;
        rb.AddTorque(forceDirection * torque * Time.deltaTime);
    }

    void ApplyFoward(float torque)
    {
        Vector3 forceDirection = transform.forward;
        rb.AddForce(forceDirection * torque * Time.deltaTime);
    }
}