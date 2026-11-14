using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    public CameraManager CameraManager;
    List<ZoneAsset> mZoneObjects = new List<ZoneAsset>();
    bool mAnomManagerActive = false;

    int mPreviousZone = -1;

    float mSpawnTime = 12;
    float mStartWait = 12;
    bool mFirstTimeSpawn = true;

    void Start()
    {
        // Find & add all zones
        ZoneAsset[] zones = GetComponentsInChildren<ZoneAsset>();

        foreach (ZoneAsset z in zones)
        {
            mZoneObjects.Add(z);
        }

        StartCoroutine(SpawnAnomaly());
    }

    IEnumerator SpawnAnomaly()
    {
        if (mFirstTimeSpawn)
        {
            mFirstTimeSpawn = false;
            yield return new WaitForSeconds(mStartWait);
        }

        List<ZoneAsset> zonesReady = new List<ZoneAsset>();

        // Prepare a list of zones that have no active anomaly.
        for (int i = 0; i < mZoneObjects.Count; i++)
        {
            if (mZoneObjects[i].IsAnomalyActive()) continue;

            zonesReady.Add(mZoneObjects[i]);
        }

        // If the ready zones list is empty, wait for next spawn time.
        if (zonesReady.Count < 1)
            yield return new WaitForSeconds(mSpawnTime);

        ZoneAsset targetZone = SelectRandomZone(zonesReady);

        targetZone.SpawnRandomAnomaly();

        yield return new WaitForSeconds(mSpawnTime);
    }

    ZoneAsset SelectRandomZone(List<ZoneAsset> _list)
    {
        ZoneAsset zone = null;
        bool HasSelectedZone = false;

        while (!HasSelectedZone)
        {
            if (_list.Count > 1)
            {
                zone = _list[Random.Range(0, _list.Count)];

                if (zone.GetId() != mPreviousZone)
                {
                    mPreviousZone = zone.GetId();
                    HasSelectedZone = true;
                }
            }
            else
            {
                zone = _list[0];
                mPreviousZone = zone.GetId();
                HasSelectedZone = true;
            }
        }

        return zone;
    }

    public bool ReportAnomaly(AnomalyType _type, int _zoneId)
    {



        return false;
    }

}
