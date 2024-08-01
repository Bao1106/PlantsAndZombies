﻿using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGODateTimePickerControl
{
    private static HAGODateTimePickerControl m_api;
    public static HAGODateTimePickerControl Api
    {
        get
        {
            if (m_api == null)
            {
                m_api = new HAGODateTimePickerControl();
            }
            return m_api;
        }
    }
    
    //event
    public Action<DateTime> ResultCallbackDateTimeEvent;
    public Action<List<DateTime>> ResultCallbackMultiDateEvent;
    public Action<HAGODatePickerPeriodType, DateTime, DateTime> ResultCallbackPeriodDateTimeEvent;
    //
    public Action<TimeSpan> ResultCallbackTimeSpanEvent;
    //
    public Action<Action> ExitEvent;

    public void CompleteTimePicker(TimeSpan time)
    {
        ResultCallbackTimeSpanEvent?.Invoke(time);
        Exit();
    }

    public void CompleteSingleDatePicker(DateTime date)
    {
        ResultCallbackDateTimeEvent?.Invoke(date);
        Exit();
    }

    public void CompleteMultiDatePicker(List<DateTime> rs)
    {
        ResultCallbackMultiDateEvent?.Invoke(rs);
        Exit();
    }

    public void CompletePeriodDatePicker(HAGODatePickerPeriodType period, DateTime startDate, DateTime endDate)
    {
        ResultCallbackPeriodDateTimeEvent?.Invoke(period, startDate, endDate);
        Exit();
    }

    public void Exit()
    {
        ExitEvent?.Invoke(() => HAGODateTimePickerManager.Api.Destroy());
    }

    #region DATEPICKER_CONTROL

    public Action<DateTime> OnDateSelectedEvent { get; set; } // notify date selected event
    public Action<DateTime> OnDateDeselectedEvent { get; set; } // notify date deselected event
    public Action<int> OnMonthSelectedEvent { get; set; } // notify month selected event
    public Action<int> OnYearSelectedEvent { get; set; } // notify year selected event
    
    // notify main view when selected dayItem in dayView
    public void DateSelected(DateTime date)
    {
        // Debug.Log("DateSelected " + date.ToString());
        OnDateSelectedEvent?.Invoke(date);
    }

    // notify main view when deselected dayItem in dayView
    public void DateDeselected(DateTime date)
    {
        // Debug.Log("DateDeselected " + date.ToString());
        OnDateDeselectedEvent?.Invoke(date);
    }

    // notify main view when selected monthItem in monthView
    public void MonthSelected(int month)
    {
        // Debug.Log("MonthSelected " + month.ToString());
        OnMonthSelectedEvent?.Invoke(month);
    }

    // notify main view when selected yearItem in yearView
    public void YearSelected(int year)
    {
        // Debug.Log("YearSelected " + year.ToString());
        OnYearSelectedEvent?.Invoke(year);
    }

    #endregion
}