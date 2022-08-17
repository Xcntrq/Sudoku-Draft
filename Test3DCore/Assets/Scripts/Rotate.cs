using UnityEngine;

namespace nsRotate
{
    public class Rotate : MonoBehaviour
    {
        public float _cd;
        public float _angle;
        public int _noTimes;

        private float _timeLeft;
        private int _movesLeft;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _timeLeft = _cd;
            _movesLeft = _noTimes;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
            }
            else
            {
                if (_movesLeft > 0)
                {
                    _timeLeft = _cd;
                    _movesLeft--;
                    //EditorApplication.isPaused = true;

                    float rnd = Random.Range(-_angle,_angle);

                    Quaternion quaternion = Quaternion.AngleAxis(rnd, transform.up);

                    _rigidbody.velocity = quaternion * _rigidbody.velocity;
                    _rigidbody.transform.forward = quaternion * _rigidbody.transform.forward;

                }
            }
        }
    }
}
