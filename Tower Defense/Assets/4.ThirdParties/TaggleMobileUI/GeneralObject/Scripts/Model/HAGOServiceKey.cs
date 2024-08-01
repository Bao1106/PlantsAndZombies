using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAGOServiceKey
{
	public const string PARAM_ID = "id";
	public const string PARAM_TEXT = "text";
	public const string PARAM_IMAGE = "image";
	public const string PARAM_IMGURL = "imgurl";
	public const string PARAM_NAME = "name";
	public const string PARAM_TYPE = "type";
	public const string PARAM_DESCRIBE = "describe";
	public const string PARAM_STATUS = "status";
	public const string PARAM_QUANTITY = "quantity";
	public const string PARAM_IMAGE_THUMB = "image_thumb";
	public const string PARAM_CONNECT_TYPE = "connect_type";
	public const string PARAM_VSM_STATS = "vsm_stats";
	public const string PARAM_SERVICE_UUID = "service_uuid";
	public const string PARAM_CHARACTERISTIC_UUID = "characteristic_uuid";
	public const string PARAM_DISPLAY_NAME = "display_name";
    public const string PARAM_KEY_FORM = "key_form";
    public const string PARAM_TITLE = "title";
    public const string PARAM_DATA = "data";
    public const string PARAM_RESOURCE = "resource";
    public const string PARAM_URL = "url";
    public const string PARAM_VALUE = "value";
    public const string PARAM_OPTIONS = "options";
    public const string PARAM_OPTION = "option";
    public const string PARAM_DEFAULT_VALUE = "default_value";
    public const string PARAM_ICON = "icon";
    public const string PARAM_IS_VSM = "is_vsm";
    public const string PARAM_FORM_ID = "form_id";
    public const string PARAM_DESC = "desc";
    public const string PARAM_FORM_DATA = "form_data";
    public const string PARAM_DISCLAIMER_DATA = "disclaimer_data";
    public const string PARAM_TERMS_CONTENT = "terms_content";
    public const string PARAM_TOGGLE_CONTENT = "toggle_content";
    public const string PARAM_CONTENT = "content";
    public const string PARAM_PLACEHOLDER = "placeholder";
    public const string PARAM_CONTENT_TYPE = "content_type";
    public const string PARAM_UNIT = "unit";
    public const string PARAM_MIN_LENGTH = "min_length";
    public const string PARAM_MAX_LENGTH = "max_length";
    public const string PARAM_MIN_VALUE = "min_value";
    public const string PARAM_MAX_VALUE = "max_value";
    public const string PARAM_IS_REQUIRED = "is_required";
    public const string PARAM_INTEGER = "integer";
    public const string PARAM_DECIMAL = "decimal";
    public const string PARAM_STANDARD = "standard";
    public const string PARAM_IHEALTH_SDK = "ihealth_sdk";
    public const string PARAM_STANDARD_PROFILE = "standard_profile";
    public const string PARAM_DATETIME_COMPONENT = "datetime";
    public const string PARAM_DATE_COMPONENT = "date";
    public const string PARAM_TIME_COMPONENT = "time";
    public const string PARAM_DROPDOWN_COMPONENT = "dropdown";
    public const string PARAM_CHECK_LIST_COMPONENT = "check_list";
    public const string PARAM_TOGGLE_COMPONENT = "toggle";
    public const string PARAM_TOGGLE_LIST_COMPONENT = "toggle_list";
    public const string PARAM_INPUTFIELD_COMPONENT = "inputfield";
    public const string PARAM_INPUTFIELD_MULTILINE_COMPONENT = "inputfield_multiline";
    public const string PARAM_ATTACHMENT_COMPONENT = "attachment";
    public const string PARAM_REAL_TIME = "realtime";
    public const string PARAM_LABEL = "label";
    public const string PARAM_UUID = "uuid";
    public const string PARAM_DEVICE_TYPE = "device_type";
    public const string PARAM_SCHEMA = "schema";
    public const string PARAM_JSON_SCHEMA = "json_schema";
    public const string PARAM_UI_SCHEMA = "ui_schema";
    public const string PARAM_JSON_DATA = "json_data";
    public const string PARAM_DEFAULT = "default";
    public const string PARAM_IS_SELECT_MULTIPLE = "is_select_multiple";
    public const string PARAM_TITLEMAP = "titleMap";
    public const string PARAM_CEIL = "ceil";
    public const string PARAM_STEP = "step";
    public const string PARAM_FLOOR = "floor";
    public const string PARAM_PERIOD_YEAR = "year";
    public const string PARAM_PERIOD_MONTH = "month";
    public const string PARAM_PERIOD_WEEK = "week";
    public const string PARAM_PERIOD_DAY = "day";
    public const string PARAM_SUBMIT = "submit";

    //JsonForm key
    #region JSON_FORM

    public const string PARAM_JSON_FORM_FORMAT = "format";
    public const string PARAM_JSONFORM_LAYOUT_VERTICAL = "vertical";
    public const string PARAM_JSONFORM_LAYOUT_HORIZONTAL = "horizontal";
    public const string PARAM_JSON_FORM_PROPERTIES = "properties";
    public const string PARAM_JSON_FORM_ITEMS = "items";
    public const string PARAM_JSON_FORM_TYPE = "type";
    public const string PARAM_JSON_FORM_TITLE = "title";
    public const string PARAM_JSON_FORM_MOBILE_EXTRAS = "mobile_extras";
    public const string PARAM_JSON_FORM_OPEN_FORM = "open_form";
    public const string PARAM_JSON_FORM_OPTIONS = "options";
    public const string PARAM_JSON_FORM_LAYOUT = "layout";
    public const string PARAM_JSON_FORM_TYPE_SECTION = "section";
    public const string PARAM_JSON_FORM_TYPE_HELP = "help";
    public const string PARAM_JSON_FORM_TYPE_HELPVALUE = "helpvalue";
    public const string PARAM_JSON_FORM_TYPE_BUTTON = "button";
    public const string PARAM_JSON_FORM_KEY = "key";
    public const string PARAM_JSON_FORM_ADVANCED = "advanced";
    public const string PARAM_JSON_FORM_TEXT_AREA = "textarea";
    public const string PARAM_JSON_FORM_RANGE = "range";
    public const string PARAM_JSON_FORM_HELP = "help";
    public const string PARAM_JSON_FORM_LABEL = "label";
    public const string PARAM_JSON_FORM_STRING = "string";
    public const string PARAM_JSON_FORM_ARRAY = "array";
    public const string PARAM_JSON_FORM_BOOLEAN = "boolean";
    public const string PARAM_JSON_FORM_INTEGER = "integer";
    public const string PARAM_JSON_FORM_NUMBER = "number";
    public const string PARAM_JSON_FORM_DATETIME_PICKER = "datetimepicker";
    public const string PARAM_JSON_FORM_FORMAT_DATEPICKER = "L";
    public const string PARAM_JSON_FORM_FORMAT_TIMEPICKER = "LT";
    public const string PARAM_JSON_FORM_FORMAT_DATETIMEPICKER = "L LT";
    public const string PARAM_JSON_FORM_RADIOS = "radios";
    public const string PARAM_JSON_FORM_RADIO_BUTTONS = "radiobuttons";
    public const string PARAM_JSON_FORM_CHECKBOX = "checkbox";
    public const string PARAM_JSON_FORM_HELP_COMPONENT = "help";
    public const string PARAM_JSON_FORM_SELECT = "select";
    public const string PARAM_JSON_FORM_SELECT_MULTIPLE = "uiselectmultiple";
    public const string PARAM_JSON_FORM_TYPE_OPEN_APPDATAFORM = "open_appdataform";
    
    #endregion


    //JSONForms key
    #region ECLIPSE_JSONFORMS
    
    // public const string PARAM_JSONFORMS_UISCHEMA_HORIZONTAL_LAYOUT = "HorizontalLayout";
    // public const string PARAM_JSONFORMS_UISCHEMA_VERTICAL_LAYOUT = "VerticalLayout";
    // public const string PARAM_JSONFORMS_UISCHEMA_LABEL = "Label";
    // public const string PARAM_JSONFORMS_UISCHEMA_CONTROL = "Control";
    // public const string PARAM_JSONFORMS_ELEMENTS = "elements";
    // public const string PARAM_JSONFORMS_ITEMS = "items";
    // public const string PARAM_JSONFORMS_SCOPE = "scope";
    // public const string PARAM_JSONFORMS_TEXT = "text";
    // public const string PARAM_JSONFORMS_STRING = "string";
    // public const string PARAM_JSONFORMS_BOOLEAN = "boolean";
    // public const string PARAM_JSONFORMS_INTEGER = "integer";
    // public const string PARAM_JSONFORMS_NUMBER = "number";
    // public const string PARAM_JSONFORMS_DATE = "date";
    // public const string PARAM_JSONFORMS_DATETIME = "datetime";
    // public const string PARAM_JSONFORMS_TIME = "time";
    // public const string PARAM_JSONFORMS_RADIO = "radio";
    // public const string PARAM_JSONFORMS_CHECKBOX = "checkbox";
    // public const string PARAM_JSONFORMS_PROPERTIES = "properties";
    // public const string PARAM_JSONFORMS_LABEL = "label";
    // public const string PARAM_JSONFORMS_MULTI = "multi";
    // public const string PARAM_JSONFORMS_RULE = "rule";
    // public const string PARAM_JSONFORMS_EFFECT = "effect";
    // public const string PARAM_JSONFORMS_DISABLE = "DISABLE";

    #endregion
}
