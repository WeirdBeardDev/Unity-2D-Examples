using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenText : MonoBehaviour
{
    #region Members
    private Text _text;
    #endregion Members

    #region MonoBehaviours
    void Start()
    {
        _text = GetComponent<Text>();
    }
    void Update()
    {
        if (_text != null)
            _text.text = $"Screen\nWidth: {Screen.width}\nHeight: {Screen.height}";
    }
    #endregion MonoBehaviours
}
