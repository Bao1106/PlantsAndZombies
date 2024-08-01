using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HAGOContainerEmojiItemView : MonoBehaviour
{
    public bool InitByUser = false;
    private ScrollRect m_scrollRect;
    private ContentSizeFitter m_contentSizeFitter;
    private VerticalLayoutGroup m_verticalLayoutGroup;
    private HorizontalLayoutGroup m_horizontalLayoutGroup;
    private GridLayoutGroup m_gridLayoutGroup;
    private bool m_isVertical = false;
    private bool m_isHorizontal = false;
    private float m_widthRollrect = 0;
    private float m_heighRollrect = 0;
    private List<RectTransform> m_items = new List<RectTransform>();

    public void Init(ScrollRect scrollRect)
    {
        if (InitByUser)
            return;

        //update view
        m_scrollRect = scrollRect;

        HAGOEmojiPickerControl.Api.OnCompleteLoadDataEvent += OnCompleteLoadDataHandler;
    }

    private void OnCompleteLoadDataHandler()
    {
        if (m_scrollRect != null)
        {
            m_isHorizontal = m_scrollRect.horizontal;
            m_isVertical = m_scrollRect.vertical;

            for (int i = 0; i < transform.childCount; i++)
            {
                m_items.Add(transform.GetChild(i).GetComponent<RectTransform>());
            }
            if (m_scrollRect.content.GetComponent<VerticalLayoutGroup>() != null)
            {
                m_verticalLayoutGroup = m_scrollRect.content.GetComponent<VerticalLayoutGroup>();
            }
            if (m_scrollRect.content.GetComponent<HorizontalLayoutGroup>() != null)
            {
                m_horizontalLayoutGroup = m_scrollRect.content.GetComponent<HorizontalLayoutGroup>();
            }
            if (GetComponent<GridLayoutGroup>() != null)
            {
                m_gridLayoutGroup = GetComponent<GridLayoutGroup>();
            }
            if (GetComponent<ContentSizeFitter>() != null)
            {
                m_contentSizeFitter = GetComponent<ContentSizeFitter>();
            }
            DisableGridComponents();
            OnScroll(Vector2.right);
        }
        else
        {
            Debug.LogError("HAGOContainerEmojiItemView => No ScrollRect component found");
        }
    }

    private void OnDestroy()
    {
        Destroy();
    }

    public void Destroy()
    {
        HAGOEmojiPickerControl.Api.OnCompleteLoadDataEvent -= OnCompleteLoadDataHandler;
    }

    void DisableGridComponents()
    {
        if (m_isVertical)
            m_heighRollrect = m_scrollRect.GetComponent<RectTransform>().rect.height;

        if (m_isHorizontal)
            m_widthRollrect = m_scrollRect.GetComponent<RectTransform>().rect.width;

        if (m_verticalLayoutGroup)
        {
            m_verticalLayoutGroup.enabled = false;
        }
        if (m_horizontalLayoutGroup)
        {
            m_horizontalLayoutGroup.enabled = false;
        }
        if (m_contentSizeFitter)
        {
            m_contentSizeFitter.enabled = false;
        }
        if (m_gridLayoutGroup)
        {
            m_gridLayoutGroup.enabled = false;
        }
    }

    public void OnScroll(Vector2 pos)
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_isVertical && m_isHorizontal)
            {
                if (m_scrollRect.transform.InverseTransformPoint(m_items[i].position).y - m_items[i].sizeDelta.y + m_items[i].sizeDelta.y * m_items[i].pivot.y  < -m_heighRollrect / 2 || m_scrollRect.transform.InverseTransformPoint(m_items[i].position).y - m_items[i].sizeDelta.y * m_items[i].pivot.y  > m_heighRollrect / 2
                || m_scrollRect.transform.InverseTransformPoint(m_items[i].position).x - m_items[i].sizeDelta.x + m_items[i].sizeDelta.x * m_items[i].pivot.x  < -m_widthRollrect / 2 || m_scrollRect.transform.InverseTransformPoint(m_items[i].position).x - m_items[i].sizeDelta.x * m_items[i].pivot.x  > m_widthRollrect / 2)
                {
                    m_items[i].gameObject.SetActive(false);
                }
                else
                {
                    m_items[i].gameObject.SetActive(true);
                }
            }
            else
            {
                if (m_isVertical)
                {
                    if (m_scrollRect.transform.InverseTransformPoint(m_items[i].position).y + m_items[i].sizeDelta.y - m_items[i].sizeDelta.y * m_items[i].pivot.y  < -m_heighRollrect / 2 || m_scrollRect.transform.InverseTransformPoint(m_items[i].position).y - m_items[i].sizeDelta.y * m_items[i].pivot.y  > m_heighRollrect / 2)
                    {
                        m_items[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        m_items[i].gameObject.SetActive(true);

                    }
                }

                if (m_isHorizontal)
                {
                    if (m_scrollRect.transform.InverseTransformPoint(m_items[i].position).x + m_items[i].sizeDelta.x - m_items[i].sizeDelta.x * m_items[i].pivot.x < -(m_widthRollrect / 2) || m_scrollRect.transform.InverseTransformPoint(m_items[i].position).x - m_items[i].sizeDelta.x * m_items[i].pivot.x > (m_widthRollrect / 2))
                    {
                        m_items[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        m_items[i].gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
