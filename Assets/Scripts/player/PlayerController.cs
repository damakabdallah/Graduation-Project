using Cinemachine.Utility;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    #region variables

        #region public variables

        public float playerSpeed = 1;

        #endregion

        #region private variables

        private Animator _playerAnimator;
        private Camera _playerCamera;
        private CharacterController _playerController;

        #endregion

        #region constent variables

        private const float PlayerRotationSensitivity = 5f;

        #endregion

    #endregion

    #region buildin methods

    void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _playerController = GetComponent<CharacterController>();
        playerSpeed *= Time.deltaTime;
    }

    void Update()
    {
        float moveOnXAxis = Input.GetAxis("Horizontal");
        float moveOnYAxis = Input.GetAxis("Vertical");
        Vector2 playerJoystick = new Vector2(moveOnXAxis, moveOnYAxis);
        MoveIn8Directions(playerJoystick);
        if(playerJoystick != Vector2.zero)
            LookForward();
    }

    #endregion

    #region custom methods

    private void LookForward()
    {
        Vector3 playerCameraEulerAngles = _playerCamera.transform.eulerAngles;
        Quaternion turnAngle = Quaternion.Euler(0,playerCameraEulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, turnAngle, Time.deltaTime * PlayerRotationSensitivity);
        
    }

    private void MoveIn8Directions(Vector2 position)
    {
        _playerAnimator.SetFloat("xAxis",position.x);
        _playerAnimator.SetFloat("yAxis",position.y);
        Vector3 distanceBetweenPlayerAndCamera = transform.position-_playerCamera.transform.position;
        distanceBetweenPlayerAndCamera.y = 0;
        distanceBetweenPlayerAndCamera.Abs();
        distanceBetweenPlayerAndCamera = distanceBetweenPlayerAndCamera.normalized;
        Vector3 playerSpeedDirection =
            new Vector3(playerSpeed * position.y * distanceBetweenPlayerAndCamera.x, 0,
                playerSpeed * position.y * distanceBetweenPlayerAndCamera.z);
        _playerController.Move(playerSpeedDirection);

    }

    #endregion
    
}
