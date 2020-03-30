using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SpaceMonkeys.Core;

namespace SpaceMonkeys.UI
{
    /// <summary>
    /// A script that you drop on a child object to the "window" GameObject you want to move around.
    /// </summary>
    /// <remarks>
    /// Find the GitHub code at https://github.com/WeirdBeardDev/Unity-2D-Examples.
    /// Find the specific file at https://github.com/WeirdBeardDev/Unity-2D-Examples/blob/master/Assets/_Game/Scripts/UI/DragWindow.cs.
    /// </remarks>
    public class DragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        #region Members
        [Header("References")]
        [SerializeField] private Image _contentImage = default;
        [SerializeField] [Range(0f, 1f)] private float _contentImageAlpha = .4f;

        [Header("Clamping Options")]
        [SerializeField] private bool _clampToScreen = true;
        [SerializeField] private bool _useOffScreenToClose = false;
        [SerializeField] private float _allowedOffset = 250f;

        private RectTransform _panelRT;
        private RectTransform _dragAreaRT;
        private Vector2 _originalLocalPointerPos;
        #endregion Members

        #region MonoBehaviours
        void Awake()
        {
            _panelRT = transform.parent as RectTransform;
            _dragAreaRT = gameObject.FindInParents<Canvas>(transform.parent.gameObject).gameObject.GetComponent<RectTransform>();
        }
        #endregion MonoBehaviours

        #region Interface Implementations
        public void OnPointerDown(PointerEventData data)
        {
            _panelRT.SetAsLastSibling();
            OriginalPosition = _panelRT.localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_dragAreaRT, data.position, data.pressEventCamera, out _originalLocalPointerPos);
            ChangeAlpha();
        }
        public void OnDrag(PointerEventData data)
        {
            if (_panelRT == null || _dragAreaRT == null) return;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_dragAreaRT, data.position, data.pressEventCamera, out Vector2 localPointerPos))
            {
                Vector3 offsetToOriginal = localPointerPos - _originalLocalPointerPos;
                _panelRT.localPosition = OriginalPosition + offsetToOriginal;
            }
            if (_clampToScreen)
                _panelRT.localPosition = _useOffScreenToClose ? ClampToScreen(_allowedOffset) : ClampToScreen();
        }
        public void OnPointerUp(PointerEventData data)
        {
            ChangeAlpha();
            if (!IsFullyOnScreen)
            {
                _panelRT.gameObject.SetActive(false);
                _panelRT.localPosition = OriginalPosition;
            }
        }
        #endregion Interface Implementations

        #region Properties
        public Vector3 OriginalPosition { get; private set; }
        public bool IsFullyOnScreen
        {
            get
            {
                bool ans = true;
                Vector3[] worldCorners = new Vector3[4];
                _panelRT.GetWorldCorners(worldCorners);

                ans &= !(worldCorners[0].x < 0);
                ans &= !(worldCorners[2].x > Screen.width);
                ans &= !(worldCorners[0].y < 0);
                ans &= !(worldCorners[2].y > Screen.height);

                return ans;
            }
        }

        #endregion Properties

        #region Helpers
        private void ChangeAlpha()
        {
            if (_contentImage != null)
            {
                Color bg = _contentImage.color;
                switch (bg.a)
                {
                    case 1f:
                        bg.a = _contentImageAlpha;
                        break;
                    default:
                        bg.a = 1f;
                        break;
                }
                _contentImage.color = bg;
            }
        }
        private Vector3 ClampToScreen() => ClampToScreen(0f);
        private Vector3 ClampToScreen(float offset)
        {
            Vector3 pos = _panelRT.localPosition;
            Vector3 minPos = _dragAreaRT.rect.min - _panelRT.rect.min;
            Vector3 maxPos = _dragAreaRT.rect.max - _panelRT.rect.max;

            pos.x = Mathf.Clamp(_panelRT.localPosition.x, minPos.x - offset, maxPos.x + offset);
            pos.y = Mathf.Clamp(_panelRT.localPosition.y, minPos.y - offset, maxPos.y + offset);

            return pos;
        }
        #endregion Helpers

    }
}