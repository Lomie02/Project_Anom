using UnityEngine;

enum PlayerDirection
{
    Forward = 0,
    Left,
    right,
}

public class PlayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    PlayerDirection mDirections;
    Camera mPlayerCamera;
    Transform mTargetDirection;

    bool mLerpingRotation = false;
    bool mLerpingZoom = false;
    float mDefaultFov;
    bool mZoomedIn = false;

    float mTargetFieldOfView;
    [Header("General")]
    public Transform mForwardDirection;
    public Transform mLeftDirection;
    public float mLookSpeed = 0.5f;

    [Header("FOV")]
    public float mCameraViewingFov = 35.0f;

    [Header("User Interface")]
    public GameObject mViewZones;

    void Start()
    {
        mPlayerCamera = GetComponentInChildren<Camera>();

        mDefaultFov = mPlayerCamera.fieldOfView;
    }

    public void Update()
    {
        if (mLerpingRotation) // Lerp the players rotations
        {
            if (mDirections == PlayerDirection.Forward)
            {
                mTargetDirection = mForwardDirection;
            }
            else
            {
                mTargetDirection = mLeftDirection;
            }

            mPlayerCamera.transform.rotation = Quaternion.Lerp(mPlayerCamera.transform.rotation, mTargetDirection.rotation, mLookSpeed * Time.deltaTime);

            if (Quaternion.Angle(mPlayerCamera.transform.rotation, mTargetDirection.rotation) < 0.1f)
            {
                mLerpingRotation = false;
            }
        }

        // lerp Player zoom
        if (mLerpingZoom)
        {
            if (mZoomedIn)
                mTargetFieldOfView = mCameraViewingFov;
            else
                mTargetFieldOfView = mDefaultFov;

            mPlayerCamera.fieldOfView = Mathf.Lerp(mPlayerCamera.fieldOfView, mTargetFieldOfView, 10.0f * Time.deltaTime);

            if (Mathf.Abs(mPlayerCamera.fieldOfView - mTargetFieldOfView) < 0.01f)
            {
                mPlayerCamera.fieldOfView = mTargetFieldOfView;
                mLerpingZoom = false;
            }
        }
    }

    public void ToggleZoom()
    {
        mZoomedIn = !mZoomedIn;

        if (mViewZones)
            mViewZones.SetActive(!mZoomedIn);

        mLerpingZoom = true;
    }

    public void LookLeft()
    {
        mLerpingRotation = true;
        mDirections = PlayerDirection.Left;
    }

    public void LookRight()
    {
        mLerpingRotation = true;
        mDirections = PlayerDirection.Forward;
    }
}
