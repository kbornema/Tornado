using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AInputDevice
{
    [SerializeField]
    private float _movementSpeed = 20.0f;
    
    private Tornado _tornado;

    public void SetTornado(Tornado t)
    {
        _tornado = t;
    }
    
    public void UpdateDevice(float deltaTime)
    {
        Update(deltaTime);

        if(IsMoving())
        {
            Vector2 input = GetInput();

            if(input.x != 0.0f || input.y != 0.0f)
            {
                input.x = Mathf.Clamp(input.x, -1.0f, 1.0f);
                input.y = Mathf.Clamp(input.y, -1.0f, 1.0f);

                Vector3 forward;
                Vector3 right;

                GameCamera.Instance.GetMovement(out forward, out right);

                Vector3 moveDir = (forward * input.y + right * input.x) * _movementSpeed * deltaTime;

                //float rotation = input.x * _rotationSpeed;

                //_tornado.Rotate(0.0f, rotation, 0.0f, ForceMode.Impulse);

                _tornado.AddForce(moveDir, ForceMode.Impulse);
            }
        }

    }

    protected void ReleaseAttractedObjects()
    {
       // _tornado.ReleaseAllObjectsVelocity();
        _tornado.ReleaseAllObectsForward();
    }

    protected abstract void Update(float deltaTime);

    protected abstract bool IsMoving();

    protected abstract Vector2 GetInput();
}
