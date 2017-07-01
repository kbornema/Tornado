using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DesktopInput : AInputDevice
{       
    protected override void Update(float deltaTime)
    {
        /*
        tornado.AddForce(new Vector3(moveDir.x, 0.0f, moveDir.y) * deltaTime * _speed, ForceMode.Impulse);

        if (Input.GetKey(KeyCode.Q))
        {
            tornado.ChangeConfPercent(-_changeConfPercent * deltaTime);
        }

        else if (Input.GetKey(KeyCode.E))
        {
            tornado.ChangeConfPercent(_changeConfPercent * deltaTime);
        }

         * */
        if (Input.GetKeyDown(KeyCode.Space))
            ReleaseAttractedObjects();
    }

    protected override Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    protected override bool IsMoving()
    {
        return true;
    }
}
