using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 _velocity;
    public Vector3 Velocity { get { return _velocity; } set { _velocity = value; } }

    [SerializeField] private float _friction;
    [SerializeField, Range(0,100)] private float _velocityReductionChocFactor = 33; 
    [SerializeField, Range(0, 50)] private float _thresoldSpeedToMove;

    private BallData _ballData;
    public int ID { get; set; }

    public Ball(int id) { ID = id; }

    private void Awake()
    {
        _velocity = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += _velocity * Time.deltaTime;
        var newMagnitude = _velocity.magnitude - _friction * Time.deltaTime;
        _velocity = _velocity.normalized * newMagnitude;

        if (_velocity.magnitude < _thresoldSpeedToMove)
        {
            _velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")){
            Bounce(collision.GetContact(0).normal);
        } 
        else if (collision.gameObject.CompareTag("Ball"))
        {
            var otherBall = collision.gameObject.GetComponent<Ball>();

            //check si les balls sont de même niveau, et si oui traiter cas particulier 

            if(_velocity.magnitude > otherBall.Velocity.magnitude)
            {
                Bounce(collision.GetContact(0).normal);
            } else
            {
                _velocity = -otherBall.Velocity *  (1 - _velocityReductionChocFactor * 0.01f);
            }
        }
    }

    public void Setup(BallData ballData) { 
        _ballData = ballData;
        //change scale and color according to data
    }
    private void Bounce(Vector3 normal) {
        normal = new Vector3(normal.x, 0f, normal.z);
        var reflect = Vector3.Reflect(_velocity, normal);
        _velocity = new Vector3(reflect.x, _velocity.y, reflect.z);
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }
}
