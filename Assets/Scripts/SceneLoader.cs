using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace resUrbis.Utility
{
    public class SceneLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private SceneAsset _wantedScene;
#endif
        [SerializeField] private LoadSceneMode _loadMode;
        [SerializeField] private int _sceneIndex;
        [SerializeField] private bool _useAdditionalCheck;

        [Header("Events")]
        [SerializeField] private UnityEvent _onLoadDirectly;

        [SerializeField] private UnityEvent _onLoadBeginAsync;
        [SerializeField] private UnityEvent _onLoadEndAsync;

        [SerializeField] private UnityEvent _onUnloadBeginAsync;
        [SerializeField] private UnityEvent _onUnloadEndAsync;

        private static bool _isProcessing = false;

        public bool additionalCheck { get; set; }
        public UnityEvent OnLoadBeginAsync => _onLoadBeginAsync;
        public UnityEvent OnLoadEndAsync => _onLoadEndAsync;
        public UnityEvent OnUnloadBeginAsync => _onUnloadBeginAsync;
        public UnityEvent OnUnloadEndAsync => _onUnloadEndAsync;

        private void Awake()
        {
            _isProcessing = false;
        }

        public void LoadScene()
        {
            if (_isProcessing)
                return;

            if (_sceneIndex == -1)
                return;

            _isProcessing = true;

            _onLoadDirectly?.Invoke();
            SceneManager.LoadScene(_sceneIndex, _loadMode);
        }
        
        public void LoadScene(int index)
        {
            if (_isProcessing)
                return;

            _isProcessing = true;

            _sceneIndex = index;
            _onLoadDirectly?.Invoke();
            SceneManager.LoadScene(index, _loadMode);
        }

        public void LoadSceneAsync()
        {
            if (_isProcessing)
                return;

            if (_sceneIndex == -1)
                return;

            _isProcessing = true;
            StartCoroutine(AsyncLoading(false, _sceneIndex));
        }
        
        public void LoadSceneAsync(int index)
        {
            if (_isProcessing)
                return;

            _isProcessing = true;

            _sceneIndex = index;
            StartCoroutine(AsyncLoading(false, index));
        }

        public void UnloadSceneAsync()
        {
            if (_isProcessing)
                return;

            if (_sceneIndex == -1)
                return;

            _isProcessing = true;
            StartCoroutine(AsyncLoading(true, _sceneIndex));
        }
        
        public void UnloadSceneAsync(int index)
        {
            if (_isProcessing)
                return;

            if (_sceneIndex == -1)
                return;

            _isProcessing = true;

            _sceneIndex = index;
            StartCoroutine(AsyncLoading(false, _sceneIndex));
        }

        private IEnumerator AsyncLoading(bool isUnloading, int sceneIndex)
        {
            if(_useAdditionalCheck)
                yield return new WaitUntil(() => additionalCheck == true);
            
            if (!isUnloading)
            {
                _onLoadBeginAsync?.Invoke();
                yield return SceneManager.LoadSceneAsync(sceneIndex, _loadMode);
                _onLoadEndAsync?.Invoke();
            }
            else
            {
                _onUnloadBeginAsync?.Invoke();
                yield return SceneManager.UnloadSceneAsync(sceneIndex);
                _onUnloadEndAsync?.Invoke();
            }
        }

        public void SetSceneIndex(int sceneIndex)
        {
            _sceneIndex = sceneIndex;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (_wantedScene == null)
            {
                _sceneIndex = -1;
                return;
            }

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (_wantedScene == AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[i].path))
                {
                    _sceneIndex = i;
                    return;
                }
            }


            _sceneIndex = -1;

        }
#endif
    } 
}
