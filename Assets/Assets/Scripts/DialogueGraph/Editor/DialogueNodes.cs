using UnityEngine;
using Unity.GraphToolkit.Editor;
using System;
using Unity.Properties;

[Serializable]

public class StartNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddOutputPort("out").Build();
    }
}

[Serializable]
public class EndNode : Node
{
    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();
    }
}

[Serializable]
public class DialogueNode : Node
{
    const DialogueUIModuleType defaultUIModuleType = DialogueUIModuleType.Panel;
    override protected void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();
        context.AddOutputPort("out").Build();
        
        var uiOption = GetNodeOptionByName("UIModuleType");
        var uiType = uiOption.TryGetValue(out DialogueUIModuleType value)
        ? value
        : DialogueUIModuleType.Panel;
        switch (uiType)
        {
            case DialogueUIModuleType.Panel:
                context.AddInputPort<string>("Speaker").Build();
                context.AddInputPort<string>("Dialogue").Build();
                break;
            case DialogueUIModuleType.Popup:
                context.AddInputPort<string>("Dialogue").Build();
                break;
            case DialogueUIModuleType.Bulle:
                context.AddInputPort<string>("Dialogue").Build();
                context.AddInputPort<float>("Display Duration").Build();
                break;
        }
    }

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<DialogueUIModuleType>("UIModuleType").WithDefaultValue(defaultUIModuleType).Delayed();
    }
}

[Serializable]
public class ChoiceNode : Node
{
    const string optionID = "portCount";
    const DialogueUIModuleType defaultUIModuleType = DialogueUIModuleType.Panel;

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("in").Build();

        var uiOption = GetNodeOptionByName("UIModuleType");
        var uiType = uiOption.TryGetValue(out DialogueUIModuleType value)
        ? value
        : DialogueUIModuleType.Panel;

        switch (uiType)
        {
            case DialogueUIModuleType.Panel:
                context.AddInputPort<string>("Speaker").Build();
                context.AddInputPort<string>("Dialogue").Build();
                break;
            case DialogueUIModuleType.Popup:
                context.AddInputPort<string>("Dialogue").Build();
                break;
            case DialogueUIModuleType.Bulle:
                return;
        }
        
        var option = GetNodeOptionByName(optionID);
        option.TryGetValue(out int portCount);
        for (int i = 0; i < portCount; i++)
        {
            context.AddInputPort<string>($"Choice Text {i}").Build();
            context.AddOutputPort($"Choice {i}").Build();
        }
    }

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<DialogueUIModuleType>("UIModuleType").WithDefaultValue(defaultUIModuleType).Delayed();
        context.AddOption<int>(optionID).WithDefaultValue(2).Delayed();
    }
}