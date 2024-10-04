
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

/// <summary>
/// The global mapping between InteractionOption's and a key that a player can press to start interaction.
/// </summary>
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

/// <summary>
/// A KeyCode with a user-friendly string name
/// </summary>
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

/// <summary>
/// An InteractionOption with a user-friendly string name.
/// </summary>
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

