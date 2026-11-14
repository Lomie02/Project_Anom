using UnityEngine;

public enum AnomalyType
{
    Environment = 0,
    ObjectMoved,
    MissingObject,
    ExtraObject,
    Duplicate,
    Humanoid,
    Ghost,
    Light,
    CameraMalfunction,
}
public class AnomalyAsset : MonoBehaviour
{

    public AnomalyType mAnomalyType;

    public GameObject mTargetAnom;
    public GameObject mPartnerAnom;

    bool mActiveAnomaly;

    public bool IsActiveAnom()
    {
        return mActiveAnomaly;
    }

    public void Spawn()
    {
        mActiveAnomaly = true;

        if (mTargetAnom)
            mTargetAnom.SetActive(mActiveAnomaly);
     
        if (mPartnerAnom)
            mPartnerAnom.SetActive(!mActiveAnomaly);
    }

    public void Despawn()
    {
        mActiveAnomaly = false;

        if (mTargetAnom)
            mTargetAnom.SetActive(mActiveAnomaly);

        if (mPartnerAnom)
            mPartnerAnom.SetActive(!mActiveAnomaly);
    }

    public AnomalyType GetAnomalyType()
    {
        return mAnomalyType;
    }

}
