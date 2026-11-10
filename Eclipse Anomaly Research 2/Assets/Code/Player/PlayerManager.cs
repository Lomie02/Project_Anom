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

    bool mLerpingRotation = false;
   public Transform mForwardDirection;
   public Transform mLeftDirection;

    Transform mTargetDirection;

    public float mLookSpeed = 0.5f;
    void Start()
    {
        mPlayerCamera = GetComponentInChildren<Camera>();
    }

    public void Update()
    {
        if (mLerpingRotation)
        {
            if(mDirections == PlayerDirection.Forward)
            {
                mTargetDirection = mForwardDirection;
            }
            else
            {
                mTargetDirection = mLeftDirection;
            }

            mPlayerCamera.transform.rotation = Quaternion.Lerp(mPlayerCamera.transform.rotation,mTargetDirection.rotation, mLookSpeed * Time.deltaTime);

            if(Quaternion.Angle(mPlayerCamera.transform.rotation, mTargetDirection.rotation) < 0.1f)
            {
                mLerpingRotation = false;
            }
        }
    }

    public void LookLeft()
    {
        mLerpingRotation = true;
        mDirections = PlayerDirection.Left;
        Debug.Log("Look Left!");
    }

    public void LookRight()
    {
        mLerpingRotation = true;
        mDirections = PlayerDirection.Forward;
        Debug.Log("Look Right!");
    }
}
