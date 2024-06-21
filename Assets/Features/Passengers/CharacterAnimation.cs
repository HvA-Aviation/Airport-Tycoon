using System;
using System.Collections;
using Implementation.Pathfinding.Scripts;
using UnityEngine;

namespace Features.Passengers
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private AnimationCurve _rotationCurve;
        [SerializeField] private float _speed;
        [SerializeField] private float _power;
        [SerializeField] private float _rotationPower;
        [SerializeField] private NPCController _npcController;

        private bool _moving;

        private void Update()
        {
            if (_npcController.Direction != Vector3.zero && !_moving)
            {
                StartMoving();
            }
            else if (_npcController.Direction == Vector3.zero && _moving)
            {
                StopMoving();
            }
        }

        private IEnumerator Bouncing()
        {
            float elapsedTime = 0;
            while (_moving || elapsedTime < 1)
            {
                if (elapsedTime == 1)
                    elapsedTime = 0;
                
                if (_npcController.Direction.x != 0)
                    transform.localScale = new Vector3(_npcController.Direction.x < 0 ? -1 : 1, 1, 1);
                
                elapsedTime = Mathf.Clamp(elapsedTime + Time.deltaTime * _speed, 0, 1);

                Vector3 localPos = transform.localPosition;
                localPos.y = _curve.Evaluate(elapsedTime * 2) * _power;
                transform.localPosition = localPos;


                Vector3 rotation = transform.localEulerAngles;
                rotation.z = _rotationCurve.Evaluate(elapsedTime) * _rotationPower;
                transform.localEulerAngles = rotation;

                yield return null;
            }

            transform.localEulerAngles = Vector3.zero;
        }

        public void StopMoving()
        {
            _moving = false;
        }

        public void StartMoving()
        {
            _moving = true;
            StopAllCoroutines();
            StartCoroutine(Bouncing());
        }
    }
}