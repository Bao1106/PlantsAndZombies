using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOUIJsonSchemaDTO
{
    [JsonProperty("id")]
    public string Id;
    
    [JsonProperty("title")]
    public string Title;

    [JsonProperty("description")]
    public string Description;

    [JsonProperty("maximum")]
    public float? Maximum { get; set; }

    [JsonProperty("minimum")]
    public float? Minimum;

    [JsonProperty("maxLength")]
    public int? MaxLength;

    [JsonProperty("minLength")]
    public int? MinLength;

    [JsonProperty("required")]
    public string[] Required;
    
    [JsonProperty("enum")]
    public string[] EnumData;
   
    [JsonProperty("type")]
    public string Type;
   
    [JsonProperty("format")]
    public string Format;
    
    [JsonProperty("properties")]
    public JObject Properties;
    
    [JsonProperty("advanced")]
    public JObject Advanced;
    
    [JsonProperty("default")]
    public string Default;
    
    [JsonProperty("items")]
    public JObject Items;

    [JsonProperty("__extra_properties")]
    public JObject ExtraProperties;
}


#region ECLIPSE_JSONFORMS

// public class HAGOUIJsonFormJsonSchemaDTO
// {
//     /**
//     * This is important because it tells refs where
//     * the root of the document is located
//     */
//     [JsonProperty("id")]
//     public string Id;

//     /**
//     * It is recommended that the meta-schema is
//     * included in the root of any JSON Schema
//     */
//     // $schema?: string;
//     /**
//     * Title of the schema
//     */
//     // title?: string;
//     /**
//     * Schema description
//     */
//     [JsonProperty("description")]
//     public string Description;

//     /////////////////////////////////////////////////
//     // Number Validation
//     /////////////////////////////////////////////////
//     /**
//     * The value must be a multiple of the number
//     * (e.g. 10 is a multiple of 5)
//     */
//     // multipleOf?: number;
//     [JsonProperty("maximum")]
//     public float? Maximum { get; set; }
//     /**
//     * If true maximum must be > value, >= otherwise
//     */
//     [JsonProperty("exclusiveMaximum")]
//     public float? ExclusiveMaximum;
//     [JsonProperty("minimum")]
//     public float? Minimum;
//     /**
//     * If true minimum must be < value, <= otherwise
//     */
//     [JsonProperty("exclusiveMinimum")]
//     public float? ExclusiveMinimum;

//     /////////////////////////////////////////////////
//     // String Validation
//     /////////////////////////////////////////////////
//     [JsonProperty("mnaxLength")]
//     public int? MaxLength;
//     [JsonProperty("minLength")]
//     public int? MinLength;
//     /**
//     * This is a regex string that the value must
//     * conform to
//     */
//     // pattern?: string;

//     /////////////////////////////////////////////////
//     // Array Validation
//     /////////////////////////////////////////////////
//     // additionalItems?: boolean | JsonSchema7;
//     // items?: JsonSchema7 | JsonSchema7[];
//     // maxItems?: number;
//     // minItems?: number;
//     // uniqueItems?: boolean;

//     /////////////////////////////////////////////////
//     // Object Validation
//     /////////////////////////////////////////////////
//     // maxProperties?: number;
//     // minProperties?: number;
//     [JsonProperty("required")]
//     public string[] Required;
//     // additionalProperties?: boolean | JsonSchema7;
//     /**
//     * Holds simple JSON Schema definitions for
//     * referencing from elsewhere.
//     */
//     // definitions?: { [key: string]: JsonSchema7 };
//     /**
//     * The keys that can exist on the object with the
//     * json schema that should validate their value
//     */
//     [JsonProperty("properties")]
//     public JObject Properties;
//     /**
//     * The key of this object is a regex for which
//     * properties the schema applies to
//     */
//     // patternProperties?: { [pattern: string]: JsonSchema7 };
//     /**
//     * If the key is present as a property then the
//     * string of properties must also be present.
//     * If the value is a JSON Schema then it must
//     * also be valid for the object if the key is
//     * present.
//     */
//     // dependencies?: { [key: string]: JsonSchema7 | string[] };

//     /////////////////////////////////////////////////
//     // Generic
//     /////////////////////////////////////////////////
//     /**
//     * Enumerates the values that this schema can be
//     * e.g.
//     * {"type": "string",
//     *  "enum": ["red", "green", "blue"]}
//     */
//     [JsonProperty("enum")]
//     public string[] EnumData;
//     /**
//     * The basic type of this schema, can be one of
//     * [string, number, object, array, boolean, null]
//     * or an array of the acceptable types
//     */
//     [JsonProperty("type")]
//     public string Type;

//     /////////////////////////////////////////////////
//     // Combining Schemas
//     /////////////////////////////////////////////////
//     // allOf?: JsonSchema7[];
//     // anyOf?: JsonSchema7[];
//     // oneOf?: JsonSchema7[];
//     /**
//     * The entity being validated must not match this schema
//     */
//     // not?: JsonSchema7;
    
//     [JsonProperty("format")]
//     public string Format;
//     // readOnly?: boolean;
//     // writeOnly?: boolean;
//     // examples?: any[];
//     // contains?: JsonSchema7;
//     // propertyNames?: JsonSchema7;
//     // const?: any;
//     // if?: JsonSchema7;
//     // then?: JsonSchema7;
//     // else?: JsonSchema7;
//     // errorMessage?: any;
// }

#endregion