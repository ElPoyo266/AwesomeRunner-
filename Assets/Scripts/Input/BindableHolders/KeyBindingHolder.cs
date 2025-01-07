using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyBindingHolder : IBindingHolder<KeyBinding>
{
    public Dictionary<ECommand, KeyBinding> InputBindings { get; private set; }

    private readonly Dictionary<ECommand, KeyBinding> DefaultKeyBindings = new Dictionary<ECommand, KeyBinding>
    {
        {ECommand.NONE, new KeyBinding(KeyCode.None)},
        {ECommand.DOWN, new KeyBinding(KeyCode.DownArrow)},
        {ECommand.UP, new KeyBinding(KeyCode.UpArrow)},
        {ECommand.LEFT, new KeyBinding(KeyCode.LeftArrow)},
        {ECommand.RIGHT, new KeyBinding(KeyCode.RightArrow)},
        {ECommand.OPEN_SCOREBOARD, new KeyBinding(KeyCode.Tab)},
        {ECommand.OPEN_PAUSE_MENU, new KeyBinding(KeyCode.Escape)}
    };
    
    private readonly Dictionary<ECommand, KeyBinding> Player2KeyBindings = new Dictionary<ECommand, KeyBinding>
    {
        {ECommand.DOWN2, new KeyBinding(KeyCode.S)},
        {ECommand.UP2, new KeyBinding(KeyCode.Z)},
        {ECommand.LEFT2, new KeyBinding(KeyCode.Q)},
        {ECommand.RIGHT2, new KeyBinding(KeyCode.D)},
    };

    public void Init(Boolean isPlayer2 = false)
    {
        if (InputBindings == null)
            if (isPlayer2)
            {
                InputBindings = Player2KeyBindings;
            }
            else
            {
                InputBindings = DefaultKeyBindings;
            }
    }
}
