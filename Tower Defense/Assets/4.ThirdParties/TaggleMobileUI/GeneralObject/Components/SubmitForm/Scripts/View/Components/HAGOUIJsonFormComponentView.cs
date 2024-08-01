using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Honeti;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HAGOUIJsonFormComponentView : MonoBehaviour
{
    public bool IsInitByUser = false;
    private Transform m_content;

    //param
    private HAGOUIJsonFormComponentDTO m_data;
    private Action<RectTransform> m_onErrorHandler;
    private Dictionary<string, HAGOUIIComponent> m_dictComponentCheckValid = new Dictionary<string, HAGOUIIComponent>();
    private Action<string, JObject> m_onClickFormButtonHandler;

    void Start()
    {
        if(!IsInitByUser)
        {
            Init(null);
        }
    }
    
    public void Init(HAGOUIJsonFormComponentDTO data, Action<RectTransform> onErrorHandler = null, Action<string, JObject> onClickFormButtonHandler = null)
    {
        m_data = data;
        m_onErrorHandler = onErrorHandler;
        m_onClickFormButtonHandler = onClickFormButtonHandler;

        //find reference
        m_content = this.transform;

        if (m_data != null) //handle dynamic UI value
        {
            InitDynamicComponents(m_data);
        }
    }

    private void InitDynamicComponents(HAGOUIJsonFormComponentDTO data)
    {
        //clear old items
        foreach (Transform tf in transform)
        {
            Destroy(tf.gameObject);
        }

        //keep for future: eclips/jsonforms
        // GenerateJSONFormView(...);

        //jsonform
        GenerateJsonFormView(data);
    }

    #region JSON_FORM

    //step 1. get items from definition
    //step 2. Loop items and check if JToken or JObject by key "type"
    //step 3.1. If schema properties has type:"section" => render layout then return to step 1
    //     3.2. else check if item is string or JObject (field with advanced data) to render fields then go to step 4
    //step 4. render field by using joSchema
    private void GenerateJsonFormView(HAGOUIJsonFormComponentDTO data)
    {
        //Debug.Log($"<color=blue>=== GenerateJsonFormView schema: {JsonConvert.SerializeObject(data.Schema)}</color>");
        //Debug.Log($"<color=blue>=== GenerateJsonFormView form: {JsonConvert.SerializeObject(data.Definition)}</color>");
        
        foreach(JToken jtItem in data.Definition)
        {
            GenerateJsonFormItemView(data.Schema, jtItem, data.Data, m_content);
        }
    }

    private void GenerateJsonFormItemView(JObject joSchema, JToken jtItemForm, JObject joData, Transform content)
    {
        try
        {
            //Debug.Log($"<color=green>=== GenerateJsonFormItemView</color>");
            
            //Debug.Log($"joSchema {JsonConvert.SerializeObject(joSchema)}");
            //Debug.Log($"jtItemForm {JsonConvert.SerializeObject(jtItemForm)}");
            //Debug.Log($"joData {JsonConvert.SerializeObject(joData)}");

            JObject joItemForm = jtItemForm.HasValues ? (JObject)jtItemForm : null;
            string itemType = joItemForm != null && joItemForm.ContainsKey(HAGOServiceKey.PARAM_JSON_FORM_TYPE) ? joItemForm.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_TYPE) : string.Empty;
            
            GameObject prefabItem = null;

            //Debug.Log($"itemType {itemType}");

            //generate layout
            if(itemType == HAGOServiceKey.PARAM_JSON_FORM_TYPE_SECTION)
            {
                if(joItemForm != null)
                {
                    string layoutType = !string.IsNullOrEmpty(itemType) && joItemForm.ContainsKey(HAGOServiceKey.PARAM_JSON_FORM_LAYOUT) ? joItemForm.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_LAYOUT) : string.Empty;

                    //Debug.Log($"<color=blue>=== GenerateJsonFormItemView ==={itemType}</color>");
                    
                    switch(layoutType)
                    {
                        // case HAGOServiceKey.PARAM_JSONFORM_LAYOUT_HORIZONTAL:
                        //     prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_HORIZONTAL_LAYOUT);
                        //     break;

                        default:
                            //NOTE: force layout to vertical on mobile due to lack of space
                            prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_VERTICAL_LAYOUT);
                            break;
                    }

                    //create item
                    GameObject goItem = CreateItem(prefabItem, content, layoutType);

                    

                    if(layoutType == HAGOServiceKey.PARAM_JSONFORM_LAYOUT_HORIZONTAL)
                    {
                        goItem.GetComponent<LayoutGroup>().padding = new RectOffset(0,0,0,0);
                    }

                    //generate child elements
                    JArray jaItems = joItemForm.Value<JArray>(HAGOServiceKey.PARAM_JSON_FORM_ITEMS) ?? new JArray();
                    //
                    foreach(JToken joElement in jaItems)
                    {
                        GenerateJsonFormItemView(joSchema, joElement, joData, goItem.transform);
                    }
                }
            }
            else if(itemType == HAGOServiceKey.PARAM_JSON_FORM_TYPE_HELP)
            {
                prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_LABEL_ITEM);

                //create item
                GameObject goItem = CreateItem(prefabItem, content, HAGOServiceKey.PARAM_JSON_FORM_TYPE_HELP);
                TextMeshProUGUI txtContent = goItem.GetComponent<TextMeshProUGUI>();

                string helpValue = joItemForm != null && joItemForm.ContainsKey(HAGOServiceKey.PARAM_JSON_FORM_TYPE_HELPVALUE) ? joItemForm.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_TYPE_HELPVALUE) : string.Empty;

                //content properties
                Color color = HAGOUtils.ParseColorFromString(helpValue.Contains("h1") ? HAGOConstant.COLOR_PRIMARY : HAGOConstant.COLOR_TEXT_DARK_DEFAULT);
                int fontSize = helpValue.Contains("h1") ? 36 : helpValue.Contains("h2") ? 32 : 30;
                TextAlignmentOptions alignment = helpValue.Contains("h1") || helpValue.Contains("h2") ? TextAlignmentOptions.Midline : TextAlignmentOptions.MidlineLeft;

                //reformat style
                string textContent = helpValue.Replace("\n", string.Empty);
                textContent = textContent.Replace("<br>", "\n");
                textContent = textContent.Replace("<strong>", "<b>");
                textContent = textContent.Replace("</strong>", "</b>");
                textContent = textContent.Replace("<small>", "<color=#727272><size=24>");
                textContent = textContent.Replace("</small>", "</size></color>");
                textContent = HAGOUtils.RemoveHeaderStyleTag(textContent);
                
                txtContent.text = textContent;
                txtContent.fontSize = fontSize;
                txtContent.alignment = alignment;
                txtContent.color = color;
            }
            else if(itemType == HAGOServiceKey.PARAM_JSON_FORM_TYPE_BUTTON)
            {
                prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_BUTTON_ITEM);
                GameObject goItem = CreateItem(prefabItem, content, HAGOServiceKey.PARAM_JSON_FORM_TYPE_BUTTON);

                string title = joItemForm.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_TITLE);
                goItem.transform.Find("TxtLabel").GetComponent<Text>().text = title;

                string buttonKey = joItemForm.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_KEY);
                JObject joMobileExtras = joItemForm.Value<JObject>(HAGOServiceKey.PARAM_JSON_FORM_MOBILE_EXTRAS);
                //
                goItem.GetComponent<Button>().onClick.AddListener(() => {
                    m_onClickFormButtonHandler?.Invoke(buttonKey, joMobileExtras);
                });
            }
            //generate field items
            else
            {
                //main params
                string itemFullPropertyKey = joItemForm != null ? joItemForm.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_KEY) : (string)jtItemForm;
                //Debug.Log($"itemFullPropertyKey: {itemFullPropertyKey}");
                
                string itemPropertyKey = itemFullPropertyKey.Split('.').LastOrDefault();
                //Debug.Log($"itemPropertyKey: {itemFullPropertyKey}");

                HAGOUIJsonSchemaDTO schemaDTO = joSchema.ToObject<HAGOUIJsonSchemaDTO>();
                JObject joItemSchema = (JObject)schemaDTO.Properties?.SelectToken(itemPropertyKey) ?? null;

                //Debug.Log($"joItemSchema Properties: {joItemSchema}");

                if(joItemSchema == null)
                {
                    joItemSchema = (JObject)schemaDTO.ExtraProperties?.SelectToken(itemPropertyKey) ?? null;
                    //Debug.Log($"joItemSchema ExtraProperties: {joItemSchema}");
                }

                HAGOUIJsonSchemaDTO itemSchema = joItemSchema != null && joItemSchema.HasValues ? joItemSchema.ToObject<HAGOUIJsonSchemaDTO>() : null;

                //Debug.Log($"<color=blue>=== GenerateJsonFormItemView: {itemFullPropertyKey}</color>");

                //sub params
                bool isCacheCompCheckValid = schemaDTO.Properties?.ContainsKey(itemPropertyKey) ?? false;
                //
                if(itemSchema != null)
                {
                    //Debug.Log($"[{itemFullPropertyKey}] itemSchema.Type {itemSchema.Type}");

                    switch(itemSchema.Type)
                    {
                        case HAGOServiceKey.PARAM_JSON_FORM_ARRAY:
                        {
                            if(itemSchema.Advanced != null)
                            {
                                string advanceType = itemSchema.Advanced.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_TYPE);

                                switch(advanceType)
                                {
                                    case HAGOServiceKey.PARAM_JSON_FORM_SELECT_MULTIPLE:
                                        prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_SELECT_TYPE_ITEM);
                                        break;

                                    default:
                                        prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_DROPDOWN_ITEM);
                                        break;
                                }
                            }
                            else
                            {
                                prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_INPUTFIELD_LIST_ITEM);
                            }
                            break;
                        }

                        case HAGOServiceKey.PARAM_JSON_FORM_STRING:
                        {
                            if(itemSchema.EnumData != null && itemSchema.EnumData.Length > 0)
                            {
                                switch(itemSchema.Format ?? string.Empty)
                                {
                                    case HAGOServiceKey.PARAM_JSON_FORM_RADIOS:
                                        prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_TOGGLE_LIST_ITEM);
                                        break;

                                    case HAGOServiceKey.PARAM_JSON_FORM_CHECKBOX:
                                        prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_CHECK_LIST_ITEM);
                                        break;
                                    
                                    default:
                                        prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_DROPDOWN_ITEM);
                                        break;
                                }
                            }
                            else if(itemSchema.Advanced != null)
                            {
                                string advanceType = itemSchema.Advanced.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_TYPE);

                                //Debug.Log($"[{itemFullPropertyKey}] advanceType.Type {advanceType}");

                                switch(advanceType)
                                {
                                    case HAGOServiceKey.PARAM_JSON_FORM_SELECT:
                                        prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_DROPDOWN_ITEM);
                                        break;

                                    case HAGOServiceKey.PARAM_JSON_FORM_TEXT_AREA:
                                        prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_INPUTFIELD_MULTILINE_ITEM);
                                        break;

                                    case HAGOServiceKey.PARAM_JSON_FORM_HELP:
                                        prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_HELP_ITEM);
                                        break;

                                    case HAGOServiceKey.PARAM_JSON_FORM_DATETIME_PICKER:
                                    {
                                        string optionsFormat = (string)itemSchema.Advanced.SelectToken(string.Format("{0}.{1}", HAGOServiceKey.PARAM_JSON_FORM_OPTIONS,HAGOServiceKey.PARAM_JSON_FORM_FORMAT));
                                        if(!string.IsNullOrEmpty(optionsFormat))
                                        {
                                            switch(optionsFormat)
                                            {
                                                case HAGOServiceKey.PARAM_JSON_FORM_FORMAT_DATETIMEPICKER:
                                                    prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_DATETIME_ITEM);
                                                    break;

                                                case HAGOServiceKey.PARAM_JSON_FORM_FORMAT_DATEPICKER:
                                                    prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_DATE_ITEM);
                                                    break;

                                                case HAGOServiceKey.PARAM_JSON_FORM_FORMAT_TIMEPICKER:
                                                    prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_TIME_ITEM);
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                        break;
                                    }

                                    default:
                                    {
                                        prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_INPUTFIELD_ITEM);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_INPUTFIELD_ITEM);
                            }
                            break;
                        }

                        case HAGOServiceKey.PARAM_JSON_FORM_INTEGER:
                        case HAGOServiceKey.PARAM_JSON_FORM_NUMBER:
                        {
                            if(itemSchema.Advanced != null)
                            {
                                string advanceType = itemSchema.Advanced.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_TYPE);

                                switch(advanceType)
                                {
                                    case HAGOServiceKey.PARAM_JSON_FORM_RANGE:
                                    {
                                        prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_SLIDER_ITEM);
                                        break;
                                    }

                                    default:
                                    {
                                        prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_INPUTFIELD_ITEM);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_INPUTFIELD_ITEM);
                            }
                            break;
                        }
                        
                        case HAGOServiceKey.PARAM_JSON_FORM_BOOLEAN:
                        {
                            prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_TOGGLE_ITEM);
                            break;
                        }

                        default:
                            break;
                    }

                    if(prefabItem == null)
                    {
                        return;
                    }

                    //create item
                    GameObject goItem = CreateItem(prefabItem, content, itemFullPropertyKey);

                    //handle data
                    InitJsonFormComponent(
                        itemPropertyKey: itemFullPropertyKey, 
                        itemSchema: itemSchema, 
                        joForm: joItemForm,
                        joData: joData,
                        goItem: goItem,
                        isDisable: false,
                        isRequired: schemaDTO.Required?.Contains(itemFullPropertyKey) ?? false
                    );

                    //cache to check valid later
                    if(isCacheCompCheckValid)
                    {
                        if(!m_dictComponentCheckValid.ContainsKey(itemFullPropertyKey))
                        {
                            m_dictComponentCheckValid.Add(itemFullPropertyKey, goItem.GetComponent<HAGOUIIComponent>());
                        }
                        else
                        {
                            //Debug.Log($"<color=red>Error same key m_dictComponentCheckValid: {itemFullPropertyKey}</color>");
                        }
                    }
                }
                else
                {
                    //Debug.Log($"<color=red>Parse field property failed: {itemFullPropertyKey}</color>");
                }
            }
        }
        catch(Exception ex)
        {
            //Debug.Log("GenerateJsonFormItemView failed: " + ex.ToString());
        }
    }

    private void InitJsonFormComponent(string itemPropertyKey, HAGOUIJsonSchemaDTO itemSchema, JObject joForm, JObject joData, GameObject goItem, bool isDisable = false, bool isRequired = false)
    {
        object data = null;
        //
        JToken jtFormType = joForm?.SelectToken(HAGOServiceKey.PARAM_JSON_FORM_TYPE) ?? null;
        string formType = jtFormType != null ? (string)jtFormType : string.Empty;
        
        switch(itemSchema.Type ?? string.Empty)
        {
            case HAGOServiceKey.PARAM_JSON_FORM_ARRAY:
            {
                if(itemSchema.Advanced != null)
                {
                    string advanceType = itemSchema.Advanced.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_TYPE);

                    switch(advanceType)
                    {
                        case HAGOServiceKey.PARAM_JSON_FORM_SELECT_MULTIPLE:
                        {
                            data = GenerateDataMultipleSelect(itemPropertyKey, itemSchema, joData);
                            break;
                        }

                        default:
                        {
                            data = GenerateDataDropdown(itemPropertyKey, itemSchema, joData);
                            break;
                        }
                    }
                }
                else
                {
                    data = GenerateDataInputfieldList(itemPropertyKey, itemSchema, joData, isRequired);
                }
                break;
            }

            case HAGOServiceKey.PARAM_JSON_FORM_STRING:
            {
                if(itemSchema.EnumData != null && itemSchema.EnumData.Length > 0)
                {
                    switch(itemSchema.Format)
                    {
                        case HAGOServiceKey.PARAM_JSON_FORM_RADIOS:
                        case HAGOServiceKey.PARAM_JSON_FORM_RADIO_BUTTONS:
                        case HAGOServiceKey.PARAM_JSON_FORM_CHECKBOX:
                        {
                            data = GenerateDataEnumList(itemPropertyKey, itemSchema, joData, formType);
                            break;
                        }

                        default:
                        {
                            data = GenerateDataDropdown(itemPropertyKey, itemSchema, joData);
                            break;
                        }
                    }
                }
                else if(itemSchema.Advanced != null)
                {
                    string advanceType = itemSchema.Advanced.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_TYPE);
                    
                    switch(advanceType)
                    {
                        case HAGOServiceKey.PARAM_JSON_FORM_SELECT:
                        {
                            data = GenerateDataSelect(itemPropertyKey, itemSchema, joData);
                            break;
                        }
                        
                        case HAGOServiceKey.PARAM_JSON_FORM_TEXT_AREA:
                        {
                            data = GenerateDataInputfield(itemPropertyKey, itemSchema, joData, isRequired);
                            break;
                        }
                        
                        case HAGOServiceKey.PARAM_JSON_FORM_HELP:
                        {
                            data = GenerateDataHelp(itemPropertyKey, itemSchema, joData);
                            break;
                        }

                        case HAGOServiceKey.PARAM_JSON_FORM_DATETIME_PICKER:
                        {
                            string optionsFormat = (string)itemSchema.Advanced.SelectToken(string.Format("{0}.{1}", HAGOServiceKey.PARAM_JSON_FORM_OPTIONS,HAGOServiceKey.PARAM_JSON_FORM_FORMAT));
                            if(!string.IsNullOrEmpty(optionsFormat))
                            {
                                switch(optionsFormat)
                                {
                                    case HAGOServiceKey.PARAM_JSON_FORM_FORMAT_DATEPICKER:
                                    case HAGOServiceKey.PARAM_JSON_FORM_FORMAT_TIMEPICKER:
                                    case HAGOServiceKey.PARAM_JSON_FORM_DATETIME_PICKER:
                                    {
                                        data = GenerateDataDateTimePicker(itemPropertyKey, itemSchema, optionsFormat, joData);
                                        break;
                                    }

                                    default:
                                        break;
                                }
                            }
                            break;   
                        }

                        default:
                        {
                            data = GenerateDataInputfield(itemPropertyKey, itemSchema, joData, isRequired);
                            break;
                        }
                    } 
                }
                else
                {
                    data = GenerateDataInputfield(itemPropertyKey, itemSchema, joData, isRequired);
                }
                break;
            }

            case HAGOServiceKey.PARAM_JSON_FORM_INTEGER:
            case HAGOServiceKey.PARAM_JSON_FORM_NUMBER:
            {
                if(itemSchema.Advanced != null)
                {
                    string advanceType = itemSchema.Advanced.Value<string>(HAGOServiceKey.PARAM_JSON_FORM_TYPE);

                    switch(advanceType)
                    {
                        case HAGOServiceKey.PARAM_JSON_FORM_RANGE:
                        {
                            data = GenerateSliderDataEnumList(itemPropertyKey, itemSchema, joData, formType);
                            break;
                        }

                        default:
                        {
                            data = GenerateDataInputfield(itemPropertyKey, itemSchema, joData, isRequired);
                            break;
                        }
                    }
                }
                else
                {
                    data = GenerateDataInputfield(itemPropertyKey, itemSchema, joData, isRequired);
                }
                break;
            }

            case HAGOServiceKey.PARAM_JSON_FORM_BOOLEAN:
            {
                data = GenerateDataToggle(itemPropertyKey, itemSchema, joData, isRequired);
                break;
            }

            default:
                break;
        }

        if(data != null)
        {
            goItem.GetComponent<HAGOUIIComponent>().Init(data, !isDisable);
        }
    }

    #endregion


    #region ECLIPSE_JSONFORMS

    //NOTE: this method parse ui schema using Eclipse/JsonForms. Not using this now. Can reuse later if need
    // private void GenerateJSONFormView(JObject joJsonSchema, JObject joUISchema, JObject joJsonData, Transform content)
    // {
    //     if(joUISchema.ContainsKey(HAGOServiceKey.PARAM_TYPE))
    //     {
    //         string uiSchemaType = joUISchema.Value<string>(HAGOServiceKey.PARAM_TYPE);
    //         string uiSchemaLabel = joUISchema.Value<string>(HAGOServiceKey.PARAM_LABEL);
    //         JObject uiSchemaOptions = joUISchema.SafeValue<JObject>(HAGOServiceKey.PARAM_OPTIONS);
    //         JObject uiSchemaRule = joUISchema.SafeValue<JObject>(HAGOServiceKey.PARAM_RULE);
    //         string uiSchemaRuleEffect = uiSchemaRule != null ? uiSchemaRule.Value<string>(HAGOServiceKey.PARAM_EFFECT) : string.Empty;
    //         bool isDisable = !string.IsNullOrEmpty(uiSchemaRuleEffect) ? uiSchemaRuleEffect == HAGOServiceKey.PARAM_DISABLE : false;

    //         //Debug.Log($"uiSchemaRuleEffect : {uiSchemaRuleEffect}");

    //         GameObject prefabItem = null;
    //         GameObject goItem = null;
    //         //
    //         switch(uiSchemaType)
    //         {
    //             case HAGOServiceKey.PARAM_JSONFORMS_UISCHEMA_HORIZONTAL_LAYOUT:
    //             case HAGOServiceKey.PARAM_JSONFORMS_UISCHEMA_VERTICAL_LAYOUT:
    //             {
    //                 //NOTE: force layout to vertical on mobile due to lack of space
    //                 prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_VERTICAL_LAYOUT);
                    
    //                 GameObject go = CreateItem(prefabItem, content);
    //                 go.name = uiSchemaType;

    //                 //generate child elements
    //                 JArray jaElements = joUISchema.SafeValue<JArray>(HAGOServiceKey.PARAM_JSONFORMS_ELEMENTS);
    //                 foreach(JObject joElement in jaElements)
    //                 {
    //                     GenerateJSONFormView(joJsonSchema, joElement, joJsonData, go.transform);
    //                 }
    //                 break;
    //             }

    //             case HAGOServiceKey.PARAM_JSONFORMS_UISCHEMA_CONTROL:
    //             {
    //                 // //Debug.Log($"============ Init PARAM_JSONFORMS_UISCHEMA_CONTROL");

    //                 string scopePath = joUISchema.Value<string>(HAGOServiceKey.PARAM_JSONFORMS_SCOPE);
    //                 scopePath = scopePath.Substring(("#/").Length).Replace("/", ".");
    //                 // //Debug.Log($"============ scopePath {scopePath}");

    //                 string scopeKey = GetScopeKey(scopePath);
    //                 JObject joProperty = (JObject)joJsonSchema.SelectToken(scopePath);
    //                 // //Debug.Log($"============ joProperty {scopeKey} : {joProperty}");
                    
    //                 HAGOUIJsonSchemaDTO jsonSchemaElementData = joProperty.ToObject<HAGOUIJsonSchemaDTO>();

    //                 //create item
    //                 bool isCacheCompCheckValid = false;
    //                 //
    //                 switch(jsonSchemaElementData.Type ?? string.Empty)
    //                 {
    //                     case HAGOServiceKey.PARAM_JSONFORMS_STRING:
    //                     {
    //                         if(jsonSchemaElementData.EnumData != null && jsonSchemaElementData.EnumData.Length > 0)
    //                         {
    //                             switch(jsonSchemaElementData.Format ?? string.Empty)
    //                             {
    //                                 case HAGOServiceKey.PARAM_JSONFORMS_RADIO:
    //                                     prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_TOGGLE_LIST_ITEM);
    //                                     break;

    //                                 case HAGOServiceKey.PARAM_JSONFORMS_CHECKBOX:
    //                                     prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_CHECK_LIST_ITEM);
    //                                     break;
                                    
    //                                 default:
    //                                     prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_DROPDOWN_ITEM);
    //                                     break;
    //                             }
    //                         }
    //                         else if(!string.IsNullOrEmpty(jsonSchemaElementData.Format))
    //                         {
    //                             switch(jsonSchemaElementData.Format)
    //                             {
    //                                 case HAGOServiceKey.PARAM_JSONFORMS_DATE:
    //                                     prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_DATE_ITEM);
    //                                     break;

    //                                 case HAGOServiceKey.PARAM_JSONFORMS_TIME:
    //                                     prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_TIME_ITEM);
    //                                     break;

    //                                 case HAGOServiceKey.PARAM_JSONFORMS_DATETIME:
    //                                     prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_DATETIME_ITEM);
    //                                     break;

    //                                 default:
    //                                     break;
    //                             }
    //                         }
    //                         else if(uiSchemaOptions != null && uiSchemaOptions.ContainsKey(HAGOServiceKey.PARAM_MULTI) && uiSchemaOptions.Value<bool>(HAGOServiceKey.PARAM_MULTI))
    //                         {
    //                             prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_INPUTFIELD_MULTILINE_ITEM);
    //                         }
    //                         else 
    //                         {
    //                             prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_INPUTFIELD_ITEM);
    //                             isCacheCompCheckValid = true;
    //                         }
    //                         break;
    //                     }

    //                     case HAGOServiceKey.PARAM_JSONFORMS_INTEGER:
    //                     case HAGOServiceKey.PARAM_JSONFORMS_NUMBER:
    //                     {
    //                         prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_INPUTFIELD_ITEM);
    //                         isCacheCompCheckValid = true;
    //                         break;
    //                     }
                        
    //                     case HAGOServiceKey.PARAM_JSONFORMS_BOOLEAN:
    //                     {
    //                         prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_TOGGLE_ITEM);
    //                         break;
    //                     }

    //                     default:
    //                         break;
    //                 }

    //                 //create item
    //                 goItem = CreateItem(prefabItem, content);

    //                 //handle data
    //                 InitControlComponent(scopePath, scopeKey, uiSchemaLabel, uiSchemaOptions, jsonSchemaElementData, joJsonData, goItem, isDisable);
                    
    //                 //cache to check valid later
    //                 if(isCacheCompCheckValid)
    //                 {
    //                     m_dictComponentCheckValid.Add(scopePath, goItem.GetComponent<HAGOUIInputFieldComponentView>());
    //                 }
    //                 break;
    //             }
                    
    //             case HAGOServiceKey.PARAM_JSONFORMS_UISCHEMA_LABEL:
    //             {
    //                 string text = joUISchema.Value<string>(HAGOServiceKey.PARAM_JSONFORMS_TEXT);
    //                 prefabItem = GetPrefComponent(HAGOConstant.PREFAB_COMPONENT_LABEL);

    //                 //create item
    //                 GameObject goText = CreateItem(prefabItem, content);

    //                 //update view
    //                 goText.name = uiSchemaType;
    //                 goText.GetComponent<Text>().text = GetLangKey(text);
    //                 break;
    //             }

    //             default:
    //                 break;
    //         }
    //     }
    //     else
    //     {
    //         //Debug.Log("Error type");
    //     }
    // }

    // private string GetScopeKey(string scopePath)
    // {
    //     return scopePath.Split('.').LastOrDefault();
    // }
    
    //using for init json data with uiSchema "Control" type
    // private void InitControlComponent(string scopePath, string scopeKey, string uiSchemaLabel, JObject uiSchemaOptions, HAGOUIJsonFormJsonSchemaDTO jsonSchemaData, JObject jsonData,  GameObject goItem, bool isDisable = false)
    // {
    //     object data = null;

    //     switch(jsonSchemaData.Type ?? string.Empty)
    //     {
    //         case HAGOServiceKey.PARAM_JSONFORMS_STRING:
    //         {
    //             if(jsonSchemaData.EnumData != null && jsonSchemaData.EnumData.Length > 0)
    //             {
    //                 switch(jsonSchemaData.Format ?? string.Empty)
    //                 {
    //                     case HAGOServiceKey.PARAM_JSONFORMS_RADIO:
    //                     case HAGOServiceKey.PARAM_JSONFORMS_CHECKBOX:
    //                     {
    //                         data = GenerateDataEnumList(scopePath, scopeKey, jsonSchemaData, jsonData, jsonSchemaData.Format == HAGOServiceKey.PARAM_JSONFORMS_CHECKBOX);
    //                         break;
    //                     }

    //                     default:
    //                     {
    //                         data = GenerateDataDropdown(scopePath, scopeKey, jsonSchemaData, jsonData);
    //                         break;
    //                     }
    //                 }
    //             }
    //             else if(!string.IsNullOrEmpty(jsonSchemaData.Format))
    //             {
    //                 switch(jsonSchemaData.Format)
    //                 {
    //                     case HAGOServiceKey.PARAM_JSONFORMS_DATE:
    //                     case HAGOServiceKey.PARAM_JSONFORMS_TIME:
    //                     case HAGOServiceKey.PARAM_JSONFORMS_DATETIME:
    //                     {
    //                         data = GenerateDataDateTimePicker(scopePath, scopeKey, jsonSchemaData, jsonData);
    //                         break;
    //                     }

    //                     default:
    //                         break;
    //                 }
    //             }
    //             else
    //             {
    //                 data = GenerateDataInputfield(scopePath, scopeKey, uiSchemaLabel, jsonSchemaData, jsonData);
    //             }
    //             break;
    //         }

    //         case HAGOServiceKey.PARAM_JSONFORMS_INTEGER:
    //         case HAGOServiceKey.PARAM_JSONFORMS_NUMBER:
    //         {
    //             data = GenerateDataInputfield(scopePath, scopeKey, uiSchemaLabel, jsonSchemaData, jsonData);
    //             break;
    //         }
            
    //         case HAGOServiceKey.PARAM_JSONFORMS_BOOLEAN:
    //         {
    //             data = GenerateDataToggle(scopePath, scopeKey, jsonSchemaData, jsonData);
    //             break;
    //         }

    //         default:
    //             break;
    //     }

    //     if(data != null)
    //     {
    //         goItem.GetComponent<HAGOUIIComponent>().Init(data, !isDisable);
    //     }
    // }

    // private T InitJsonFormsDataValue<T>(string scopePath, JObject jsonData)
    // {
    //     try
    //     {
    //         // //Debug.Log($"======== GetDataValue: {scopePath} - jsonData: {jsonData}");

    //         scopePath = scopePath.Replace(HAGOServiceKey.PARAM_JSON_PROPERTIES + ".", string.Empty);
    //         if(jsonData == null || !jsonData.HasValues)
    //         {
    //             return default(T);
    //         }
    //         else
    //         {
    //             return jsonData.SelectToken(scopePath).ToObject<T>();
    //         }
    //     }
    //     catch(Exception ex)
    //     {
    //         //Debug.Log("======== GetDataValue Failed: " + ex);
    //         return default(T);
    //     }
    // }

    // private HAGOUIDropdownDTO GenerateDataDropdown(string scopePath, string scopeKey, HAGOUIJsonSchemaDTO jsonSchemaData, JObject jsonData)
    // {
    //     string defaultValueKey = InitDataValue<string>(scopePath, jsonData) ?? string.Empty;
    //     int defaultValueIndex = 0;

    //     List<HAGOUIDropdownOptionDTO> options = new List<HAGOUIDropdownOptionDTO>();
    //     //
    //     for(int i = 0; i < jsonSchemaData.EnumData.Length; i++)
    //     {
    //         HAGOUIDropdownOptionDTO optionDTO = new HAGOUIDropdownOptionDTO(
    //             id: jsonSchemaData.EnumData[i],
    //             name: GetLangKey(jsonSchemaData.EnumData[i]),
    //             icon: string.Empty
    //         );

    //         options.Add(optionDTO);

    //         if(jsonSchemaData.EnumData[i] == defaultValueKey)
    //         {
    //             defaultValueIndex = i;
    //         }
    //     }
        
    //     return new HAGOUIDropdownDTO(
    //         id: scopeKey,
    //         title: GetLangKey(scopeKey),
    //         keyForm: scopeKey,
    //         options: options,
    //         defaultValue: defaultValueIndex
    //     );
    // }

    // private HAGOUIDateTimeComponentDTO GenerateDataDateTimePicker(string scopePath, string scopeKey, HAGOUIJsonSchemaDTO jsonSchemaData, JObject jsonData)
    // {
    //     DateTime defaulValue = InitDataValue<DateTime>(scopePath, jsonData);

    //     return new HAGOUIDateTimeComponentDTO(
    //         id: scopeKey,
    //         title: GetLangKey(scopeKey),
    //         keyForm: scopeKey,
    //         value: defaulValue == default(DateTime) ? DateTime.Now : defaulValue
    //     );
    // }

    // private HAGOUIInputFieldDTO GenerateDataInputfield(string scopePath, string scopeKey, string title, HAGOUIJsonSchemaDTO jsonSchemaData, JObject jsonData)
    // {
    //     HAGOUIInputFieldContentType contentType = jsonSchemaData.Type == HAGOServiceKey.PARAM_JSON_FORM_INTEGER ? HAGOUIInputFieldContentType.Integer :
    //                         jsonSchemaData.Type == HAGOServiceKey.PARAM_JSON_FORM_NUMBER ? HAGOUIInputFieldContentType.Decimal :
    //                                                                                         HAGOUIInputFieldContentType.Standard;

    //     string content = jsonSchemaData.Type == HAGOServiceKey.PARAM_JSON_FORM_INTEGER ? InitDataValue<int?>(scopePath, jsonData)?.ToString() ?? string.Empty:
    //                         jsonSchemaData.Type == HAGOServiceKey.PARAM_JSON_FORM_NUMBER ? InitDataValue<float?>(scopePath, jsonData)?.ToString() ?? string.Empty:
    //                                                                                         InitDataValue<string>(scopePath, jsonData) ?? string.Empty;

    //     return new HAGOUIInputFieldDTO(
    //         id: scopeKey,
    //         title: !string.IsNullOrEmpty(title) ? GetLangKey(title, true) : GetLangKey(scopeKey),
    //         placeholder: GetLangKey(jsonSchemaData.Description, true) ?? string.Empty,
    //         content: content,
    //         unit: string.Empty,     //TODO: handle unit later
    //         contentType: contentType,
    //         keyForm: scopeKey,
    //         minValue: jsonSchemaData.Minimum ?? -1,
    //         maxValue: jsonSchemaData.Maximum ?? -1,
    //         minLength: jsonSchemaData.MinLength ?? -1,
    //         maxLength: jsonSchemaData.MaxLength ?? -1
    //     );
    // }

    // private HAGOUIToggleOptionDTO GenerateDataToggle(string scopePath, string scopeKey, HAGOUIJsonSchemaDTO jsonSchemaData, JObject jsonData)
    // {
    //     bool defaultValue = InitDataValue<bool>(scopePath, jsonData);

    //     return new HAGOUIToggleOptionDTO(
    //         id: scopeKey,
    //         title: GetLangKey(scopeKey),
    //         keyForm: scopeKey,
    //         defaultValue: defaultValue
    //     );
    // }

    // private HAGOUIToggleListDTO GenerateDataEnumList(string scopePath, string scopeKey, HAGOUIJsonSchemaDTO jsonSchemaData, JObject jsonData, bool isCheckbox)
    // {
    //     List<string> defaultValue = InitDataValue<List<string>>(scopePath, jsonData) ?? new List<string>();

    //     List<HAGOUIToggleOptionDTO> options = new List<HAGOUIToggleOptionDTO>();
    //     //
    //     for(int i = 0; i < jsonSchemaData.EnumData.Length; i++)
    //     {
    //         HAGOUIToggleOptionDTO optionDTO = new HAGOUIToggleOptionDTO(
    //             id: jsonSchemaData.EnumData[i],
    //             title: GetLangKey(jsonSchemaData.EnumData[i]),
    //             keyForm: jsonSchemaData.EnumData[i],
    //             defaultValue: defaultValue.Count > 0 ? defaultValue.Contains(jsonSchemaData.EnumData[i]) : i == 0
    //         );

    //         options.Add(optionDTO);
    //     }

    //     return new HAGOUIToggleListDTO(
    //         id: scopeKey,
    //         title: GetLangKey(scopeKey),
    //         keyForm: scopeKey,
    //         options: options
    //     );
    // }

    #endregion

    private T InitDataValue<T>(string propertyKey, JObject joData, string defaultValue)
    {
        try
        {
            if(joData == null || !joData.HasValues)
            {
                //Debug.Log($"InitDataValue [{propertyKey}] defaultValue: {defaultValue}");

                if(!string.IsNullOrEmpty(defaultValue))
                {
                    try
                    {
                        TypeConverter tc = TypeDescriptor.GetConverter(typeof(T)); 
                        return (T)tc.ConvertFrom(defaultValue);
                    }
                    catch(Exception ex)
                    {
                        //Debug.Log($"InitDataValue failed, cannot Parse {defaultValue} with type {typeof(T).ToString()}: {ex.ToString()}");
                        return default(T);
                    }
                }
                else
                {
                    return default(T);
                }
            }
            else
            {
                return joData.SelectToken(propertyKey).ToObject<T>();
            }
        }
        catch(Exception ex)
        {
            // //Debug.Log("======== InitDataValue Failed: " + ex);
            return default(T);
        }
    }

    private HAGOUIDropdownDTO GenerateDataSelect(string itemPropertyKey, HAGOUIJsonSchemaDTO itemSchema, JObject joData)
    {
        //Debug.Log($"{itemPropertyKey} GenerateDataDropdown: { JsonConvert.SerializeObject(joData) }");
        
        List<HAGOUIDropdownOptionDTO> options = new List<HAGOUIDropdownOptionDTO>();
        int defaultValueIndex = 0;
        //
        try
        {
            string defaultValue = joData.Value<string>(itemPropertyKey) ?? string.Empty;

            int index = 0;

            foreach(JToken jt in itemSchema.Advanced.Value<JArray>(HAGOServiceKey.PARAM_TITLEMAP))
            {
                JObject joItem = (JObject)jt;

                HAGOUIDropdownOptionDTO optionDTO = new HAGOUIDropdownOptionDTO(
                    id: joItem.Value<string>(HAGOServiceKey.PARAM_VALUE),
                    name: joItem.Value<string>(HAGOServiceKey.PARAM_NAME),
                    icon: string.Empty
                );

                options.Add(optionDTO);

                if(optionDTO.ID == defaultValue)
                {
                    defaultValueIndex = index;
                }

                index++;
            }
        }
        catch(Exception ex)
        {
            return null;
        }
        
        return new HAGOUIDropdownDTO(
            id: itemPropertyKey,
            title: GetLangKey(itemSchema.Title),
            keyForm: itemPropertyKey,
            options: options,
            defaultValue: defaultValueIndex
        );
    }

    private HAGOUIToggleListDTO GenerateDataMultipleSelect(string itemPropertyKey, HAGOUIJsonSchemaDTO itemSchema, JObject joData)
    {
        //Debug.Log($"{itemPropertyKey} GenerateDataMultipleSelect");
        
        List<HAGOUIToggleOptionDTO> options = new List<HAGOUIToggleOptionDTO>();
        //
        try
        {
            foreach(JToken jt in itemSchema.Advanced.Value<JArray>(HAGOServiceKey.PARAM_TITLEMAP))
            {
                JObject joItem = (JObject)jt;

                HAGOUIToggleOptionDTO optionDTO = new HAGOUIToggleOptionDTO(
                    id: joItem.Value<string>(HAGOServiceKey.PARAM_VALUE),
                    title: joItem.Value<string>(HAGOServiceKey.PARAM_NAME),
                    keyForm: joItem.Value<string>(HAGOServiceKey.PARAM_VALUE),
                    defaultValue: false
                );

                options.Add(optionDTO);
            }
        }
        catch(Exception ex)
        {
            return null;
        }
        
        return new HAGOUIToggleListDTO(
            id: itemPropertyKey,
            title: GetLangKey(itemSchema.Title),
            keyForm: itemPropertyKey,
            options: options,
            isSelectMultiple: true
        );
    }
    
    private HAGOUIDropdownDTO GenerateDataDropdown(string itemPropertyKey, HAGOUIJsonSchemaDTO itemSchema, JObject joData)
    {
        //Debug.Log($"{itemPropertyKey} GenerateDataDropdown");
        
        string defaultValueKey = InitDataValue<string>(itemPropertyKey, joData, itemSchema.Default);
        int defaultValueIndex = 0;

        List<HAGOUIDropdownOptionDTO> options = new List<HAGOUIDropdownOptionDTO>();
        //
        for(int i = 0; i < itemSchema.EnumData.Length; i++)
        {
            HAGOUIDropdownOptionDTO optionDTO = new HAGOUIDropdownOptionDTO(
                id: itemSchema.EnumData[i],
                name: GetLangKey(itemSchema.EnumData[i]),
                icon: string.Empty
            );

            options.Add(optionDTO);

            if(itemSchema.EnumData[i] == defaultValueKey)
            {
                defaultValueIndex = i;
            }
        }
        
        return new HAGOUIDropdownDTO(
            id: itemPropertyKey,
            title: GetLangKey(itemSchema.Title),
            keyForm: itemPropertyKey,
            options: options,
            defaultValue: defaultValueIndex
        );
    }

    private HAGOUIDateTimeComponentDTO GenerateDataDateTimePicker(string itemPropertyKey, HAGOUIJsonSchemaDTO itemSchema, string optionsFormat, JObject joData)
    {
        DateTime defaulValue = InitDataValue<DateTime>(itemPropertyKey, joData, itemSchema.Default);
        //Debug.Log($"<color=blue>GenerateDataDateTimePicker Format {optionsFormat}</color>");

        return new HAGOUIDateTimeComponentDTO(
            id: itemPropertyKey,
            title: GetLangKey(itemSchema.Title),
            keyForm: itemPropertyKey,
            value: optionsFormat == HAGOServiceKey.PARAM_JSON_FORM_FORMAT_TIMEPICKER ? DateTime.MinValue : (defaulValue == default(DateTime) ? DateTime.Now : defaulValue),
            isDurationPicker: optionsFormat == HAGOServiceKey.PARAM_JSON_FORM_FORMAT_TIMEPICKER
        );
    }

    private HAGOUIInputFieldDTO GenerateDataInputfield(string propertyKey, HAGOUIJsonSchemaDTO itemSchema, JObject joData, bool isRequired)
    {
        HAGOUIInputFieldContentType contentType = itemSchema.Type == HAGOServiceKey.PARAM_JSON_FORM_INTEGER ? HAGOUIInputFieldContentType.Integer :
                                                    itemSchema.Type == HAGOServiceKey.PARAM_JSON_FORM_NUMBER ? HAGOUIInputFieldContentType.Decimal :
                                                    HAGOUIInputFieldContentType.Standard;

        string placholder = (itemSchema.Advanced?.ContainsKey(HAGOServiceKey.PARAM_PLACEHOLDER) ?? false) ? itemSchema.Advanced.Value<string>(HAGOServiceKey.PARAM_PLACEHOLDER) : itemSchema.Description;
        string content = string.Empty;

        switch(content)
        {
            case HAGOServiceKey.PARAM_JSON_FORM_INTEGER:
                content = InitDataValue<int?>(propertyKey, joData, itemSchema.Default).ToString();
                break;

            case HAGOServiceKey.PARAM_JSON_FORM_NUMBER:
                content = InitDataValue<float?>(propertyKey, joData, itemSchema.Default).ToString();
                break;

            default:
                content = InitDataValue<string>(propertyKey, joData, itemSchema.Default);
                break;
        }

        return new HAGOUIInputFieldDTO(
            id: propertyKey,
            title: GetLangKey(itemSchema.Title),
            placeholder: GetLangKey(placholder, true),
            content: content,
            unit: string.Empty,     //TODO: handle unit later
            contentType: contentType,
            keyForm: propertyKey,
            minValue: itemSchema.Minimum ?? -1,
            maxValue: itemSchema.Maximum ?? -1,
            minLength: itemSchema.MinLength ?? -1,
            maxLength: itemSchema.MaxLength ?? -1,
            isRequired: isRequired
        );
    }

    private HAGOUIInputFieldListDTO GenerateDataInputfieldList(string propertyKey, HAGOUIJsonSchemaDTO itemSchema, JObject joData, bool isRequired)
    {
        string itemsType = itemSchema.Items.Value<string>(HAGOServiceKey.PARAM_TYPE);
        HAGOUIInputFieldContentType contentType = itemsType == HAGOServiceKey.PARAM_JSON_FORM_INTEGER ? HAGOUIInputFieldContentType.Integer :
                                                    itemsType == HAGOServiceKey.PARAM_JSON_FORM_NUMBER ? HAGOUIInputFieldContentType.Decimal :
                                                    HAGOUIInputFieldContentType.Standard;

        List<string> content = InitDataValue<List<string>>(propertyKey, joData, itemSchema.Default) ?? new List<string>();

        return new HAGOUIInputFieldListDTO(
            id: propertyKey,
            title: GetLangKey(itemSchema.Title),
            placeholder: GetLangKey(itemSchema.Description, true) ?? string.Empty,
            content: content,
            unit: string.Empty,     //TODO: handle unit later
            contentType: contentType,
            keyForm: propertyKey,
            minValue: itemSchema.Minimum ?? -1,
            maxValue: itemSchema.Maximum ?? -1,
            minLength: itemSchema.MinLength ?? -1,
            maxLength: itemSchema.MaxLength ?? -1,
            isRequired: isRequired
        );
    }

    private HAGOUITextDTO GenerateDataHelp(string propertyKey, HAGOUIJsonSchemaDTO itemSchema, JObject joData)
    {
        string content = InitDataValue<string>(propertyKey, joData, itemSchema.Default);

        //try set default value
        if(string.IsNullOrEmpty(content))
        {
            content = itemSchema.Default ?? string.Empty;
        }

        return new HAGOUITextDTO(
            id: propertyKey,
            title: GetLangKey(itemSchema.Title),
            content: content,
            keyForm: propertyKey
        );
    }

    private HAGOUIToggleOptionDTO GenerateDataToggle(string propertyKey, HAGOUIJsonSchemaDTO itemSchema, JObject joData, bool isRequired)
    {
        bool defaultValue = InitDataValue<bool>(propertyKey, joData, itemSchema.Default);

        return new HAGOUIToggleOptionDTO(
            id: propertyKey,
            title: GetLangKey(itemSchema.Title),
            keyForm: propertyKey,
            defaultValue: defaultValue,
            isRequired: isRequired
        );
    }

    private HAGOUIToggleListDTO GenerateSliderDataEnumList(string propertyKey, HAGOUIJsonSchemaDTO itemSchema, JObject joData, string formType = HAGOServiceKey.PARAM_JSON_FORM_RADIOS)
    {
        JObject joOption = itemSchema.Advanced.Value<JObject>(HAGOServiceKey.PARAM_OPTIONS);
        int floor = joOption.Value<int>(HAGOServiceKey.PARAM_FLOOR);
        int ceil = joOption.Value<int>(HAGOServiceKey.PARAM_CEIL);
        int step = joOption.Value<int>(HAGOServiceKey.PARAM_STEP);

        int defaultNumberValue = floor;
        if(!int.TryParse(itemSchema.Default, out defaultNumberValue))
        {
            defaultNumberValue = floor;
        }

        //Debug.Log($"{propertyKey} GenerateSliderDataEnumLis: {floor} - {ceil} : +{step}, default {defaultNumberValue}");

        List<HAGOUIToggleOptionDTO> options = new List<HAGOUIToggleOptionDTO>();
        // 
        for(int i = floor; i <= ceil; i += step)
        {
            HAGOUIToggleOptionDTO optionDTO = new HAGOUIToggleOptionDTO(
                id: i.ToString(),
                title: i.ToString(),
                keyForm: i.ToString(),
                defaultValue: defaultNumberValue == i
            );

            options.Add(optionDTO);
        }

        return new HAGOUIToggleListDTO(
            id: propertyKey,
            title: GetLangKey(itemSchema.Title),
            keyForm: propertyKey,
            options: options,
            formType: formType,
            itemSchema.Advanced
        );
    }

    private HAGOUIToggleListDTO GenerateDataEnumList(string propertyKey, HAGOUIJsonSchemaDTO itemSchema, JObject joData, string formType = HAGOServiceKey.PARAM_JSON_FORM_RADIOS)
    {
        //Debug.Log($"{propertyKey} GenerateDataEnumList");

        List<string> defaultValue = InitDataValue<List<string>>(propertyKey, joData, itemSchema.Default) ?? new List<string>();

        List<HAGOUIToggleOptionDTO> options = new List<HAGOUIToggleOptionDTO>();
        //
        for(int i = 0; i < itemSchema.EnumData.Length; i++)
        {
            HAGOUIToggleOptionDTO optionDTO = new HAGOUIToggleOptionDTO(
                id: itemSchema.EnumData[i],
                title: GetLangKey(itemSchema.EnumData[i]),
                keyForm: itemSchema.EnumData[i],
                defaultValue: defaultValue.Count > 0 ? defaultValue.Contains(itemSchema.EnumData[i]) : i == 0
            );

            options.Add(optionDTO);
        }

        return new HAGOUIToggleListDTO(
            id: propertyKey,
            title: GetLangKey(itemSchema.Title),
            keyForm: propertyKey,
            options: options,
            formType: formType
        );
    }

    private string GetLangKey(string scopeKey, bool ignoreConvertToTitleCase = false)
    {
        if(string.IsNullOrEmpty(scopeKey))
        {
            return string.Empty;
        }

        scopeKey = !HAGOUtils.IsLangKey(scopeKey) ?
                        (I18N.instance.HasKey("^" + scopeKey) ? "^" + scopeKey : scopeKey) : scopeKey;

        return I18N.instance.HasKey(scopeKey) ?
                    I18N.instance.getValue(scopeKey) :
                    (
                        ignoreConvertToTitleCase ?
                        scopeKey : 
                        (
                            //check snake case first before check camel case
                            (HAGOUtils.IsSnakeCase(scopeKey) ? HAGOUtils.ConvertSnakeCaseToTitleCase(scopeKey) :
                            (HAGOUtils.IsCamelCase(scopeKey) ? HAGOUtils.ConvertCamelCaseToTitleCase(scopeKey) : scopeKey))
                        )
                    );
    }

    private GameObject CreateItem(GameObject goPref, Transform content, string name)
    {
        GameObject go = Instantiate(goPref, content);
        go.name = name;

        return go;
    }

    private GameObject GetPrefComponent(string path)
    {
        //Debug.Log($"GetPrefComponent {path}");
        return Resources.Load<GameObject>(path);
    }

    public void AddCheckValid(params HAGOUIIComponent[] comps)
    {
        foreach(HAGOUIIComponent comp in comps)
        {
            m_dictComponentCheckValid.Add(comp.GetKeyForm(), comp);
        }
    }

    public void IsValid(Action<bool, Transform> callback)
    {
        foreach(KeyValuePair<string, HAGOUIIComponent> comp in m_dictComponentCheckValid)
        {
            //Debug.Log($"comp {comp.Key}");
            bool isValid = comp.Value.CheckValid();
            //Debug.Log($"=> isValid {isValid}");

            if (!isValid)
            {
                Transform tfItem = comp.Value.GetTransform().transform;
                HAGOSubmitFormControl.Api.ActiveError(m_data?.Id ?? -1, string.Empty, comp.Value.GetKeyForm(), tfItem);
                callback?.Invoke(false, tfItem);
                return;
            }
        }

        callback?.Invoke(true, null);
    }

    public void GetFormDataResult(Action<HAGOUIJsonFormDataResultDTO> callback)
    {
        IsValid((isValid, tfItemError) => {
            HAGOUIJsonFormDataResultDTO formResult = new HAGOUIJsonFormDataResultDTO(isValid, null);

            if(isValid)
            {
                Dictionary<string, JToken> data = new Dictionary<string, JToken>();

                foreach (HAGOUIIComponent formComp in transform.GetComponentsInChildren<HAGOUIIComponent>())
                {
                    JToken jtValue = formComp.GetJsonValue();
                    if(!formComp.IgnoreGetResult && jtValue != null)
                    {
                        data.Add(formComp.GetKeyForm(), jtValue);
                    }
                }

                formResult.Data = new HAGOUIJsonFormDataDTO(m_data?.Id ?? -1, data);
            }
            else
            {
                m_onErrorHandler?.Invoke(tfItemError as RectTransform);
            }
        
            //Debug.Log($"======= Get form data output: {JsonConvert.SerializeObject(formResult)}");
            callback?.Invoke(formResult);
        });
    }

    public JToken GetPropertyValue(string key)
    {
        foreach (HAGOUIIComponent formComp in transform.GetComponentsInChildren<HAGOUIIComponent>())
        {
            if(!formComp.IgnoreGetResult && formComp.GetKeyForm() == key)
            {
                return formComp.GetJsonValue();
            }
        }

        return null;
    }
    
    public void Clear()
    {
        foreach(KeyValuePair<string, HAGOUIIComponent> comp in m_dictComponentCheckValid)
        {
            comp.Value.Clear();
        }
    }

    public void SetValue(string keyForm, string content)
    {
        HAGOUIIComponent ihaComp = m_content.GetComponentsInChildren<HAGOUIIComponent>().Where(x => x.GetKeyForm().Equals(keyForm)).FirstOrDefault();
        if (ihaComp != null)
        {
            ihaComp.SetValue(content);
        }
    }
}
