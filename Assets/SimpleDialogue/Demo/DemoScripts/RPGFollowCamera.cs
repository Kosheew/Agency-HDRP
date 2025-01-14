using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleDialogue.DemoScripts
{
    public class RPGFollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform _leftLimitPoint;
        [SerializeField] private Transform _rightLimitPoint;
        [SerializeField] private Transform _upLimitPoint;
        [SerializeField] private Transform _downLimitPoint;
        [Space]
        [SerializeField] private float _smooth = 1.0f;

        private RPGPlayerController _controller;
        private Camera _camera;

        private void Awake()
        {
            _controller = FindObjectOfType(typeof(RPGPlayerController), true) as RPGPlayerController;
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            Vector2 direction = Vector2.zero;
            if (!float.IsInfinity(_smooth))
            {
                direction = Vector2.Lerp(transform.position, _controller.transform.position, _smooth * Time.smoothDeltaTime);
            }
            else
            {
                direction = _controller.transform.position;
            }
            transform.position = direction;
            AlignEdges();
        }

        private void AlignEdges()
        {
            float cameraHeightSide = _camera.orthographicSize;
            float cameraWidthSide = ((cameraHeightSide * 2) * Screen.width / Screen.height) / 2;
            if (transform.position.x - cameraWidthSide < _leftLimitPoint.position.x)
            {
                transform.position = new Vector2(_leftLimitPoint.position.x + cameraWidthSide, transform.position.y);
            }
            if (transform.position.x + cameraWidthSide > _rightLimitPoint.position.x)
            {
                transform.position = new Vector2(_rightLimitPoint.position.x - cameraWidthSide, transform.position.y);
            }
            if (transform.position.y + cameraHeightSide > _upLimitPoint.position.y)
            {
                transform.position = new Vector2(transform.position.x, _upLimitPoint.position.y - cameraHeightSide);
            }
            if (transform.position.y - cameraHeightSide < _downLimitPoint.position.y)
            {
                transform.position = new Vector2(transform.position.x, _downLimitPoint.position.y + cameraHeightSide);
            }
        }
    }
}