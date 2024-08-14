using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAGOConstant
{
	//path
	public const string PATH_LANGUAGE = "Language/HAGOLanguage";

	//mockup config
	public const string CONFIG_PAIR_DEVICE_TYPE = "Config/hago_pairdevice_device_type";

	//prefab
	public const string PREFAB_PAIR_DEVICE = "GOPairDeviceComp";
	public const string PREFAB_SHOW_REWARD = "GOShowRewardComp";
	public const string PREFAB_DATETIME_PICKER = "GODateTimePickerComp";
    public const string PREFAB_EMOJI_PICKER = "GOEmojiPickerComp";
    public const string PREFAB_ITEM_PICKER = "GOItemPickerComp";
	public const string PREFAB_WEBVIEW = "GOWebViewComp";
    public const string PREFAB_FPS_MANAGER = "GOFPSManager";
    public const string PREFAB_QUIZ = "GOQuizComp";
	//
    public const string PREFAB_SUBMIT_JSON_FORMS = "GOUISubmitJsonFormComp";
	public const string PREFAB_COMPONENT_DATETIME_ITEM = "Components/GOUIDateTimeComp";
    public const string PREFAB_COMPONENT_DATE_ITEM = "Components/GOUIDateComp";
    public const string PREFAB_COMPONENT_TIME_ITEM = "Components/GOUITimeComp";
    public const string PREFAB_COMPONENT_DROPDOWN_ITEM = "Components/GOUIDropdownListComp";
    public const string PREFAB_COMPONENT_CHECK_LIST_ITEM = "Components/GOUICheckListComp";
    public const string PREFAB_COMPONENT_TOGGLE_LIST_ITEM = "Components/GOUIToggleListComp";
    public const string PREFAB_COMPONENT_TOGGLE_ITEM = "Components/GOUIToggleComp";
    public const string PREFAB_COMPONENT_INPUTFIELD_ITEM = "Components/GOUIInputfieldComp";
    public const string PREFAB_COMPONENT_INPUTFIELD_LIST_ITEM = "Components/GOUIInputfieldListComp";
    public const string PREFAB_COMPONENT_INPUTFIELD_MULTILINE_ITEM = "Components/GOUIInputfieldMultilineComp";
    public const string PREFAB_COMPONENT_HELP_ITEM = "Components/GOUIHelpComp";
    public const string PREFAB_COMPONENT_ATTACHMENT_ITEM = "Components/GOUIAttachmentComp";
    public const string PREFAB_COMPONENT_HORIZONTAL_LAYOUT = "Components/GOUIHorizontalLayout";
    public const string PREFAB_COMPONENT_VERTICAL_LAYOUT = "Components/GOUIVerticalLayout";
    public const string PREFAB_COMPONENT_LABEL_ITEM = "Components/GOUILabel";
    public const string PREFAB_COMPONENT_SLIDER_ITEM = "Components/GOUISliderComp";
    public const string PREFAB_COMPONENT_SELECT_TYPE_ITEM = "Components/GOUISelectTypeComp";
    public const string PREFAB_COMPONENT_BUTTON_ITEM = "Components/GOUIButtonComp";

	//color
	public static string COLOR_TEXT_DARK_DEFAULT //The default text dark color
    {
        get { return "#424242"; }
    }

    public static string COLOR_ERROR //The error red color
    {
        get { return "#F44336"; }
    }
    
    public static string COLOR_HIGHLIGHT //The highlight cyan color
    {
        get { return "#23B188"; }
    }
    
    public static string COLOR_TAB_ICON_NORMAL
    {
        get { return "#727272"; }
    }

    public static string COLOR_PRIMARY => "#00AFAF";

    //format
    public static string FORMAT_DATE //format date
    {
        get { return "dd MMM yyyy"; }
    }

    public static string FORMAT_CHART_DATE //format date for chart
    {
        get { return "dd/MM"; }
    }

    public static string FORMAT_DATE_DAY //format date day
    {
        get { return "dd"; }
    }

    public static string FORMAT_DATE_DAY_MONTH //format date day month
    {
        get { return "dd MMM"; }
    }

    public static string FORMAT_DATE_MONTH_YEAR //format date month year
    {
        get { return "MMMM yyyy"; }
    }

    public static string FORMAT_DATE_YEAR //format date year
    {
        get { return "yyyy"; }
    }

    public static string FORMAT_TIME_12_HOURS //format 12h time
    {
        get { return "hh:mm tt"; }
    }

    public static string FORMAT_TIME_DURATION //format time duration
    {
        get { return "HH:mm:ss"; }
    }

    public static string FORMAT_DATETIME_ISO_O
    {
        get { return "o"; }
    }
}