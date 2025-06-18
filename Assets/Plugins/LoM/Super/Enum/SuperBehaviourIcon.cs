
namespace LoM.Super
{
    /// <summary>
    /// Enum to specify an icon for a SuperBehaviour
    /// </summary>
    public enum SuperBehaviourIcon
    {
        None = -1,
        Default = 0,
        
        // Game State 1-50
        GameManager = 1,
        GameState = 2,
        Manager = 3,
        Controller = 4,
        Spawn = 5,
        Animation = 6,
        Trigger = 7,
        Loader = 8,
        State = 9,
        SceneManager = 10,
        Generator = 11,
        
        // Player 51-70
        PlayerController = 51,
        PlayerMovement = 52,
        
        // UI 71-100
        Menu = 71,
        Control = 72,
        
        // Test 101-150
        Test = 101,
        Debug = 102,
        Gizmo = 103,
        
        // Misc 151-200
        Data = 151,
        Input = 152,
        Settings = 153,
        Sound = 154,
        File = 155,
        FileSmall = 156,
        Enum = 157,
        Interface = 158,
        
        // None
        Custom = 999
    }
}