using System.Collections;
using UnityEngine;

namespace SimpleDialogue.DemoScripts
{
    public class RPGPlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed = 4.0f;
        [Space]
        [SerializeField] private Sprite _leftStanding;
        [SerializeField] private Sprite _rightStanding;
        [SerializeField] private Sprite _upStanding;
        [SerializeField] private Sprite _downStanding;
        [Space]
        [SerializeField] private Sprite[] _leftWalking;
        [SerializeField] private Sprite[] _rightWalking;
        [SerializeField] private Sprite[] _upWalking;
        [SerializeField] private Sprite[] _downWalking;
        [Space]
        [SerializeField] private float _spriteInterval = 0.2f;
        [SerializeField] private float _interactDistance = 0.8f;

        private Rigidbody2D m_rigidbody;
        private SpriteRenderer m_spriteRenderer;

        private Vector2 _input;
        private Vector2 _inputNormalized;
        private Coroutine _walkingCoroutine;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!DialogueSystem.IsPlaying)
            {
                _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }
            else
            {
                _input = Vector2.zero;
            }

            if (_input != Vector2.zero)
            {
                _inputNormalized = _input.normalized;
                if (_walkingCoroutine == null)
                {
                    _walkingCoroutine = StartCoroutine(AnimateWalking());
                }
            }
            else
            {
                if (_walkingCoroutine != null)
                {
                    StopCoroutine(_walkingCoroutine);
                    _walkingCoroutine = null;
                    SetStandingSprite();
                }
            }

            if (Input.GetKeyDown(DialogueSystem.ActiveSubmitKey) || Input.GetKeyDown(DialogueSystem.AltSubmitKey))
            {
                if (!DialogueSystem.IsPlaying)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, _inputNormalized, _interactDistance);
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.TryGetComponent<DialoguePlayer>(out DialoguePlayer player))
                        {
                            player.TryPlay();
                        }
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            m_rigidbody.velocity = _input.normalized * _speed;
        }

        private IEnumerator AnimateWalking()
        {
            Sprite[] currentWalkingSprites = null;

            while (true)
            {
                if (_input.x < 0)
                {
                    currentWalkingSprites = _leftWalking;
                }
                else if (_input.x > 0)
                {
                    currentWalkingSprites = _rightWalking;
                }
                else if (_input.y > 0)
                {
                    currentWalkingSprites = _upWalking;
                }
                else if (_input.y < 0)
                {
                    currentWalkingSprites = _downWalking;
                }

                if (currentWalkingSprites != null && currentWalkingSprites.Length > 0)
                {
                    foreach (Sprite sprite in currentWalkingSprites)
                    {
                        m_spriteRenderer.sprite = sprite;
                        yield return new WaitForSeconds(_spriteInterval);
                    }
                }
            }
        }

        private void SetStandingSprite()
        {
            if (_inputNormalized.x < 0)
            {
                m_spriteRenderer.sprite = _leftStanding;
            }
            else if (_inputNormalized.x > 0)
            {
                m_spriteRenderer.sprite = _rightStanding;
            }
            else if (_inputNormalized.y > 0)
            {
                m_spriteRenderer.sprite = _upStanding;
            }
            else if (_inputNormalized.y < 0)
            {
                m_spriteRenderer.sprite = _downStanding;
            }
        }
    }
}