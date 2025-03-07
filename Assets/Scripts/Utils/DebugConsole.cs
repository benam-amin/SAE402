using System.Collections.Generic;
using System;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.SceneManagement;

enum DisplayType
{
    Hide,
    HidePanel,
    Show,
    Autocomplete,
    UnknownCommand,
}

// https://www.youtube.com/watch?v=VzOEM-4A2OM
// https://github.com/MinaPecheux/UnityTutorials-RTS/blob/master/Assets/Scripts/DebugConsole/DebugConsole.cs
public class DebugConsole : MonoBehaviour
{
    private bool showConsole;
    private bool showHelp;

    private string input = "";

    public static DebugCommand HELP;
    public static DebugCommand HELP_2;
    public static DebugCommand<string> TELEPORT;
    public static DebugCommand<string> LOAD;
    public static DebugCommand<float?> HEAL;
    public static DebugCommand<float?> HURT;
    public static DebugCommand QUIT;
    public static DebugCommand DIE;
    public static DebugCommand RELOAD;
    public static DebugCommand PRINT_SCREEN;
    public List<object> commandList;

    private Vector2 scroll;

    private readonly CultureInfo cultureInfo = new CultureInfo("en-US");

    private bool finishAutoCompletion = false;

    [SerializeField]
    private GUIStyle mainContainerStyle;

    [SerializeField]
    private GUIStyle unknownCmdLabelStyle;

    [SerializeField]
    private DisplayType displayType = DisplayType.Hide;

    [Header("ScriptableObjects")]
    public PlayerData playerData;

    [Header("Broadcast event channels")]
    public VectorEventChannel onDebugTeleportEvent;
    public StringEventChannel onLevelEnded;
    public VoidEventChannel onDebugPlayerDeathEvent;
    public BoolEventChannel onDebugConsoleOpenEvent;

    private void Awake()
    {
        HELP = new DebugCommand("help", "Show all commands", "help", () =>
        {
            showHelp = true;
        });

        HELP_2 = new DebugCommand("?", "Show all commands", "?", () =>
        {
            showHelp = true;
        });

        QUIT = new DebugCommand("quit", "Close the help command", "quit", () =>
        {
            Hide();
        });

        DIE = new DebugCommand("die", "kill player", "die", () =>
        {
            onDebugPlayerDeathEvent.Raise();
        });

        TELEPORT = new DebugCommand<string>("teleport", "Teleports player into a specific place. Example : teleport 10,0.6", "teleport <string as Vector2>", (val) =>
        {
            onDebugTeleportEvent.Raise(StringToVector3(val));
            Camera.main.transform.position = Camera.main.GetComponent<CameraFollow>().GetNextPosition();
        });

        HEAL = new DebugCommand<float?>("heal", "Heal the player with an amount of points (0 by default)", "heal <int>", (val) =>
        {
            playerData.currentHealth += val ?? 0;
        });

        HURT = new DebugCommand<float?>("hurt", "Hurt the player with an amount of points (0 by default)", "hurt <int>", (val) =>
        {
            playerData.currentHealth -= val ?? 0;
        });

        LOAD = new DebugCommand<string>("load_scene", "Load specific scene by name", "load_scene <string>", (val) =>
        {
            onLevelEnded.Raise(val);
        });

        RELOAD = new DebugCommand("reload_scene", "Reload current scene", "reload_scene", () =>
        {
            onLevelEnded.Raise(SceneManager.GetActiveScene().name);
        });

        PRINT_SCREEN = new DebugCommand("printscreen", "Take a screenshot of the camera", "printscreen", () =>
        {
            ScreenCapture.CaptureScreenshot("SomeLevel.png");
        });

        commandList = new List<object> {
            HELP,
            HELP_2,
            TELEPORT,
            HEAL,
            HURT,
            QUIT,
            LOAD,
            DIE,
            RELOAD,
            PRINT_SCREEN,
        };
        commandList = commandList
            .Select(x => x as DebugCommandBase)
            .OrderBy(x => x.commandId)
            .Select(x => x as object)
            .ToList();
    }

    public Vector3 StringToVector3(string rString)
    {
        string[] listParams = rString.Split(',');
        try
        {
            float x = Convert.ToSingle(listParams[0], cultureInfo);
            float y = Convert.ToSingle(listParams[1], cultureInfo);
            Vector3 rValue = new(x, y, 0);

            return rValue;
        }
        catch
        {
            return Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F12))
        {
            showConsole = !showConsole;
            if (showConsole)
            {
                onDebugConsoleOpenEvent.Raise(showConsole);
            }
            else
            {
                Hide();
            }
        }
#endif
    }

    private void OnGUI()
    {
        if (!showConsole)
        {
            return;
        }

        mainContainerStyle = new GUIStyle(GUI.skin.box);
        float y = 0f;
        float inputContainerHeight = 50;

        GUI.Box(new Rect(0, y, Screen.width, inputContainerHeight), "DEBUG CONSOLE - Use help or ? command to list all available commands", mainContainerStyle);

        input = GUI.TextField(new Rect(10f, y + 20f, Screen.width - 20f, 20f), input);

        // Log area
        y = 30f;
        if (displayType != DisplayType.Hide && displayType != DisplayType.HidePanel)
        {
            if (displayType == DisplayType.UnknownCommand)
            {
                GUI.Box(new Rect(0, inputContainerHeight, Screen.width, 60), "");
                GUI.Label(new Rect(10, inputContainerHeight + 20, Screen.width, 35), $"Unknown command. Use help or ? command to list all available commands", unknownCmdLabelStyle);
            }
            else
            {
                GUI.Box(new Rect(0, inputContainerHeight, Screen.width, 165), "");
            }
        }

        if (input.Length > 0 && displayType == DisplayType.Autocomplete)
        {
            Autocomplete(inputContainerHeight, input.Split(" ")[0], finishAutoCompletion);
        }
        else if (showHelp)
        {
            ShowHelp(inputContainerHeight);
        }
        else if (input.Length == 0 && displayType != DisplayType.UnknownCommand)
        {
            displayType = DisplayType.Hide;
        }

        Event e = Event.current;
        if (e.isKey)
        {
            if (
                (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter) &&
                input.Length > 0
            )
            {
                HandleInput();
            }
            else if (
                e.keyCode == KeyCode.Escape ||
                (e.keyCode == KeyCode.D && (Event.current.modifiers == EventModifiers.Control || Event.current.modifiers == EventModifiers.Command))
            )
            {
                Hide();
            }
            else
            {
                finishAutoCompletion = false;
            }
        }

        // https://docs.unity3d.com/ScriptReference/Event.KeyboardEvent.html
        if (Event.current.Equals(Event.KeyboardEvent("tab")))
        {
            if (displayType == DisplayType.Autocomplete)
            {
                finishAutoCompletion = true;
            }
            displayType = DisplayType.Autocomplete;
        }
    }

    private void Hide()
    {
        GUI.FocusControl(null);
        showConsole = false;
        onDebugConsoleOpenEvent.Raise(showConsole);
        displayType = DisplayType.Hide;
    }

    private void HandleInput()
    {
        string[] listCommandProperties = input.Split(" ");

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            string pattern = @"\b" + Regex.Escape(commandBase.commandId) + @"\b";
            if (Regex.IsMatch(input.Trim(), pattern) || input.Trim() == "?")
            {
                input = "";
                displayType = DisplayType.HidePanel;
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                    break;
                }
                else if (commandList[i] as DebugCommand<string> != null)
                {
                    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/ranges#systemrange
                    (commandList[i] as DebugCommand<string>).Invoke(string.Join("", listCommandProperties[1..]));
                    break;
                }
                else if (commandList[i] as DebugCommand<float?> != null)
                {
                    if (float.TryParse(listCommandProperties[1], out float val))
                    {
                        (commandList[i] as DebugCommand<float?>).Invoke(val);
                    }
                    else
                    {
                        Debug.LogError($"requires a float parameter!");
                    }
                    break;
                }
            }
            else
            {
                displayType = DisplayType.UnknownCommand;
            }
        }
    }

    private void ShowHelp(float y)
    {
        displayType = DisplayType.Show;
        Rect helpContainerViewport = new(0, 0, Screen.width, 20 * commandList.Count);
        scroll = GUI.BeginScrollView(new Rect(0, y + 5, Screen.width, 150), scroll, helpContainerViewport, GUIStyle.none, GUI.skin.verticalScrollbar);

        ShowResults(0, commandList.Select(x => x as DebugCommandBase).ToList());

        GUI.EndScrollView();
    }


    private void Autocomplete(float y, string newInput, bool fillField)
    {
        if (commandList.Select(x => x as DebugCommandBase).Where(k => k.commandId.StartsWith(newInput.ToLower())).ToList() == null)
        {
            return;
        }
        List<DebugCommandBase> autocompleteCommands = commandList.Select(x => x as DebugCommandBase)
            .Where(k => k.commandId.StartsWith(newInput.ToLower())).ToList();

        Rect helpContainerViewport = new(0, y, Screen.width, 20 * autocompleteCommands.Count);
        scroll = GUI.BeginScrollView(new Rect(0, y + 5, Screen.width, 150), scroll, helpContainerViewport, GUIStyle.none, GUI.skin.verticalScrollbar);

        if (autocompleteCommands.Count == 0)
        {
            displayType = DisplayType.Hide;
        }

        if (fillField && autocompleteCommands.Count == 1)
        {
            DebugCommandBase command = autocompleteCommands[0];
            input = command.commandFormat;
        }

        ShowResults(y, autocompleteCommands);

        GUI.EndScrollView();
    }

    private void ShowResults(float y, List<DebugCommandBase> list)
    {
        const float itemHeight = 20;
        foreach (var item in list.Select((value, idx) => new { idx, value }))
        {
            DebugCommandBase command = item.value;
            int index = item.idx;

            string commandLabel = $"{command.commandFormat} - {command.commandDescription}";
            Rect commandLabelRect = new(5, y + (itemHeight * index), Screen.width - 25f, itemHeight);

            if (GUI.Button(commandLabelRect, commandLabel))
            {
                input = command.commandFormat;
            }
        }
    }
}
