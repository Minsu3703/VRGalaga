using UnityEngine;
using UnityEngine.XR;

[RequireComponent (typeof(XRInputData))]
public class AimController : MonoBehaviour
{
    private XRInputData _inputData;
    [SerializeField] Transform aimHUD;

    [SerializeField] float aimingDistance = 0.5f;
    
    [SerializeField] Vector3 centerVector;
    

    private void Awake()
    {
        _inputData = GetComponent<XRInputData>();

        centerVector = aimHUD.localPosition;
    }

    private void Update()
    {
        if (_inputData.leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftStick))
        {
            /*
            if (leftStick.x >= maxAimDistanceX)
            {
                leftStick.x = maxAimDistanceX;
            }
            if (leftStick.y <= maxAimDistanceY)
            {
                leftStick.y = maxAimDistanceY;
            }
            */

            if (leftStick != Vector2.zero)
            {
                aimHUD.position = (centerVector + new Vector3(leftStick.x, leftStick.y, 0)) * Time.deltaTime;
                
                aimHUD.position = new Vector3(Mathf.Clamp(aimHUD.position.x, centerVector.x-aimingDistance, centerVector.x + aimingDistance), 
                    Mathf.Clamp(aimHUD.position.y, centerVector.y - aimingDistance, centerVector.y + aimingDistance), aimHUD.position.z);
            }
            else if (leftStick == Vector2.zero)
            {
                aimHUD.position = centerVector;
            }
        }
    }
}
