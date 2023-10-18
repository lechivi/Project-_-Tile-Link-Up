using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSetting : EditorWindow
{
    [MenuItem("Window/UI Toolkit/GameSetting")]
    public static void ShowWindow()
    {
        GameSetting wnd = GetWindow<GameSetting>();
        wnd.titleContent = new GUIContent("GameSetting");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy
        Label label = new Label("Hello World!");
        root.Add(label);

        // Create button
        Button button = new Button();
        button.name = "button";
        button.text = "Button";
        root.Add(button);

        // Create toggle
        Toggle toggle = new Toggle();
        toggle.name = "toggle";
        toggle.label = "Toggle";
        root.Add(toggle);
    }

    string textFiled;

    private void OnGUI()
    {
        GUILayout.Label("This is a label!", EditorStyles.boldLabel);
        EditorGUILayout.TextField("TEXT FIELD", textFiled);

        if (GUILayout.Button("Press me"))
        {
            Debug.Log("Button was pressed");
        }
    }
}
