using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening;
using Honeti;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class HAGOUtils
{
    public static bool ContainsKey(this JObject jobject, string key)
    {
        return jobject[key] != null;
    }

    public static string GetMonthName(int index)
    {
        string monthName = string.Empty;
        switch (index)
        {
            case 1:
                monthName = HAGOLangConstant.JANUARY;
                break;
            case 2:
                monthName = HAGOLangConstant.FEBRUARY;
                break;
            case 3:
                monthName = HAGOLangConstant.MARCH;
                break;
            case 4:
                monthName = HAGOLangConstant.APRIL;
                break;
            case 5:
                monthName = HAGOLangConstant.MAY;
                break;
            case 6:
                monthName = HAGOLangConstant.JUNE;
                break;
            case 7:
                monthName = HAGOLangConstant.JULY;
                break;
            case 8:
                monthName = HAGOLangConstant.AUGUST;
                break;
            case 9:
                monthName = HAGOLangConstant.SEPTEMBER;
                break;
            case 10:
                monthName = HAGOLangConstant.OCTOBER;
                break;
            case 11:
                monthName = HAGOLangConstant.NOVEMBER;
                break;
            case 12:
                monthName = HAGOLangConstant.DECEMBER;
                break;
            default:
                return string.Empty;
        }
        
        return I18N.instance.getValue(monthName);
    }

    // public static HAGOConnectType ParseConnectType(string connectType)
    // {
    //     switch(connectType)
    //     {
    //         case HAGOServiceKey.PARAM_IHEALTH_SDK:
    //             return HAGOConnectType.IHEALTH_SDK;
            
    //         case HAGOServiceKey.PARAM_STANDARD_PROFILE:
    //         default:
    //             return HAGOConnectType.BLUETOOTH_PROFILE;
    //     }
    // }

    // public static BluetoothBasic.BLE_TypeValue ConvertStringToBLETypeValue(string value)
    // {
    //     switch(value.ToLower())
    //     {
    //         case "decimal":
    //             return BluetoothBasic.BLE_TypeValue.DECIMAL;

    //         case "double":
    //             return BluetoothBasic.BLE_TypeValue.DOUBLE;

    //         case "string":
    //             return BluetoothBasic.BLE_TypeValue.STRING;

    //         case "single":
    //             return BluetoothBasic.BLE_TypeValue.SINGLE;

    //         case "float":
    //             return BluetoothBasic.BLE_TypeValue.FLOAT;

    //         default:
    //             return BluetoothBasic.BLE_TypeValue.FLOAT;
    //     }
    // }

    public static DateTime GetDateTimeFromEpoch(long seconds)
    {
        DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return date.AddSeconds(seconds).ToLocalTime();
    }

    public static DateTime GetDateTimeFromEpochUtc(long seconds)
    {
        DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return date.AddSeconds(seconds);
    }

    public static long GetEpochTimeFromDateTime(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return (long) Math.Floor(diff.TotalSeconds);
    }

    public static bool IsLangKey(string title)
    {
        return title?.StartsWith("^") ?? false;
    }

    public static Type GetComponentType(string compKey)
    {
        if(HAGOSubmitFormModel.Api.ComponentDict.ContainsKey(compKey))
        {
            return HAGOSubmitFormModel.Api.ComponentDict[compKey];
        }

        return null;
    }

    public static string GetComponentKey(Type type)
    {
        foreach(KeyValuePair<string, Type> comp in HAGOSubmitFormModel.Api.ComponentDict)
        {
            if(comp.Value == type)
            {
                return comp.Key;
            }
        }

        return string.Empty;
    }

    public static Type GetComponentViewType(string compKey)
    {
        if(HAGOSubmitFormModel.Api.ComponentViewDict.ContainsKey(compKey))
        {
            return HAGOSubmitFormModel.Api.ComponentViewDict[compKey];
        }

        return null;
    }

    public static MethodInfo GetComponentViewMethod(GameObject go, out string typeStr, out Type type)
    {
        type = null;
        typeStr = string.Empty;
        // var comp = go.GetComponent<IFoo>();
        foreach(var compView in HAGOSubmitFormModel.Api.ComponentViewDict)
        {

            type = compView.Value;
            typeStr = compView.Key.ToString();
            Debug.Log("=====typeStr " + typeStr);

            MethodInfo method = type.GetMethod("ExportView", new Type[]{ typeof(int) });
            return method;
        }

        return null;
    }

    public static Color ParseColorFromString(string color)
    {
        if(!color.StartsWith("#"))
        {
            color = "#" + color;
        }
        
        Color result;
        if (ColorUtility.TryParseHtmlString(color, out result))
        {
            return result;
        }
        return Color.white;
    }

    public static String ParseColorToHex(Color c)
    {
        return "#" + ColorUtility.ToHtmlStringRGB(c);
    }

    public static Color GetColorFromPaletteIndex(int index, bool reverseColor = false)
    {
        if(reverseColor)
        {
            index = 9 - int.Parse(index.ToString().ToCharArray().LastOrDefault().ToString());
        }

        string colorHex = HAGOConstant.COLOR_PRIMARY;
        switch(index)
        {
            case 0: 
                colorHex = "#F44336";
                    break;

            case 1: 
                colorHex = "#2196F3";
                    break;

            case 2: 
                colorHex = "#23B188";
                    break;

            case 3: 
                colorHex = "#EF69AA";
                    break;

            case 4: 
                colorHex = "#305F72";
                    break;

            case 5:
                colorHex = "#795548";
                    break;

            case 6:
                colorHex = "#F18C8E";
                    break;

            case 7:
                colorHex = "#64DD17";
                    break;

            case 8:
                colorHex = "#E738A8";
                    break;

            case 9:
                colorHex = "#FF9800";
                    break;

            default:
                    colorHex = "#727272";
                    break;
        }

        return ParseColorFromString(colorHex);
    }

    public static string GetBase64StringFromTexture(Texture2D texture)
    {
        if(texture == null)
        {
            return string.Empty;
        }
        
        byte[] bytes = texture.EncodeToJPG();
        return "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
    }

    public static Byte[] GetDataFromAudioClip(AudioClip clip)
    {
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];
        Byte[] bytesData = new Byte[samples.Length * 2];

        int rescaleFactor = 32767; 

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        return bytesData;
    }

    public static string ReplaceEmojiName(string unicodeRaw) {
        var chars = new List<char>();
        // some characters are multibyte in UTF32, split them
        foreach (var point in unicodeRaw.Split('-'))
        {
            // parse hex to 32-bit unsigned integer (UTF32)
            uint unicodeInt = uint.Parse(point, System.Globalization.NumberStyles.HexNumber);
            // convert to bytes and get chars with UTF32 encoding
            chars.AddRange(Encoding.UTF32.GetChars(BitConverter.GetBytes(unicodeInt)));
        }
        // this is resulting emoji
        return new string(chars.ToArray());
    }

    public static string ConvertCamelCaseToTitleCase(string s)
    {
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        return ti.ToTitleCase(Regex.Replace(s,"[A-Z]"," $0"));
    }

    public static bool IsCamelCase(string content)
    {
        return ToCamelCase(content) == content;
    }

    public static string ToCamelCase(string str)
    {                    
        if(!string.IsNullOrEmpty(str) && str.Length > 1)
        {
            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        return str;
    }

    public static bool IsSnakeCase(string content)
    {
        return content.Contains("_");
    }

    public static string ConvertSnakeCaseToTitleCase(string content)
    {
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        return ti.ToTitleCase(content.ToLower().Replace("_", " "));
    }

	public static DateTime GetPeriodStartDate(HAGODatePickerPeriodType periodType, DateTime date)
    {
        switch(periodType)
        {
            case HAGODatePickerPeriodType.Year:
                return new DateTime(date.Year, 1, 1);

            case HAGODatePickerPeriodType.Month:
                return new DateTime(date.Year, date.Month, 1);

            case HAGODatePickerPeriodType.Day:
                return date.Date;

            case HAGODatePickerPeriodType.Week:
            default:
                return GetFirstDayOfWeek(date);
        }
    }

    public static DateTime GetPeriodEndDate(HAGODatePickerPeriodType periodType, DateTime date)
    {
        switch(periodType)
        {
            case HAGODatePickerPeriodType.Year:
                return date.Date.AddYears(1).AddSeconds(-1);

            case HAGODatePickerPeriodType.Month:
                return date.Date.AddMonths(1).AddSeconds(-1);

            case HAGODatePickerPeriodType.Day:
                return date.Date.AddDays(1).AddSeconds(-1);

            case HAGODatePickerPeriodType.Week:
            default:
                return GetFirstDayOfWeek(date).Date.AddDays(7).AddSeconds(-1);
        }
    }

    //get new date after change back/next by navigation bar
    public static DateTime GetNavigationPeriodDate(HAGODatePickerPeriodType periodType, bool isNext, DateTime date)
    {
        switch(periodType)
        {
            case HAGODatePickerPeriodType.Year:
                return date.Date.AddYears(isNext ? 1 : -1);

            case HAGODatePickerPeriodType.Month:
                return date.Date.AddMonths(isNext ? 1 : -1);
                
            case HAGODatePickerPeriodType.Day:
                return date.Date.AddDays(isNext ? 1 : -1);

            case HAGODatePickerPeriodType.Week:
            default:
                return date.Date.AddDays(isNext ? 7 : -7);
        }
    }

    public static string ConvertDatePickerPeriodTypeToString(HAGODatePickerPeriodType periodType)
    {
        switch(periodType)
        {
            case HAGODatePickerPeriodType.Year:
                return HAGOServiceKey.PARAM_PERIOD_YEAR;

            case HAGODatePickerPeriodType.Month:
                return HAGOServiceKey.PARAM_PERIOD_MONTH;
                
            case HAGODatePickerPeriodType.Day:
                return HAGOServiceKey.PARAM_PERIOD_DAY;

            case HAGODatePickerPeriodType.Week:
            default:
                return HAGOServiceKey.PARAM_PERIOD_WEEK;
        }
    }

    public static DateTime GetDateTimeFromISOString(string dtStr)
    {
        return DateTime.Parse(dtStr, null, DateTimeStyles.RoundtripKind);
    }

    public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
    {
        DayOfWeek firstDay = DayOfWeek.Monday;
        DateTime firstDayInWeek = dayInWeek.Date;
        while (firstDayInWeek.DayOfWeek != firstDay)
        {
            firstDayInWeek = firstDayInWeek.AddDays(-1);
        }

        return firstDayInWeek;
    }

    public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
    {
        DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
        DateTime firstDayInWeek = dayInWeek.Date;
        while (firstDayInWeek.DayOfWeek != firstDay)
        {
            firstDayInWeek = firstDayInWeek.AddDays(-1);
        }

        return firstDayInWeek;
    }

    public static string RemoveHTMLTag(string input)
    {
        return Regex.Replace(input, "<.*?>", String.Empty);
    }

    public static string RemoveHeaderStyleTag(string input)
    {
        input = Regex.Replace(input, "<h.*?>", string.Empty);
        input = Regex.Replace(input, "</h.*?>", string.Empty);
        return input;
    }

    public static string ToISOStringDatetime(this DateTime dateTime)
    {
        return dateTime.ToString(HAGOConstant.FORMAT_DATETIME_ISO_O);
    }
    
    public static JObject MergeForm(params JObject[] forms)
    {
        if(forms.Length == 0)
            return new JObject();

        if(forms.Length == 1)
            return forms[0];

        JObject formFirst = forms[0];
        foreach(JObject formSecond in forms.Where(x => x != formFirst))
        {
            formFirst.Merge(formSecond, new JsonMergeSettings(){ MergeArrayHandling = MergeArrayHandling.Union });
        }

        return formFirst;
    }
    
    public static JObject MergeForm(JObject formFirst, Dictionary<string, JObject> nestedForms)
    {
        JObject rs = formFirst ?? new JObject();
 
        if(nestedForms != null)
        {
            foreach(var form in nestedForms)
            {
                foreach(KeyValuePair<string, JToken> jtk in form.Value)
                {
                    if(!rs.ContainsKey(jtk.Key))
                    {   
                        rs.Add(jtk.Key, jtk.Value);
                    }
                    else
                    {
                        rs[jtk.Key] = jtk.Value;
                    }
                }
            }
        }

        return rs;
    }

    public static string GetInitialsName(string name)
    {
        if(string.IsNullOrEmpty(name))
            return string.Empty;

        string[] words = name.Split(' ');
        return words.Length == 1 ?
                    (name.Length >= 2 ? string.Format("{0}{1}", name[0].ToString().ToUpper(), name[1].ToString().ToLower()) : name[0].ToString().ToUpper()) : 
                    string.Format("{0}{1}", words[0][0], words[1][0]).ToUpper();
    }

    public static string GetLanguageValue(string content)
    {
        if(string.IsNullOrEmpty(content))
            return string.Empty;

        if(!content.StartsWith("^") && I18N.instance.HasKey("^" + content))
        {
            content = "^" + content;
        }

        string langKey = content.ToUpper();
        return I18N.instance.HasKey(langKey) ? I18N.instance.getValue(langKey) :
                IsSnakeCase(content) ? ConvertSnakeCaseToTitleCase(content) : content;
    }
}
