using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HAGODatePickerCoreView : MonoBehaviour
{
	private Button m_btnTabDay;
	private Button m_btnTabWeek;
	private Button m_btnTabMonth;
	private Button m_btnTabYear;
	//
	private HAGODatePickerDayTabView m_dayView;
	private HAGODatePickerMonthTabView m_monthView;
	private HAGODatePickerYearTabView m_yearView;

	//param
	[HideInInspector]
	public HAGODateTimePickerType pickerType;
	private DateTime m_defaultSelectedDate = DateTime.Today;
	private HAGODatePickerPeriodType m_periodType;
	//
	private DateTime m_dateSelectedData; // return data for DatePicker single date result
	private Dictionary<DateTime,bool> m_dateMarkData; // list dates have circle mark icon
	//
	private List<DateTime> m_listDateSelectedData; // return data for DatePicker with multiple dates result
	private Action<List<DateTime>> m_onValueChangedMultipleDateSelectedEvent; // notify value changed in multiple date
	//
	public Action<DateTime> m_onResponseDateSelectedEvent; // response date selected
	private Action<List<DateTime>> m_onResponseMultipleDateSelectedEvent; // response list date selected
	public Action<HAGODatePickerPeriodType, DateTime, DateTime> m_onResponsePeriodDateSelectedEvent; // response period date selected

	//const
	private Color CONST_COLOR_TAB_ACTIVE = new Color(33f / 255f, 188f / 255f, 154f / 255f, 255f / 255f);
	private Color CONST_COLOR_TAB_NORMAL = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
	private Color CONST_COLOR_TAB_OUTLINE_ACTIVE = new Color(33f / 255f, 188f / 255f, 154f / 255f, 255f / 255f);
	private Color CONST_COLOR_TAB_OUTLINE_NORMAL = new Color(189 / 255f, 189 / 255f, 189 / 255f, 189 / 255f);
    private Color CONST_COLOR_TEXT_ACTIVE = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
   	private Color CONST_COLOR_TEXT_NORMAL = new Color(50f / 255f, 50f / 255f, 50f / 255f, 255f / 255f);

	void OnDestroy ()
	{
		Destroy();
	}

	public void Destroy ()
	{
		//unregister event
		m_monthView?.Destroy();
		m_yearView?.Destroy();
		//
		HAGODateTimePickerControl.Api.OnDateSelectedEvent -= OnDateSelectedHandler;
		HAGODateTimePickerControl.Api.OnDateDeselectedEvent -= OnDateDeselectedHandler;
		HAGODateTimePickerControl.Api.OnMonthSelectedEvent -= OnMonthSelectedHandler;
		HAGODateTimePickerControl.Api.OnYearSelectedEvent -= OnYearSelectedHandler;
	}

	/// <summary>
    /// Initialization single date picker
    /// </summary>
	///<param name = "onDateSelectedEvent">Return a list selected date after user selected day on calendar
	///or user call FinishSelectMultipleDate() if in single select mode</param>
	///<param name = "dateMarkData">Dates will have circle mark icon</param>
	public virtual void InitDatePicker(Action<DateTime> onDateSelectedEvent, DateTime selectedDate, Dictionary<DateTime,bool> dateMarkData = null)
	{
		//handle value
		pickerType = HAGODateTimePickerType.SingleDate;
		m_defaultSelectedDate = selectedDate;
		m_dateSelectedData = m_defaultSelectedDate;
		m_dateMarkData = dateMarkData;

		//register event
		m_onResponseDateSelectedEvent = onDateSelectedEvent;

		InitView();
	}

	/// <summary>
    /// Initialization multiple date picker
    /// </summary>
	///<param name = "onValueChanged">Return a list selected dates after user selected day on calendar</param>
	///<param name = "onMultiDateSelectedEvent">Return a list selected dates after user call FinishSelectMultipleDate() if in multiple select mode</param>
	///<param name = "dateMarkData">Dates will have circle mark icon</param>
	public virtual void InitMultiDatePicker (Action<List<DateTime>> onValueChangedEvent, Action<List<DateTime>> onMultipleDateSelectedEvent, Dictionary<DateTime,bool> dateMarkData = null)
	{
		//handle value
		pickerType = HAGODateTimePickerType.MultiDate;
		m_dateMarkData = dateMarkData;

		//register event
		m_onValueChangedMultipleDateSelectedEvent = onValueChangedEvent;
		m_onResponseMultipleDateSelectedEvent = onMultipleDateSelectedEvent;

		InitView();
	}

	/// <summary>
    /// Initialization period date picker
    /// </summary>
	///<param name = "onValueChanged">Return a list selected dates after user selected day on calendar</param>
	///<param name = "dateMarkData">Dates will have circle mark icon</param>
	public virtual void InitPeriodDatePicker (Action<HAGODatePickerPeriodType, DateTime, DateTime> onResponsePeriodDateSelectedEvent)
	{
		//handle value
		pickerType = HAGODateTimePickerType.PeriodDate;
		m_defaultSelectedDate = DateTime.Today;
		m_dateSelectedData = m_defaultSelectedDate;

		//register event
		m_onResponsePeriodDateSelectedEvent = onResponsePeriodDateSelectedEvent;

		InitView();
	}

	/// <summary>
    /// Initialization view
    /// </summary>
	public virtual void InitView()
	{
		//find reference
		m_btnTabDay = transform.Find("Tabs/BtnDay").GetComponent<Button>();
		m_btnTabWeek = transform.Find("Tabs/BtnWeek").GetComponent<Button>();
        m_btnTabMonth = transform.Find("Tabs/BtnMonth").GetComponent<Button>();
        m_btnTabYear = transform.Find("Tabs/BtnYear").GetComponent<Button>();
		//
		m_dayView = transform.Find("DayView").GetComponent<HAGODatePickerDayTabView>();
		m_monthView = transform.Find("MonthView").GetComponent<HAGODatePickerMonthTabView>();
		m_yearView = transform.Find("YearView").GetComponent<HAGODatePickerYearTabView>();

		//init other view
		m_listDateSelectedData = new List<DateTime>();
		//
		m_yearView.Init(m_defaultSelectedDate.Year);
		m_monthView.Init(m_defaultSelectedDate.Month, m_yearView.yearSelected);
		m_dayView.Init(pickerType == HAGODateTimePickerType.MultiDate, m_dateMarkData);
		//
		m_btnTabWeek.gameObject.SetActive(pickerType == HAGODateTimePickerType.PeriodDate);
		//
		ChangeTab(HAGODatePickerPeriodType.Day);

		//add listener
		m_btnTabDay.onClick.AddListener(TabDayOnClick);
		m_btnTabWeek.onClick.AddListener(TabWeekOnClick);
		m_btnTabMonth.onClick.AddListener(TabMonthOnClick);
		m_btnTabYear.onClick.AddListener(TabYearOnClick);

		//register event
		HAGODateTimePickerControl.Api.OnDateSelectedEvent += OnDateSelectedHandler;
		HAGODateTimePickerControl.Api.OnDateDeselectedEvent += OnDateDeselectedHandler;
		HAGODateTimePickerControl.Api.OnMonthSelectedEvent += OnMonthSelectedHandler;
		HAGODateTimePickerControl.Api.OnYearSelectedEvent += OnYearSelectedHandler;
	}

	/// <summary>
    /// Handle click on tab day
    /// </summary>
	private void TabDayOnClick ()
	{
		ChangeTab(HAGODatePickerPeriodType.Day);
	}
	
	/// <summary>
    /// Handle click on tab week
    /// </summary>
	private void TabWeekOnClick ()
	{
		ChangeTab(HAGODatePickerPeriodType.Week);
	}

	/// <summary>
    /// Handle click on tab month
    /// </summary>
	private void TabMonthOnClick ()
	{
		ChangeTab(HAGODatePickerPeriodType.Month);
	}

	/// <summary>
    /// Handle click on tab year
    /// </summary>
	private void TabYearOnClick ()
	{
		ChangeTab(HAGODatePickerPeriodType.Year);
	}

	/// <summary>
    /// Handler date selected
    /// </summary>
	private void OnDateSelectedHandler (DateTime date)
	{
		if(pickerType == HAGODateTimePickerType.PeriodDate)
		{
			m_onResponsePeriodDateSelectedEvent?.Invoke(m_periodType, HAGOUtils.GetPeriodStartDate(m_periodType, date), HAGOUtils.GetPeriodEndDate(m_periodType, date));
		}
		else if(pickerType == HAGODateTimePickerType.MultiDate)
		{
			m_listDateSelectedData.Add(date);
			m_onValueChangedMultipleDateSelectedEvent?.Invoke(m_listDateSelectedData);
		}
		else if(pickerType == HAGODateTimePickerType.SingleDate)
		{
			m_dateSelectedData = date;
			OnDateSelectedResponseHandler();
		}
	}
	
	/// <summary>
    /// Handler date deselected
    /// </summary>
	private void OnDateDeselectedHandler (DateTime date)
	{
		m_listDateSelectedData.Remove(date);
		m_onValueChangedMultipleDateSelectedEvent?.Invoke(m_listDateSelectedData);
	}

	/// <summary>
    /// Handler month selected
    /// </summary>
	private void OnMonthSelectedHandler (int month)
	{
		if(pickerType == HAGODateTimePickerType.PeriodDate)
		{
			m_dateSelectedData = new DateTime(m_dateSelectedData.Year, month, 1);
			m_onResponsePeriodDateSelectedEvent?.Invoke(m_periodType, HAGOUtils.GetPeriodStartDate(m_periodType, m_dateSelectedData), HAGOUtils.GetPeriodEndDate(m_periodType, m_dateSelectedData));
		}
		else if(pickerType == HAGODateTimePickerType.SingleDate)
		{
			m_dateSelectedData = new DateTime(m_dateSelectedData.Year, month, m_dateSelectedData.Day);
			ChangeTab(HAGODatePickerPeriodType.Day);
		}
	}

	/// <summary>
    /// Handler year selected
    /// </summary>
	private void OnYearSelectedHandler (int year)
	{
		if(pickerType == HAGODateTimePickerType.PeriodDate)
		{
			m_dateSelectedData = new DateTime(year, 1, 1);
			m_onResponsePeriodDateSelectedEvent?.Invoke(m_periodType, HAGOUtils.GetPeriodStartDate(m_periodType, m_dateSelectedData), HAGOUtils.GetPeriodEndDate(m_periodType, m_dateSelectedData));
		}
		else
		{
			m_dateSelectedData = new DateTime(year, m_dateSelectedData.Month, m_dateSelectedData.Day);
			ChangeTab(HAGODatePickerPeriodType.Month);
		}
	}

	/// <summary>
    /// Change current tab to day, month or year
    /// </summary>
	private void ChangeTab (HAGODatePickerPeriodType tab)
	{
		m_periodType = tab;

		// change color current active tab
        OnTabChangeHandler(tab);

        // update view when date change
        if (tab == HAGODatePickerPeriodType.Day || tab == HAGODatePickerPeriodType.Week)
        {
			if(pickerType == HAGODateTimePickerType.MultiDate)
			{
				m_dayView.OnDateChangeHandler(m_monthView.monthSelected, m_yearView.yearSelected, m_listDateSelectedData);
			}
			else if(pickerType == HAGODateTimePickerType.SingleDate || pickerType == HAGODateTimePickerType.PeriodDate)
			{
				m_dayView.OnDateChangeHandler(m_monthView.monthSelected, m_yearView.yearSelected, new List<DateTime>(){ m_defaultSelectedDate });
			}
        }
        else if (tab == HAGODatePickerPeriodType.Month)
        {
			//do nothing => ignore
        }
        else if (tab == HAGODatePickerPeriodType.Year)
        {
           	m_yearView.ForceScrollToYearSelected();
        }
	}

	/// <summary>
    /// Tab change handler
    /// </summary>
	private void OnTabChangeHandler (HAGODatePickerPeriodType tab)
	{
		//update color
		UpdateTabColor(m_btnTabDay, tab == HAGODatePickerPeriodType.Day);
        UpdateTabColor(m_btnTabWeek, tab == HAGODatePickerPeriodType.Week);
        UpdateTabColor(m_btnTabMonth, tab == HAGODatePickerPeriodType.Month);
        UpdateTabColor(m_btnTabYear, tab == HAGODatePickerPeriodType.Year);

        // show current active tab content
        m_dayView.gameObject.SetActive(tab == HAGODatePickerPeriodType.Day || tab == HAGODatePickerPeriodType.Week);
        m_monthView.gameObject.SetActive(tab == HAGODatePickerPeriodType.Month);
        m_yearView.gameObject.SetActive(tab == HAGODatePickerPeriodType.Year);
	}

	/// <summary>
    /// Change color tab to green when pressed or white if not
    /// </summary>
    private void UpdateTabColor (Button btnTab, bool isActive)
    {
        // active or unactive interactable current selected tab
        btnTab.interactable = !isActive;

		// update color
        btnTab.GetComponent<Image>().color = isActive ? CONST_COLOR_TAB_ACTIVE : CONST_COLOR_TAB_NORMAL;
        btnTab.transform.Find("Text").GetComponent<Text>().color = isActive ? CONST_COLOR_TEXT_ACTIVE : CONST_COLOR_TEXT_NORMAL;
		//
        if (btnTab.GetComponent<Outline>() != null)
        {
            btnTab.GetComponent<Outline>().effectColor = isActive ? CONST_COLOR_TAB_OUTLINE_ACTIVE : CONST_COLOR_TAB_OUTLINE_NORMAL;
        }
    }

	/// <summary>
    /// Response DateTime / List<DateTime> selected
    /// </summary>
	public void OnDateSelectedResponseHandler()
	{
		if(pickerType == HAGODateTimePickerType.MultiDate)
		{
			// Debug.Log("======= DateSelectedHandler =======");
			// foreach(DateTime d in m_listDateSelectedData.OrderBy(d => d.Date))
			// {
			// 	Debug.Log(d.ToString("dd MM yyyy"));
			// }
			// Debug.Log("===================================");

			m_onResponseMultipleDateSelectedEvent?.Invoke(GetMultiDateValue());
		}
		else if(pickerType == HAGODateTimePickerType.SingleDate)
		{
			// Debug.Log("======= DateSelectedHandler =======");
			// Debug.Log(m_dateSelectedData.ToString("dd MM yyyy"));
			// Debug.Log("===================================");

			m_onResponseDateSelectedEvent?.Invoke(GetSingleDateValue());
		}
	}

	public List<DateTime> GetMultiDateValue()
	{
		return m_listDateSelectedData.OrderBy(d => d.Date).ToList();
	}

	public DateTime GetSingleDateValue()
	{
		return m_dateSelectedData;
	}
}
