using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.UI;

[RequireComponent(typeof(XRInputData))]
public class InputTest : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] ActionBasedController _left;
    [SerializeField] ActionBasedController _right;

    [SerializeField] Transform leftHandle;
    [SerializeField] Transform rightHandle;

    [SerializeField] GameObject gameStartCanvas;
    [SerializeField] Image aButtonImage;
    [SerializeField] Image xButtonImage;

    [SerializeField] Transform player;

    [Header("Variable")]
    [SerializeField] float XrotateSpeed = 120f;
    [SerializeField] float ZrotateSpeed = 2.0f;

    XRIDefaultInputActions _actions;

    Vector3 _leftControllerPositionOffset;
    Vector3 _rightControllerPositionOffset;
    Quaternion _rightControllerRotationOffset;

    private XRInputData _inputData;
    [SerializeField] private bool _leftSetupFinishied;
    [SerializeField] private bool _rightSetupFinishied;

    private void Awake()
    {
        _inputData = GetComponent<XRInputData>();
    }

    private void Start()
    {
        //_leftControllerPositionOffset = new Vector3(0, 0, _left.transform.position.z);
        //_rightControllerRotationOffset = new Quaternion(0, 0, _left.transform.rotation.z, 0);
        StartCoroutine(C_SetOffset());
    }

    IEnumerator C_SetOffset()
    {
        // 오른쪽 트리거 버튼 누를때까지 기다림
        //yield return new WaitUntil(() =>
        //{
        //    if (_inputData.leftController.TryGetFeatureValue(CommonUsages.triggerButton, out bool leftTriggerButton))
        //    {
        //        if (leftTriggerButton)
        //            return true;
        //    }

        //    return false;
        //});

        Time.timeScale = 0f;

        yield return new WaitUntil(() =>
        {
#if UNITY_EDITOR    
            if (Input.GetKeyDown(KeyCode.A))
            {
                aButtonImage.color = new Color(aButtonImage.color.r, aButtonImage.color.g, aButtonImage.color.b, 50f / 255f);
                _leftSetupFinishied = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                xButtonImage.color = new Color(xButtonImage.color.r, xButtonImage.color.g, xButtonImage.color.b, 50f / 255f);
                _rightSetupFinishied = true;
            }
#else
            if (_inputData.leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftButton))
            {
                if (leftButton)
                {
                    aButtonImage.color = new Color(aButtonImage.color.r, aButtonImage.color.g, aButtonImage.color.b, 50f / 255f);
                    _leftSetupFinishied = true;

                    if (_inputData.leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 leftPosition))
                    {
                        _leftControllerPositionOffset = leftPosition;
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
            }
            if (_inputData.rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightButton))
            {
                if (rightButton)
                {
                    xButtonImage.color = new Color(xButtonImage.color.r, xButtonImage.color.g, xButtonImage.color.b, 50f / 255f);
                    _rightSetupFinishied = true;

                    if (_inputData.rightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 rightPosition))
                    {
                        //_rightControllerPositionOffset = rightPosition;
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
            }
#endif
            if (_leftSetupFinishied && _rightSetupFinishied)
            {
                Time.timeScale = 1f;
                gameStartCanvas.SetActive(false);
                return true;
            }

            return false;

            //if (_inputData.leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 leftPosition) &&
            //    _inputData.leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion leftRotation))
            //{
            //    _leftControllerPositionOffset = leftPosition;
            //    _rightControllerRotationOffset = leftRotation;
            //    _setupFinishied = true;
            //}
            //else
            //{
            //    throw new System.Exception();
            //}

            //if (_inputData.leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 leftPosition))
            //{
            //    _leftControllerPositionOffset = leftPosition;
            //    _leftSetupFinishied = true;
            //}
            //else
            //{
            //    throw new System.Exception();
            //}

            //if (_inputData.rightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 rightPosition))
            //{
            //    //_rightControllerPositionOffset = rightPosition;
            //    _rightSetupFinishied = true;
            //}
            //else
            //{
            //    throw new System.Exception();
            //}
        });
    }

    private void Update()
    {
        if (!_leftSetupFinishied || !_rightSetupFinishied)
            return;

        // 왼쪽 조이스틱 앞뒤 이동 값.
        Vector3 rotX = _left.positionAction.action.ReadValue<Vector3>() - _leftControllerPositionOffset;
        player.rotation *= Quaternion.Euler(rotX.z * XrotateSpeed, 0, 0);
        //leftHandle.rotation = Quaternion.Euler((_left.transform.position.z - _leftControllerPositionOffset.z) * Mathf.Rad2Deg, 0, 0);
        leftHandle.localPosition = new Vector3(leftHandle.localPosition.x, leftHandle.localPosition.y, Mathf.Clamp(rotX.z / 2f, -0.08f, 0.08f));

        // 오른쪽 조이스틱 좌우 회전 값.
        Quaternion rotZ = _right.rotationAction.action.ReadValue<Quaternion>();
        player.rotation *= Quaternion.Euler(0, 0, -rotZ.z * ZrotateSpeed);
        rightHandle.localRotation = new Quaternion(rightHandle.localRotation.x, rightHandle.localRotation.y, Mathf.Clamp(-_right.transform.localRotation.z, -0.4f, 0.4f), rightHandle.localRotation.w);
    }
}
