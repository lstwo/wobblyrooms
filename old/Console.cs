using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using OldWobblyrooms;
using OldWobblyrooms.MainMenu;

public class Console : MonoBehaviour
{
    bool showConsole;
    string input;

    public InputAction toggleConsole;
    public InputAction returnKey;

    public static DebugCommand SET_SEED;
    public static DebugCommand NEW_SEED;
    public List<object> commandList;

    void OnToggleConsole(InputAction.CallbackContext context)
    {
        showConsole = !showConsole;
    }

    void OnReturn(InputAction.CallbackContext context)
    {
        HandleInput();
        input = "";
    }

    void Awake()
    {
        toggleConsole.performed += OnToggleConsole;
        returnKey.performed += OnReturn;

        SET_SEED = new DebugCommand("setseed", "Sets the Seed of the current save", "set_seed", () =>
        {
            GameSaves.GetSave(GameSaves.currentSave).seed = 0;
            Debug.Log("ayo");
        });

        NEW_SEED = new DebugCommand("newseed", "Generates a new Seed (Dependent on your current seed)", "new_seed", () => {
            GameSaves.GetSave(GameSaves.currentSave).seed = UnityEngine.Random.Range(0, Int32.MaxValue);
            Debug.Log("yo");
        });

        commandList = new List<object>
        {
            SET_SEED,
            NEW_SEED,
        };
    }

    void Update()
    {

    }

    private void OnGUI()
    {
        if(!showConsole) { return; }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);

        GUI.SetNextControlName("console");
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        GUI.FocusControl("console");
    }

    private void HandleInput()
    {
        Debug.Log("HandleInput");
        for(int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if(input.Contains(commandBase.commandId))
            {
                if(commandList[i] as DebugCommand != null)
                {
                    Debug.Log("Invoking Command");
                    (commandList[i] as DebugCommand).Invoke();
                }
            }
        }
    }
}

public class DebugCommandBase
{
    private string _commandId;
    private string _commandDescription;
    private string _commandFormat;

    public string commandId { get { return _commandId;} }
    public string commandDescription { get { return _commandDescription;} }
    public string commandFormat { get { return _commandFormat;} }

    public DebugCommandBase(string id, string description, string format)
    {
        _commandId = id;
        _commandDescription = description;
        _commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;

    public DebugCommand(string id, string description, string format, Action command) : base (id , description, format) 
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}
