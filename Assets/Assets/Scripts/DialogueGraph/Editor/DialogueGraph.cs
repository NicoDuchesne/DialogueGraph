using UnityEngine;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using System;

[Serializable]
[Graph(AssetsExtention)]

public class DialogueGraph : Graph
{

    public const string AssetsExtention = "dialoguegraph";

    [MenuItem("Assets/Create/Dialogue Graph", false)]
    private static void CreateAssetFile()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
    }
}
