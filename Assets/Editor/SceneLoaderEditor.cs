using resUrbis.Utility;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneLoader), true)]
[CanEditMultipleObjects]
public class SceneLoaderEditor : Editor
{
    private static SerializedProperty _wantedScene;
    private static SerializedProperty _sceneIndex;
    private static SerializedProperty _loadMode;
    private static SerializedProperty _useAdditionalCheck;

    private static SerializedProperty _onLoadDirectly;

    private static SerializedProperty _onLoadBeginAsync;
    private static SerializedProperty _onLoadEndAsync;

    private static SerializedProperty _onUnloadBeginAsync;
    private static SerializedProperty _onUnloadEndAsync;

    private static bool _loadEventsOpen = false;
    private static bool _unloadEventsOpen = false;

    private static string _sceneIndexText => _sceneIndex.intValue >= 0 ? _sceneIndex.intValue.ToString() : "-";

    private void OnEnable()
    {
        _sceneIndex = serializedObject.FindProperty(nameof(_sceneIndex));
        _wantedScene = serializedObject.FindProperty(nameof(_wantedScene));
        _loadMode = serializedObject.FindProperty(nameof(_loadMode));
        _useAdditionalCheck = serializedObject.FindProperty(nameof(_useAdditionalCheck));
        _onLoadDirectly = serializedObject.FindProperty(nameof(_onLoadDirectly));
        _onLoadBeginAsync = serializedObject.FindProperty(nameof(_onLoadBeginAsync));
        _onLoadEndAsync = serializedObject.FindProperty(nameof(_onLoadEndAsync));
        _onUnloadBeginAsync = serializedObject.FindProperty(nameof(_onUnloadBeginAsync));
        _onUnloadEndAsync = serializedObject.FindProperty(nameof(_onUnloadEndAsync));

        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel($"{_sceneIndex.displayName}");
            GUILayout.Label(_sceneIndexText, GUILayout.ExpandWidth(false));
            EditorGUILayout.PropertyField(_wantedScene, GUIContent.none);
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.PropertyField(_loadMode);
        EditorGUILayout.PropertyField(_onLoadDirectly);

        EditorGUILayout.PropertyField(_useAdditionalCheck);

        _loadEventsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_loadEventsOpen, "Loads Events Async");

        if (_loadEventsOpen)
        {
            EditorGUILayout.PropertyField(_onLoadBeginAsync);
            EditorGUILayout.PropertyField(_onLoadEndAsync);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        _unloadEventsOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_unloadEventsOpen, "Unloads Events Async");

        if (_unloadEventsOpen)
        {
            EditorGUILayout.PropertyField(_onUnloadBeginAsync);
            EditorGUILayout.PropertyField(_onUnloadEndAsync);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
    }
}
