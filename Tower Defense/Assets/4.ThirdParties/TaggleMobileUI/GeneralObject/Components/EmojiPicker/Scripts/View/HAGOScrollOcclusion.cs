
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HAGOScrollOcclusion : MonoBehaviour
{
    //if true user will need to call Init() method manually (in case the contend of the scrollview is generated from code or requires special initialization)
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
    private List<HAGOContainerEmojiItemView> m_listContaineritemView = new List<HAGOContainerEmojiItemView>();
    private float m_padding = 10f;

    //cache
    private int m_lowestIndexVisible;
    public void Init()
    {
        if (InitByUser)
            return;

        HAGOEmojiPickerControl.Api.OnCompleteLoadDataEvent += OnCompleteLoadDataHandler;
    }

    private void OnCompleteLoadDataHandler()
    {
        if (GetComponent<ScrollRect>() != null)
        {
            m_scrollRect = GetComponent<ScrollRect>();
            m_scrollRect.onValueChanged.AddListener(OnScroll);

            m_isHorizontal = m_scrollRect.horizontal;
            m_isVertical = m_scrollRect.vertical;

            for (int i = 0; i < m_scrollRect.content.childCount; i++)
            {
                m_items.Add(m_scrollRect.content.GetChild(i).GetComponent<RectTransform>());
                m_listContaineritemView.Add(m_scrollRect.content.GetChild(i).GetComponent<HAGOContainerEmojiItemView>());
            }
            if (m_scrollRect.content.GetComponent<VerticalLayoutGroup>() != null)
            {
                m_verticalLayoutGroup = m_scrollRect.content.GetComponent<VerticalLayoutGroup>();
            }
            if (m_scrollRect.content.GetComponent<HorizontalLayoutGroup>() != null)
            {
                m_horizontalLayoutGroup = m_scrollRect.content.GetComponent<HorizontalLayoutGroup>();
            }
            if (m_scrollRect.content.GetComponent<GridLayoutGroup>() != null)
            {
                m_gridLayoutGroup = m_scrollRect.content.GetComponent<GridLayoutGroup>();
            }
            if (m_scrollRect.content.GetComponent<ContentSizeFitter>() != null)
            {
                m_contentSizeFitter = m_scrollRect.content.GetComponent<ContentSizeFitter>();
            }
            DisableGridComponents();
            OnScroll(Vector2.right);
        }
        else
        {
            Debug.LogError("HAGOScrollOcclusionEmoji => No ScrollRect component found");
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
        m_lowestIndexVisible = m_items.Count-1;
        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_isVertical && m_isHorizontal)
            {
                if (m_scrollRect.transform.InverseTransformPoint(m_items[i].position).y - m_items[i].sizeDelta.y + m_items[i].sizeDelta.y * m_items[i].pivot.y - m_padding < -m_heighRollrect / 2 || m_scrollRect.transform.InverseTransformPoint(m_items[i].position).y - m_items[i].sizeDelta.y * m_items[i].pivot.y + m_padding > m_heighRollrect / 2
                || m_scrollRect.transform.InverseTransformPoint(m_items[i].position).x - m_items[i].sizeDelta.x + m_items[i].sizeDelta.x * m_items[i].pivot.x - m_padding < -m_widthRollrect / 2 || m_scrollRect.transform.InverseTransformPoint(m_items[i].position).x - m_items[i].sizeDelta.x * m_items[i].pivot.x + m_padding > m_widthRollrect / 2)
                {
                    m_items[i].gameObject.SetActive(false);
                }
                else
                {
                    m_items[i].gameObject.SetActive(true);
                    m_listContaineritemView[i].OnScroll(pos);
                    if (i<m_lowestIndexVisible)
                    {
                        m_lowestIndexVisible = i;
                    }
                }
            }
            else
            {
                if (m_isVertical)
                {
                    if (m_scrollRect.transform.InverseTransformPoint(m_items[i].position).y + m_items[i].sizeDelta.y - m_items[i].sizeDelta.y * m_items[i].pivot.y - m_padding < -m_heighRollrect / 2 || m_scrollRect.transform.InverseTransformPoint(m_items[i].position).y - m_items[i].sizeDelta.y * m_items[i].pivot.y + m_padding > m_heighRollrect / 2)
                    {
                        m_items[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        m_items[i].gameObject.SetActive(true);
                        m_listContaineritemView[i].OnScroll(pos);
                        if (i < m_lowestIndexVisible)
                        {
                            m_lowestIndexVisible = i;
                        }
                    }
                }

                if (m_isHorizontal)
                {
                    if (m_scrollRect.transform.InverseTransformPoint(m_items[i].position).x + m_items[i].sizeDelta.x - m_items[i].sizeDelta.x * m_items[i].pivot.x - m_padding < -(m_widthRollrect / 2) || m_scrollRect.transform.InverseTransformPoint(m_items[i].position).x - m_items[i].sizeDelta.x * m_items[i].pivot.x + m_padding > (m_widthRollrect / 2))
                    {
                        m_items[i].gameObject.SetActive(false);
                        HAGOEmojiPickerControl.Api.VisibleCategoryItemView(i, false);
                    }
                    else
                    {
                        m_items[i].gameObject.SetActive(true);
                        HAGOEmojiPickerControl.Api.VisibleCategoryItemView(i, true);
                        m_listContaineritemView[i].OnScroll(pos);
                        if (i < m_lowestIndexVisible)
                        {
                            m_lowestIndexVisible = i;
                        }
                    }
                }
            }
        }

        HAGOEmojiPickerControl.Api.GetLowestIndexVisibleCategory(m_lowestIndexVisible);
    }
}