
using System.Collections.Generic;

public enum ESceneState
{
    Loaded,
    InLoad,
    InUnload,
    Unloaded
}

public enum ESceneEvent
{
    OnLoad,
    OnUnload,
    OnSyncronize,
    OnSwitch,
    OnLoadComplete,
    OnUnloadComplete,
    OnSwitchComplete,
    OnSyncronizeComplete,
    OnLoadEventComplete,
    OnunloadEventComplete,
    OnSwitchEventComplete
}

public enum ESceneError
{
    Disconnect,
    Timeout
}

public struct SceneDescriptor
{
    public string sceneName;
    public bool bNeeded;
    public ESceneState state;

    public List<string> chunkLockers;

    public override string ToString()
    {
        string str = "CL: ";
        foreach (var cl in chunkLockers)
        {
            str += cl + " ";
        }
        return $"{sceneName}: {bNeeded} {state} {str}";
    }
}

public struct ServerSceneDescriptor
{
    public string sceneName;
    public bool bNeeded;
    public ESceneState state;

    public List<ulong> clientLockers;

    public override string ToString()
    {
        string str = "CL: ";
        foreach (var cl in clientLockers)
        {
            str += cl + " ";
        }
        return $"{sceneName}: {bNeeded} {state} {str}";
    }
}

public struct SceneStateDescriptor
{
    public string sceneName;
    public ESceneState currentState;
    public ESceneState targetState;
}
