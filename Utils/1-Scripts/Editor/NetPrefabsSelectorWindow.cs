using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using UnityEditor;
using UnityEngine;

namespace AnkleBreaker.Utils.Fishnet.Editor
{
    /// <summary>
    /// EditorWindow-based replacement for the OdinSelector&lt;GameObject&gt;.
    /// Displays a searchable list of network prefabs for selection.
    /// </summary>
    public class NetPrefabsSelectorWindow : EditorWindow
    {
        private List<GameObject> _source;
        private bool _supportsMultiSelect;
        private Action<List<GameObject>> _onSelectionConfirmed;

        private string _searchFilter = "";
        private Vector2 _scrollPosition;
        private HashSet<int> _selectedIndices = new HashSet<int>();
        private List<GameObject> _filteredList = new List<GameObject>();

        public static NetPrefabsSelectorWindow Show(
            List<GameObject> source,
            bool supportsMultiSelect,
            Action<List<GameObject>> onSelectionConfirmed)
        {
            var window = GetWindow<NetPrefabsSelectorWindow>(true, "Select Network Prefab(s)");
            window._source = source ?? new List<GameObject>();
            window._supportsMultiSelect = supportsMultiSelect;
            window._onSelectionConfirmed = onSelectionConfirmed;
            window._selectedIndices.Clear();
            window._searchFilter = "";
            window.minSize = new Vector2(350, 400);
            window.ShowUtility();
            return window;
        }

        private void OnGUI()
        {
            if (_source == null)
            {
                EditorGUILayout.HelpBox("No source list provided.", MessageType.Warning);
                return;
            }

            // Search toolbar
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            _searchFilter = EditorGUILayout.TextField(_searchFilter, EditorStyles.toolbarSearchField);
            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSearchCancelButton") ?? EditorStyles.miniButton, GUILayout.Width(18)))
            {
                _searchFilter = "";
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();

            // Filter list
            _filteredList.Clear();
            for (int i = 0; i < _source.Count; i++)
            {
                if (_source[i] == null) continue;
                if (!string.IsNullOrEmpty(_searchFilter) &&
                    !_source[i].name.ToLowerInvariant().Contains(_searchFilter.ToLowerInvariant()))
                    continue;
                _filteredList.Add(_source[i]);
            }

            // Scrollable list
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            for (int i = 0; i < _filteredList.Count; i++)
            {
                var prefab = _filteredList[i];
                int sourceIndex = _source.IndexOf(prefab);
                bool isSelected = _selectedIndices.Contains(sourceIndex);

                EditorGUILayout.BeginHorizontal();

                // Selection toggle or highlight
                if (_supportsMultiSelect)
                {
                    bool newSelected = EditorGUILayout.Toggle(isSelected, GUILayout.Width(20));
                    if (newSelected != isSelected)
                    {
                        if (newSelected) _selectedIndices.Add(sourceIndex);
                        else _selectedIndices.Remove(sourceIndex);
                    }
                }

                // Icon + name
                var nob = prefab.GetComponent<NetworkObject>();
                Texture icon = nob != null ? EditorGUIUtility.GetIconForObject(nob) : null;
                if (icon == null) icon = EditorGUIUtility.IconContent("GameObject Icon").image;

                var style = isSelected && !_supportsMultiSelect
                    ? new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold }
                    : EditorStyles.label;

                var content = new GUIContent(" " + prefab.name, icon, AssetDatabase.GetAssetPath(prefab));

                if (GUILayout.Button(content, style, GUILayout.Height(20)))
                {
                    if (_supportsMultiSelect)
                    {
                        if (_selectedIndices.Contains(sourceIndex))
                            _selectedIndices.Remove(sourceIndex);
                        else
                            _selectedIndices.Add(sourceIndex);
                    }
                    else
                    {
                        _selectedIndices.Clear();
                        _selectedIndices.Add(sourceIndex);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            // Bottom bar
            EditorGUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            string selectionInfo = _selectedIndices.Count > 0
                ? $"{_selectedIndices.Count} selected"
                : "None selected";
            EditorGUILayout.LabelField(selectionInfo, EditorStyles.miniLabel, GUILayout.Width(100));

            if (GUILayout.Button("Confirm", GUILayout.Width(80)))
            {
                var result = _selectedIndices
                    .Where(idx => idx >= 0 && idx < _source.Count)
                    .Select(idx => _source[idx])
                    .Where(go => go != null)
                    .ToList();

                _onSelectionConfirmed?.Invoke(result);
                Close();
            }

            if (GUILayout.Button("Cancel", GUILayout.Width(80)))
            {
                Close();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(4);
        }
    }
}