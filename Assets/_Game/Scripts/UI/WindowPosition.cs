using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpaceMonkeys.UI;

public class WindowPosition : MonoBehaviour
{
    #region Members
    [SerializeField] private RectTransform _windowRT = default;
    [SerializeField] private CornerType _cornerType = default;
    private Text _text = default;
    private DragWindow _drag = default;
    #endregion Members

    #region MonoBehaviours
    void Start()
    {
        _text = GetComponent<Text>();
        _drag = _windowRT.GetComponentInChildren<DragWindow>();
    }
    void Update()
    {
        if (_windowRT != null && _text != null)
        {
            Vector3[] corners = new Vector3[4];
            if (_cornerType == CornerType.Local)
            {
                _windowRT.GetLocalCorners(corners);
            }
            else
            {
                _windowRT.GetWorldCorners(corners);
            }

            _text.text = $"Window {_cornerType.ToString()}\nUpper Left: ({corners[1]})\nLower Right: ({corners[3]}\nFully On Screen: {_drag.IsFullyOnScreen}";
        }
    }
    #endregion MonoBehaviours
}

public enum CornerType
{
    Local,
    World
}
