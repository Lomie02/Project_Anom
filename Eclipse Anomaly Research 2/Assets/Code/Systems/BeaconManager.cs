using UnityEngine;

public class BeaconManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("General")]
    public bool mBeaconEnabled = true;
    [Header("Beacon")]
    public Renderer mBeaconShader;
    public float mBeaconCharge;
    public float mBeaconMaxCharge;
    [Header("Light")]
    public Light mBeaconLight;
    public float mBeaconLightMax = 7;
    public float mBeaconLightCharge = 0.01f;



    float mTimer;
    float mChargeRate;

    bool mIsBeaconReady = false;
    void Start()
    {
        mTimer = 0;
        mBeaconShader.sharedMaterials[1].SetFloat("_Intensity", mTimer);
        mBeaconLight.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Dont update beacon unless enabled.
        if (!mBeaconEnabled) return;

        if (!mIsBeaconReady)
        {

            mTimer += mBeaconCharge * Time.deltaTime;
            if (mTimer >= mBeaconMaxCharge)
            {
                mIsBeaconReady = true;
            }

            if (mBeaconLight.intensity < mBeaconLightMax)
                mBeaconLight.intensity += mBeaconLightCharge * Time.deltaTime;

            mBeaconShader.sharedMaterials[1].SetFloat("_Intensity", mTimer);
        }
    }
    public void ReleaseCharge()
    {
        if (!mIsBeaconReady) return;

        mTimer = 0;
        mIsBeaconReady = false;
        mBeaconLight.intensity = 0;
        mBeaconShader.sharedMaterials[1].SetFloat("_Intensity", mTimer);
    }
}
