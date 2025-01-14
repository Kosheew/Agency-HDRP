using UnityEngine;

namespace SimpleDialogue.DemoScripts
{
    public class UPlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed = 7.0f;
        [Space]
        [SerializeField] private KeyCode[] _controls = new KeyCode[4] { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };
        [Space]
        [SerializeField] private Sprite[] _standingSprites = new Sprite[4];
        [SerializeField] private Sprite[] _walkingSprites = new Sprite[4];
        [Space]
        [SerializeField] private int _frameOffset = 1;

        private int _currentFrameCount;
        private bool _animationWalking;
        private bool _isWalking;

        private void OnValidate()
        {
            if (_controls.Length != 4)
            {
                _controls = new KeyCode[4];
            }
            if (_standingSprites.Length != 4)
            {
                _standingSprites = new Sprite[4];
            }
            if (_walkingSprites.Length != 4)
            {
                _walkingSprites = new Sprite[4];
            }
            if (_speed < 0)
            {
                _speed = 0;
            }
        }

        private BoxCollider2D m_boxCollider;
        private SpriteRenderer m_spriteRenderer;

        private void Awake()
        {
            m_boxCollider = GetComponent<BoxCollider2D>();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            Vector2 leftUpPos = ((Vector2)transform.position + m_boxCollider.offset) + new Vector2(-(m_boxCollider.size.x / 1.9f) * transform.localScale.x, (m_boxCollider.size.y / 1.9f)) * transform.localScale.y;
            Vector2 leftDownPos = ((Vector2)transform.position + m_boxCollider.offset) + new Vector2(-(m_boxCollider.size.x / 1.9f) * transform.localScale.x, -(m_boxCollider.size.y / 1.9f)) * transform.localScale.y;
            Vector2 rightUp = ((Vector2)transform.position + m_boxCollider.offset) + new Vector2((m_boxCollider.size.x / 1.9f) * transform.localScale.x, (m_boxCollider.size.y / 1.9f)) * transform.localScale.y;
            Vector2 rightDownPos = ((Vector2)transform.position + m_boxCollider.offset) + new Vector2((m_boxCollider.size.x / 1.9f) * transform.localScale.x, -(m_boxCollider.size.y / 1.9f)) * transform.localScale.y;

            if (Input.GetKey(_controls[0]) && !Physics2D.Linecast(leftUpPos, leftDownPos))
            {
                _isWalking = true;
                if (_currentFrameCount < _frameOffset)
                {
                    _currentFrameCount++;
                }
                else
                {
                    _animationWalking = !_animationWalking;
                    HorizontalTick(-_speed * Time.deltaTime);
                    _currentFrameCount = 0;
                }
            }
            if (Input.GetKey(_controls[1]) && !Physics2D.Linecast(rightUp, rightDownPos))
            {
                _isWalking = true;
                if (_currentFrameCount < _frameOffset)
                {
                    _currentFrameCount++;
                }
                else
                {
                    HorizontalTick(_speed * Time.deltaTime);
                    _currentFrameCount = 0;
                }
            }
            else _isWalking = false;

            if (Input.GetKey(_controls[2]) && !Physics2D.Linecast(leftUpPos, rightUp))
            {
                _isWalking = true;
                if (_currentFrameCount < _frameOffset)
                {
                    _currentFrameCount++;
                }
                else
                {
                    VerticalTick(_speed * Time.deltaTime);
                    _currentFrameCount = 0;
                }
            }
            else if ((Input.GetKey(_controls[3]) && !Physics2D.Linecast(leftDownPos, rightDownPos)))
            {
                _isWalking = true;
                if (_currentFrameCount < _frameOffset)
                {
                    _currentFrameCount++;
                }
                else
                {
                    VerticalTick(-_speed * Time.deltaTime);
                    _currentFrameCount = 0;
                }
            }
            else _isWalking = false;

            if (!_isWalking)
            {
                if (m_spriteRenderer.sprite == _walkingSprites[0])
                {
                    m_spriteRenderer.sprite = _standingSprites[0];
                }
                if (m_spriteRenderer.sprite == _walkingSprites[1])
                {
                    m_spriteRenderer.sprite = _standingSprites[1];
                }
                if (m_spriteRenderer.sprite == _walkingSprites[2])
                {
                    m_spriteRenderer.sprite = _standingSprites[2];
                }
                if (m_spriteRenderer.sprite == _walkingSprites[3])
                {
                    m_spriteRenderer.sprite = _standingSprites[3];
                }
            }
        }

        private void HorizontalTick(float vector)
        {
            transform.position = new Vector2(transform.position.x + vector, transform.position.y);

            if (vector > 0)
            {
                if (!_animationWalking)
                {
                    m_spriteRenderer.sprite = _walkingSprites[1];
                    _animationWalking = true;
                }
                else
                {
                    m_spriteRenderer.sprite = _standingSprites[1];
                    _animationWalking = false;
                }
            }
            else
            {
                if (!_animationWalking)
                {
                    m_spriteRenderer.sprite = _walkingSprites[0];
                    _animationWalking = true;
                }
                else
                {
                    m_spriteRenderer.sprite = _standingSprites[0];
                    _animationWalking = false;
                }
            }
        }

        private void VerticalTick(float vector)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + vector);

            if (vector > 0)
            {
                if (!_animationWalking)
                {
                    m_spriteRenderer.sprite = _walkingSprites[2];
                    _animationWalking = true;
                }
                else
                {
                    m_spriteRenderer.sprite = _standingSprites[2];
                    _animationWalking = false;
                }
            }
            else
            {
                if (!_animationWalking)
                {
                    m_spriteRenderer.sprite = _walkingSprites[3];
                    _animationWalking = true;
                }
                else
                {
                    m_spriteRenderer.sprite = _standingSprites[3];
                    _animationWalking = false;
                }
            }
        }
    }
}