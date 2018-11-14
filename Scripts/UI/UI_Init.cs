using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public static class UI_Init
{
    // 初始化UI控件
    public static void Init(this UnityEngine.UI.Button button, UnityAction onClick, bool trigger = false)
    {
        if (onClick != null) button.onClick.AddListener(onClick);
        if (trigger) onClick();
    }
    public static void Init(this Toggle toggle, UnityAction<bool> onToggle, bool trigger = false)
    {
        if (onToggle != null) toggle.onValueChanged.AddListener(onToggle);
        if (trigger) onToggle(toggle.isOn);
    }
    // trigger 是否立即触发事件
    public static void Init(this Dropdown drop, int value, IList<string> options, UnityAction<int> onValueChanged, bool trigger = false)
    {
        Init(drop, value, new List<string>(options), onValueChanged);
        if (trigger) onValueChanged(value);
    }
    public static void Init(this Dropdown drop, int value, List<string> options, UnityAction<int> onValueChanged)
    {
        drop.ClearOptions();
        drop.AddOptions(options);
        drop.value = value;
        drop.onValueChanged.AddListener(onValueChanged); // add ofter set a default value, or it'll trigger the changed func
        drop.gameObject.AddComponent<DropDownLocateSelectedItem>();
    }
    public static void Init(this InputField input, int value, UnityAction<string> onValueChanged)
    {
        Init(input, value.ToString(), onValueChanged);
    }
    public static void Init(this InputField input, string text, UnityAction<string> onValueChanged, bool trigger = false)
    {
        input.text = text;
        input.onValueChanged.AddListener(onValueChanged);
        if (trigger) onValueChanged(text);
    }
    public static void Init(this InputField input, UnityAction<string> onValueChanged)
    {
        input.onValueChanged.AddListener(onValueChanged);
    }
    public static void Init(this Slider slider, UnityAction<float> onValueChanged)
    {
        slider.onValueChanged.AddListener(onValueChanged);
    }
    public static void Init(this Slider slider, Vector3 mdm, UnityAction<float> onValueChanged)
    {
        slider.minValue = mdm.x;
        slider.maxValue = mdm.z;
        slider.value = mdm.y;
        slider.onValueChanged.AddListener(onValueChanged);
    }
    public static void Init(this Slider slider, float min, float max, float defaultValue,
        UnityAction<float> onValueChanged)
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = defaultValue;
        slider.onValueChanged.AddListener(onValueChanged);
    }
    public static void SingleCheck(this MonoBehaviour mono, params Toggle[] toggles)
    {
        foreach (var tgl1 in toggles)
        {
            foreach (var tgl2 in toggles)
            {
                if (tgl1 != tgl2)
                {
                    tgl1.onValueChanged.AddListener(b => { if (b) tgl2.isOn = false; });
                }
            }
        }
    }
}
