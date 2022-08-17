using UnityEngine;

namespace nsMove
{
    public class Damn { }


    public class Move : MonoBehaviour
    {
        public float _duration;
        public float _force;
        public float _maxSpeed;

        private float _timeLeft;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _timeLeft = _duration;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
                if (_rigidbody.velocity.magnitude < _maxSpeed)
                {
                    _rigidbody.AddForce(transform.forward * _force, ForceMode.VelocityChange);
                   // Debug.Log("forced");
                }
            }
            else
            {

            }
        }
    }
}
