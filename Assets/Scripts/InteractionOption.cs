
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum InteractionOption
{
    PICK_UP,
    OPEN,
    CLOSE,
    PULL,
    ZOOM
}

public class OptionsKeyMap
{
    public static Dictionary<InteractionOption, NamedKey> map = new Dictionary<InteractionOption, NamedKey>() {
        {InteractionOption.PICK_UP, new NamedKey("LMB", KeyCode.Mouse0) },
        {InteractionOption.OPEN, new NamedKey("E", KeyCode.E)},
        {InteractionOption.CLOSE, new NamedKey("E", KeyCode.E)},
        {InteractionOption.PULL, new NamedKey ("E", KeyCode.E)},
        {InteractionOption.ZOOM, new NamedKey("E", KeyCode.E) }
    };
}

[Serializable]
public struct NamedKey
{
    public string name;
    public KeyCode keyCode;

    public NamedKey(string name, KeyCode keyCode)
    {
        this.name = name;
        this.keyCode = keyCode;
    }
}

[Serializable]
public struct InteractionOptionInstance
{
    public InteractionOption option;

    // For example, "Pick up". This is set by a particular gameplay object
    public string gameplayName;

    public InteractionOptionInstance(InteractionOption option, string gameplayName)
    {
        this.option = option;
        this.gameplayName = gameplayName;
    }
}

