using UnityEngine;

public class JetFighterController : MonoBehaviour
{
    public GameObject leftJetEngine;
    public GameObject rightJetEngine;
    public float thrustForce = 1000f;
    public float rotationSpeed = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        // Check if left or right jet engine is active
        bool isLeftEngineActive = Input.GetKey(KeyCode.A); // Press A to activate left jet engine
        bool isRightEngineActive = Input.GetKey(KeyCode.D); // Press D to activate right jet engine


        Quaternion playerRotation = transform.parent.rotation;
        

        if (isLeftEngineActive && !isRightEngineActive)
        {
            leftJetEngine.transform.localRotation = Quaternion.Inverse(playerRotation);
            // Activate left engine only, rotate clockwise
            ApplyThrust(leftJetEngine.transform.position, -rotationSpeed);
            
        }
        else if (!isLeftEngineActive && isRightEngineActive)
        {
            // Activate right engine only, rotate counterclockwise
            ApplyThrust(rightJetEngine.transform.position, rotationSpeed);
        }
        else if (isLeftEngineActive && isRightEngineActive)
        {
            // Both engines active, move forward
            rb.AddForce(transform.forward * thrustForce * Time.deltaTime);
        }
    }

    void ApplyThrust(Vector3 enginePosition, float torque)
    {
        Vector3 forceDirection = transform.forward;
        rb.AddForceAtPosition(forceDirection * thrustForce * Time.deltaTime, enginePosition);
        rb.AddTorque(transform.up * torque * Time.deltaTime);
    }
}