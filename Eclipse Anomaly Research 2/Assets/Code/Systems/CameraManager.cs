using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CameraManager : MonoBehaviour
{
    [Header("General")]
    public RenderTexture mCameraFeed;
    List<ZoneAsset> mZoneObjects = new List<ZoneAsset>();
    [Header("Interface")]
    public GameObject mLoadingFeedScreen;
    public TextMeshProUGUI mCameraLocationText;
    public Slider mLoadBar;

    public Button[] mInteractableButtons;
    //================

    int mGeneratedId = -1;
    int mActiveZone;
    int mLookingAtZone = -1;

    float mLoadingWait = 2f;

    bool mIsLoadingNewFeed = false;
    float mLoadBarRate = 2f;

    void Start()
    {
        // Find & add all zones
        ZoneAsset[] zones = GetComponentsInChildren<ZoneAsset>();

        foreach (ZoneAsset z in zones)
        {
            mZoneObjects.Add(z);
            z.SetId(GenerateId()); // Create an id
        }

        StartCoroutine(SetUp());

    }

    void Update()
    {
        if (mIsLoadingNewFeed)
        {
            mLoadBar.value = Mathf.Lerp(mLoadBar.value, mLoadBar.maxValue, mLoadBarRate * Time.deltaTime);
        }


    }
    IEnumerator SetUp()
    {
        yield return new WaitForSeconds(1f);

        SetUpFirstCamera();
    }

    int GenerateId()
    {
        mGeneratedId++;
        return mGeneratedId;
    }
    void SetUpFirstCamera()
    {
        mCameraFeed.Release();
        mCameraFeed.Create();

        mLookingAtZone = mZoneObjects[mActiveZone].GetId();

        mCameraLocationText.text = mZoneObjects[mActiveZone].GetZoneName();

        mZoneObjects[mActiveZone].SetAsMainCamera(true);
        mZoneObjects[mActiveZone].GetMainCamera().targetTexture = mCameraFeed;
    }

    public void NextCamera()
    {
        if (mZoneObjects.Count < 2) return;

        mIsLoadingNewFeed = true;
        mLoadBar.value = 0;
        ToggleButtons(false);
        StartCoroutine(NextCameraWait());
    }

    public void PreviousCamera()
    {
        if (mZoneObjects.Count < 2) return;

        mIsLoadingNewFeed = true;
        mLoadBar.value = 0;

        ToggleButtons(false);
        StartCoroutine(PrevCameraWait());

    }

    IEnumerator NextCameraWait()
    {
        if (mLoadingFeedScreen)
            mLoadingFeedScreen.SetActive(true);

        mCameraLocationText.text = "LOADING NEW FEED.";

        yield return new WaitForSeconds(mLoadingWait);

        mZoneObjects[mActiveZone].SetAsMainCamera(false); // unset last zone as main

        mActiveZone++;

        if (mActiveZone >= mZoneObjects.Count)
            mActiveZone = 0;

        // Set Up camera
        mCameraFeed.Release();
        mCameraFeed.Create();

        mCameraLocationText.text = mZoneObjects[mActiveZone].GetZoneName();
        mLookingAtZone = mZoneObjects[mActiveZone].GetId();

        mZoneObjects[mActiveZone].SetAsMainCamera(true);
        mZoneObjects[mActiveZone].GetMainCamera().targetTexture = mCameraFeed;

        if (mLoadingFeedScreen)
            mLoadingFeedScreen.SetActive(false);
        mIsLoadingNewFeed = false;
        ToggleButtons(true);
    }

    IEnumerator PrevCameraWait()
    {
        if (mLoadingFeedScreen)
            mLoadingFeedScreen.SetActive(true);

        mCameraLocationText.text = "LOADING NEW FEED.";
        yield return new WaitForSeconds(mLoadingWait);

        mZoneObjects[mActiveZone].SetAsMainCamera(false); // unset last zone as main

        mActiveZone--;

        if (mActiveZone < 0)
            mActiveZone = mZoneObjects.Count - 1;

        // Set Up camera
        mCameraFeed.Release();
        mCameraFeed.Create();

        mCameraLocationText.text = mZoneObjects[mActiveZone].GetZoneName();
        mLookingAtZone = mZoneObjects[mActiveZone].GetId();

        mZoneObjects[mActiveZone].SetAsMainCamera(true);
        mZoneObjects[mActiveZone].GetMainCamera().targetTexture = mCameraFeed;

        if (mLoadingFeedScreen)
            mLoadingFeedScreen.SetActive(false);
        mIsLoadingNewFeed = false;
        ToggleButtons(true);
    }

    void ToggleButtons(bool _state)
    {
        foreach(Button b in mInteractableButtons)
        {
            b.interactable = _state;
        }
    }

    public int GetCurrentZoneId()
    {
        return mLookingAtZone;
    }
}
