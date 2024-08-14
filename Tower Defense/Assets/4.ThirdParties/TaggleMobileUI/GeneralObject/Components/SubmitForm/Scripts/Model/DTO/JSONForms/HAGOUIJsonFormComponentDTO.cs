using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class HAGOUIJsonFormComponentDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string SchemaName { get; set; }
    public JObject Schema { get; set; } //json schema
    public JArray Definition { get; set; } //ui schema
    public JObject Data { get; set; } //form data
    
    public HAGOUIJsonFormComponentDTO()
    {
        Id = -1;
        Title = string.Empty;
        Definition = new JArray();
        Data = new JObject();
    }

    public HAGOUIJsonFormComponentDTO(long id, string title, string schemaName, JObject schema, JArray definition, JObject data)
    {
        Id = id;
        Title = title;
        SchemaName = schemaName;
        Schema = schema;
        Definition = definition;
        Data = data ?? new JObject();
    }

    public HAGOUIJsonFormComponentDTO(HAGOUIJsonFormComponentDTO form)
    {
        Id = form.Id;
        Title = form.Title;
        SchemaName = form.SchemaName;
        Schema = form.Schema;
        Definition = form.Definition;
        Data = form.Data ?? new JObject();
    }

    public bool IsEmpty()
    {
        return Schema == null || Definition == null || (!Schema.HasValues && !Definition.HasValues);
    }
}