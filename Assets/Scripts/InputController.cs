using UnityEngine;
using UnityEngine.InputSystem;

namespace MetalheadGame
{
    public class InputController : MonoBehaviour, MetalheadInput.IMainActions
    {
        [SerializeField] private LocomotionController _locomotion;
        [SerializeField] private AnimationController _animation;

        private MetalheadInput _input;

        private void Awake()
        {
            _input = new MetalheadInput();
            _input.Main.AddCallbacks(this);
        }

        private void OnEnable() => _input.Main.Enable();
        private void OnDisable() => _input.Main.Disable();
        private void OnDestroy() => _input.Dispose();

        private void Update()
        {
            if (!_locomotion)
            {
                return;
            }
            
            var isRunning = _input != null && _input.Main.Run.IsPressed();
            ApplyRunning(isRunning);

            var isGrounded = _locomotion.IsGrounded || _locomotion.VerticalVelocity > 0f;
            _animation?.SetGrounded(isGrounded);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var moveInput = context.ReadValue<Vector2>();
            _locomotion?.SetMoveInput(moveInput);
            _animation?.SetMoveInput(moveInput);
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            ApplyRunning(context.ReadValueAsButton());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!_locomotion || !context.performed || !_locomotion.IsGrounded)
            {
                return;
            }

            _animation?.PlayJump();
        }

        private void ApplyRunning(bool isRunning)
        {
            _locomotion?.SetRunning(isRunning);
            _animation?.SetRunning(isRunning);
        }
    }
}
