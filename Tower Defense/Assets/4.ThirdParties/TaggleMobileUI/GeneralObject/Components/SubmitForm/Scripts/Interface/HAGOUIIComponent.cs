using Newtonsoft.Json.Linq;
using UnityEngine;

public interface HAGOUIIComponent
{
	bool IgnoreGetResult { get; set; }
	
	void Init(object data, bool isEditMode);
	object ExportView(string id);
	string GetFormType();
	JToken GetJsonValue();
	string GetKeyForm();	//TODO: replace deprecated GetKeyForm into GetId (jsonforms id = scopeKey)
	void ActiveError();
	void ResetError();
	void SetValue(string value);
	bool CheckValid();
	Transform GetTransform();
	void Clear();	//clear input value (Ex: after submit,...)
}