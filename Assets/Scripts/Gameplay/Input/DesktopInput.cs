using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DesktopInput : IInputDevice
{   
    [SerializeField]
    private float _changeConfPercent = 2.0f;
    [SerializeField]
    private float _speed = 10.0f;
    
    public void Update(Tornado tornado, float deltaTime)
    {
        Vector2 moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        tornado.AddForce(new Vector3(moveDir.x, 0.0f, moveDir.y) * deltaTime * _speed, ForceMode.Impulse);

        if (Input.GetKey(KeyCode.Q))
        {
            tornado.ChangeConfPercent(-_changeConfPercent * deltaTime);
        }

        else if (Input.GetKey(KeyCode.E))
        {
            tornado.ChangeConfPercent(_changeConfPercent * deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            tornado.ReleaseAllAttractedObjects();
    }
}
