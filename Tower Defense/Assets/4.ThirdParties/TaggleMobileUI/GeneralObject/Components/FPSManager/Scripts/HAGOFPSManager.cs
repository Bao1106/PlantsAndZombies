using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using System;
using TaggleTemplate.Core;

public class HAGOFPSManager : MonoBehaviour
{
    #region API
    private static HAGOFPSManager m_api;
    public static HAGOFPSManager Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = Instantiate(Resources.Load<GameObject>(HAGOConstant.PREFAB_FPS_MANAGER)).GetComponent<HAGOFPSManager>();
            }
            return m_api;
        }
    }
    #endregion

    private ScrollRect m_lastScrollRect;
    private bool m_isChangeValueScrolling;
    private ScrollRect m_cacheScrollRect;

    private int m_targetFrame = 60;
    private int m_targetFrameSaveBattery = 30;

    private int m_intRenderFrameInternal = 1;

    private const float CONST_MINIMUM_VELOCITY = 0.2f;

    /// <summary>
    /// Init FPSManager
    /// </summary>
    /// <param name="targetFrame"></param>
    public void Init(int targetFrame, int targetFrameToSaveBattery)
    {
        Debug.Log("FPSManager Init targetFrame: " + targetFrame + " - targetFrameToSaveBattery: " + targetFrameToSaveBattery);

        QualitySettings.vSyncCount = 0;

        m_targetFrame = targetFrame;

        if (m_targetFrame < 60)
        {
            Debug.LogError("TargetFrameRate Input < 60 => FPSManager not active");
            DestroyObj();
            return;
        }

        m_targetFrameSaveBattery = targetFrameToSaveBattery;

        if (m_targetFrame < m_targetFrameSaveBattery)
        {
            Debug.LogError("targetFrameToSaveBattery greater m_targetFrame => FPSManager not active \nTargetFrameToSaveBattery should be lower targetFrame");
            DestroyObj();
            return;
        }

        if (m_targetFrame> m_targetFrameSaveBattery)
        {
            m_intRenderFrameInternal = m_targetFrame / m_targetFrameSaveBattery;
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0))
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 0)
            {
                foreach (var go in raycastResults)
                {
                    m_cacheScrollRect = go.gameObject.transform.parent?.GetComponent<ScrollRect>();
                    if (m_cacheScrollRect)
                    {
                        m_lastScrollRect = m_cacheScrollRect;
                        m_lastScrollRect.onValueChanged.RemoveListener(OnCheckEndScrolling);
                        m_lastScrollRect.onValueChanged.AddListener(OnCheckEndScrolling);
                        m_isChangeValueScrolling = true;
                        break;
                    }
                }
            }
        }

        if (Input.GetMouseButton(0) || (Input.touchCount > 0))
        {
            // If the mouse button is down render at maximum FPS (every frame).
            MaximumRenderFrame();
        }

        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0) && Input.touches[0].phase == TouchPhase.Ended)
        {
            if (!m_lastScrollRect || m_lastScrollRect && m_lastScrollRect.velocity.magnitude <= CONST_MINIMUM_VELOCITY)
            {
                MinimumRenderFrame();
            }
        }

        if (m_isChangeValueScrolling)
        {
            if (m_lastScrollRect.velocity.magnitude <= CONST_MINIMUM_VELOCITY)
            {
                MinimumRenderFrame();
                m_isChangeValueScrolling = false;
            }
        }
        else
        {
            MinimumRenderFrame();
        }
    }

    private void MinimumRenderFrame()
    {
        OnDemandRendering.renderFrameInterval = m_intRenderFrameInternal;
    }

    private void OnCheckEndScrolling(Vector2 vector)
    {
        if (m_lastScrollRect)
        {
            m_isChangeValueScrolling = true;
        }
    }

    private void MaximumRenderFrame()
    {
        OnDemandRendering.renderFrameInterval = 1;
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        MaximumRenderFrame();
    }

    private void OnDestroy()
    {
        MaximumRenderFrame();
    }
}