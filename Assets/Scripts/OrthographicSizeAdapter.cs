using UnityEngine;
using UnityEngine.Events;

namespace resUrbis.UserInterface
{

    public enum AdgjustType
    {
        horizontal,

        vertical
    }
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(Camera))]
    public class OrthographicSizeAdapter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _backRenderer;
        [SerializeField] private AdgjustType _type;
        [SerializeField] private UnityEvent _onAdapted;

        private Camera _cam;

        public bool adapted { get; private set; }

        public SpriteRenderer backRenderer
        {
            get => _backRenderer;
            set
            {
                _backRenderer = value;
                Adapt();
            }
        }

        void Awake()
        {
            _cam ??= GetComponent<Camera>();

            if (!_backRenderer)
                return;

            Adapt();
        }

#if !UNITY_EDITOR
        private void Adapt()
        {
#else
        public void Adapt()
        {
            _cam = GetComponent<Camera>();
#endif
            adapted = false;

            if (_type == AdgjustType.horizontal)
                _cam.orthographicSize = _backRenderer.bounds.size.x * Screen.height / Screen.width * 0.5f;
            else if (_type == AdgjustType.vertical)
                _cam.orthographicSize = _backRenderer.bounds.size.y / 2;

            _onAdapted?.Invoke();

            adapted = true;
        }

        private void OnDestroy()
        {
            adapted = false;
        }
    }
}
