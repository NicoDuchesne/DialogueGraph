using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public RuntimeDialogueGraph RunitmeGraph;
    [Header("Panel Settings")]
    [Header("UI Components")]
    public GameObject DialoguePanelContainer;
    public TextMeshProUGUI PanelSpeakerNameText;
    public TextMeshProUGUI PanelDialogueText;

    [Header("Choice Button UI")]
    public Button PanelChoiceButtonPrefab;
    public Transform PanelChoiceButtonContainer;

    [Header("Popup Settings")]
    [Header("UI Components")]
    public GameObject DialoguePopupContainer;
    public TextMeshProUGUI PopupDialogueText;

    [Header("Choice Button UI")]
    public Button PopupChoiceButtonPrefab;
    public Transform PopupChoiceButtonContainer;

    [Header("Bulle Settings")]
    [Header("UI Components")]
    public GameObject DialogueBulleContainer;
    public TextMeshProUGUI BulleDialogueText;

    private Dictionary<string, RuntimeDialogueNode> _nodeLookup = new Dictionary<string, RuntimeDialogueNode>();
    private RuntimeDialogueNode _currentNode;

    void Start()
    {
        foreach (var node in RunitmeGraph.AllNodes)
        {
            _nodeLookup[node.NodeID] = node;
        }

        if (!string.IsNullOrEmpty(RunitmeGraph.EntryNodeID))
        {
            ShowNode(RunitmeGraph.EntryNodeID);
        }
        else
        {
            EndDialogue();
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && _currentNode != null && _currentNode.Choices.Count == 0 && _currentNode.UIModuleType != DialogueUIModuleType.Bulle)
        {
            if (!string.IsNullOrEmpty(_currentNode.NextNodeID))
            {
                ShowNode(_currentNode.NextNodeID);
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void ShowNode(string nodeID)
    {
        if (!_nodeLookup.ContainsKey(nodeID))
        {
            EndDialogue();
            return;
        }

        _currentNode = _nodeLookup[nodeID];

        switch(_currentNode.UIModuleType)
        {
            case DialogueUIModuleType.Panel:
                ShowPanelNode();
                break;
            case DialogueUIModuleType.Popup:
                ShowPopupNode();
                break;
            case DialogueUIModuleType.Bulle:
                ShowBulleNode();
                break;
        }
        
    }

#region Show Node Types
    private void ShowBulleNode()
    {
        DialoguePanelContainer.SetActive(false);
        DialoguePopupContainer.SetActive(false);
        DialogueBulleContainer.SetActive(true);
        BulleDialogueText.SetText(_currentNode.DialogueText);
        Debug.Log(_currentNode.NextNodeID);
        StartCoroutine(WaitAndEndDialogue(_currentNode.DisplayDuration));
    }

    private void ShowPopupNode()
    {
        DialoguePanelContainer.SetActive(false);
        DialogueBulleContainer.SetActive(false);
        DialoguePopupContainer.SetActive(true);
        PopupDialogueText.SetText(_currentNode.DialogueText);

        foreach (Transform child in PopupChoiceButtonContainer)
        {
            Destroy(child.gameObject);
        }

        if (_currentNode.Choices.Count > 0)
        {
            foreach (var choice in _currentNode.Choices)
            {
                Button button = Instantiate(PopupChoiceButtonPrefab, PopupChoiceButtonContainer);

                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.SetText(choice.ChoiceText);
                }

                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        if (!string.IsNullOrEmpty(choice.DestinationNodeID))
                        {
                            ShowNode(choice.DestinationNodeID);
                        }
                        else
                        {
                            EndDialogue();
                        }
                    });
                }
            }
        }
    }

    private void ShowPanelNode()
    {
        DialoguePanelContainer.SetActive(true);
        DialogueBulleContainer.SetActive(false);
        DialoguePopupContainer.SetActive(false);
        PanelSpeakerNameText.SetText(_currentNode.SpeakerName);
        PanelDialogueText.SetText(_currentNode.DialogueText);

        foreach (Transform child in PanelChoiceButtonContainer)
        {
            Destroy(child.gameObject);
        }

        if (_currentNode.Choices.Count > 0)
        {
            foreach (var choice in _currentNode.Choices)
            {
                Button button = Instantiate(PanelChoiceButtonPrefab, PanelChoiceButtonContainer);

                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.SetText(choice.ChoiceText);
                }

                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        if (!string.IsNullOrEmpty(choice.DestinationNodeID))
                        {
                            ShowNode(choice.DestinationNodeID);
                        }
                        else
                        {
                            EndDialogue();
                        }
                    });
                }
            }
        }
    }

#endregion
    private void EndDialogue()
    {
        switch(_currentNode?.UIModuleType)
        {
            case DialogueUIModuleType.Panel:
                DialoguePanelContainer.SetActive(false);
                foreach (Transform child in PanelChoiceButtonContainer)
                {
                    Destroy(child.gameObject);
                }
                break;
            case DialogueUIModuleType.Popup:
                DialoguePopupContainer.SetActive(false);
                foreach (Transform child in PopupChoiceButtonContainer)
                {
                    Destroy(child.gameObject);
                }
                break;
            case DialogueUIModuleType.Bulle:
                DialogueBulleContainer.SetActive(false);
                break;
        }
        _currentNode = null;
    }

    IEnumerator WaitAndEndDialogue(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!string.IsNullOrEmpty(_currentNode.NextNodeID))
        {
            ShowNode(_currentNode.NextNodeID);
        }
        else
        {
            EndDialogue();
        }
    }
}
