using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Build;

public class ZoneAsset : MonoBehaviour
{
    public string mZoneName;
    public int mZoneID = -1;
    Camera mZoneCamera;
    List<AnomalyAsset> mAnomalyObjects = new List<AnomalyAsset>();

    AnomalyAsset mCurrentAnomaly;
    void Start()
    {
        mZoneCamera = GetComponentInChildren<Camera>();
        AnomalyAsset[] assets = GetComponentsInChildren<AnomalyAsset>();

        foreach (AnomalyAsset a in assets)
        {
            a.Despawn();
            mAnomalyObjects.Add(a);
        }

        SetAsMainCamera(false);
    }

    public string GetZoneName()
    {
        return mZoneName;
    }

    public Camera GetMainCamera()
    {
        return mZoneCamera;
    }

    public void SetAsMainCamera(bool _state)
    {
        mZoneCamera.gameObject.SetActive( _state );
    }

    public int GetId()
    {
        return mZoneID;
    }

    public void SetId(int _index)
    {
        mZoneID = _index;
    }

    public bool IsAnomalyActive()
    {
        for (int i = 0; i < mAnomalyObjects.Count; i++)
        {
            if (!mAnomalyObjects[i].IsActiveAnom()) continue;
            return true;
        }

        return false;
    }

    public void SpawnRandomAnomaly()
    {
        if (mAnomalyObjects.Count < 1) return;

        mCurrentAnomaly = mAnomalyObjects[Random.Range(0, mAnomalyObjects.Count)];

        if (mCurrentAnomaly)
            mCurrentAnomaly.Spawn();
    }

    public void ClearAnomalies()
    {
        if (mCurrentAnomaly)
            mCurrentAnomaly.Despawn();
    }

    public AnomalyType GetAnomType()
    {
        if (!mCurrentAnomaly) return AnomalyType.Environment;

        return mCurrentAnomaly.GetAnomalyType();
    }
}
