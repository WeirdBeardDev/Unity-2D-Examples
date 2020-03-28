using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SpaceMonkeys.Core;

namespace SpaceMonkeys.UI
{
    public class DragWindow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IInitializePotentialDragHandler
    {
        #region Members
        [Header("References")]
        [SerializeField] private Canvas _canvas = default;
        [SerializeField] private RectTransform _windowRectTransform = default;
        [SerializeField] private Image _contentImage = default;
        [SerializeField] [Range(0f, 1f)] private float _contentImageAlpha = .4f;

        [Header("Clamping Options")]
        [SerializeField] private bool _clampToScreen = true;
        [SerializeField] private bool _useOffScreenToClose = false;
        [SerializeField] private float _allowedOffset = 250f;
        #endregion Members

        #region MonoBehaviours
        void Awake()
        {
            if (_windowRectTransform == null)
            {
                _windowRectTransform = transform.parent.GetComponent<RectTransform>();
            }
            if (_canvas == null)
            {
                _canvas = gameObject.FindInParents<Canvas>(transform.parent.gameObject);
            }
        }
        #endregion MonoBehaviours

        #region Interface Implementations
        public void OnInitializePotentialDrag(PointerEventData eventData) => OriginalPosition = _windowRectTransform.position;
        public void OnBeginDrag(PointerEventData eventData) => ChangeAlpha(_contentImage, _contentImageAlpha);
        public void OnDrag(PointerEventData eventData)
        {
            if (_windowRectTransform is null || _canvas is null) return;
            _windowRectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
            if (_clampToScreen)
                _windowRectTransform.position = _useOffScreenToClose ? ClampToScreen(_allowedOffset) : ClampToScreen();
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            ChangeAlpha(_contentImage, 1f);
            if (!IsWindowFullyOnScreen())
            {
                DeactivateWindow();
                _windowRectTransform.position = OriginalPosition;
            }
        }
        public void OnPointerDown(PointerEventData eventData) => _windowRectTransform.SetAsLastSibling();
        #endregion Interface Implementations

        #region Properties
        public Vector3 OriginalPosition { get; private set; }
        #endregion Properties

        #region Helpers
        private void ChangeAlpha(Image background, float alpha)
        {
            if (background != null)
            {
                Color bg = background.color;
                bg.a = alpha;
                background.color = bg;
            }
        }
        private Vector3 ClampToScreen() => ClampToScreen(0f);
        private Vector3 ClampToScreen(float windowOffset)
        {
            Vector3 ans = new Vector3();
            var rt = _windowRectTransform;
            Vector2 scale = new Vector2(rt.lossyScale.x, rt.lossyScale.y);
            float adjHeight, adjWidth;
            adjWidth = (rt.rect.width * rt.anchorMax.x * scale.x) - (windowOffset * scale.x);
            adjHeight = (rt.rect.height * (1 - rt.anchorMax.y) * scale.y) - (windowOffset * scale.y);

            ans.x = Mathf.Clamp(rt.position.x, adjWidth + .01f, Screen.width - adjWidth);
            ans.y = Mathf.Clamp(rt.position.y, adjHeight + .01f, Screen.height - adjHeight);
            ans.z = rt.position.z;

            return ans;
        }
        private bool IsWindowFullyOnScreen()
        {
            bool ans = true;
            var rt = _windowRectTransform;
            Vector3[] worldCorners = new Vector3[4];
            rt.GetWorldCorners(worldCorners);

            ans &= !(worldCorners[0].x < 0);
            ans &= !(worldCorners[2].x > Screen.width);
            ans &= !(worldCorners[0].y < 0);
            ans &= !(worldCorners[2].y > Screen.height);

            return ans;
        }
        private void DeactivateWindow() => _windowRectTransform.gameObject.SetActive(false);
        private void PrintScreenStats(string prefix) => print($"{prefix} - Resolution: {Screen.currentResolution}");
        private void PrintWindowLocation(string prefix)
        {
            // Vector3[] localCorners = new Vector3[4];
            Vector3[] fourCorners = new Vector3[4];
            // _windowRectTransform.GetLocalCorners(localCorners);
            _windowRectTransform.GetWorldCorners(fourCorners);
            // print($"{prefix} - Local Window top left: ({localCorners[1].x},{localCorners[1].y})");
            print($"{prefix} - World Window top left: ({fourCorners[1].x}, {fourCorners[1].y})");
            print($"{prefix} - World Window bottom left: ({fourCorners[0].x}, {fourCorners[0].y})");
        }
        #endregion Helpers

    }
}