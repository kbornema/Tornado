using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour 
{
    [SerializeField]
    private Tornado _tornado;
    [SerializeField]
    private float _changeConfPercent = 0.1f;



    private void FixedUpdate()
    {
        Vector2 moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        _tornado.AddForce(new Vector3(moveDir.x, 0.0f, moveDir.y));

        if(Input.GetKey(KeyCode.Q))
        {
            _tornado.ChangeConfPercent(-_changeConfPercent * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.E))
        {
            _tornado.ChangeConfPercent(_changeConfPercent * Time.deltaTime);
        }
    }

}
