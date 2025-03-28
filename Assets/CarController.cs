using Fusion;
using System;
using UnityEngine;

public class CarController : NetworkBehaviour
{
    [Header("Car Settings")]
    [SerializeField] public float carSpeed = 80f;
    [SerializeField] public float turnSpeed = 60f;
    [SerializeField] private float reverseSpeed = 60f;
    [SerializeField] private float acceleration = 40;

    private float targetSpeed;
    private float currentSpeed = 0f;

    private Rigidbody rb;
    private NetworkInputData inputData;

    public override void Spawned()
    {
        rb = GetComponent<Rigidbody>();    
    }

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData inputData))
        {
            this.inputData = inputData;

            HandleRotation(inputData);
            HandleMovement(inputData);
        }
    }


    private void HandleRotation(NetworkInputData inputData)
    {
        if(!Object.HasInputAuthority)
        {
            return;
        }

        float horizontalInput = inputData.horizontalInput;
        float rbVelocityMag = rb.velocity.magnitude;

        //Araba max h?zda ise de?er 1 olacak. duruyorsa 0 olacak. ne kadar h?zl? gidiyor ise o kadar kolay sapacak. 
        float speedFactor = Mathf.Clamp01(rbVelocityMag / carSpeed);

        float steerAmount = horizontalInput * speedFactor * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, steerAmount, 0);

        rb.MoveRotation(rb.rotation * turnRotation);
    }
    private void HandleMovement(NetworkInputData inputData)
    {
        if (!Object.HasInputAuthority)
        {
            return;
        }

        if (inputData.isReversing)
        {
            targetSpeed = -reverseSpeed;
        }
        else if (inputData.horizontalInput == 0)
        {
            targetSpeed = carSpeed;
        }
        else
        {
            targetSpeed = carSpeed * 0.6f;
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

        Vector3 velocity = transform.forward * currentSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentSpeed = 0;
        targetSpeed = 0;
    }

    private void OnCollisionExit(Collision collision)
    {
        targetSpeed = carSpeed;
    }

}
