using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerConfigurationManager;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerConfigurationManager : MonoBehaviour
{
    #region Defenitions
    //Custom class holding joined player configuration
    public class PlayerConfig
    {
        //Reference to the PlayerInput object
        public PlayerInput Input { get; set; }
        //Reference to the PlayerIndex
        public int PlayerIndex { get; set; }
        //Whether this player is connected or not
        public bool PlayerConnected { get; set; }
    }
    #endregion

    //Configuration objects for Players
    private List<PlayerConfig> mPlayerConfigs = new();

    //Reference to the PlayerInputManager component
    private PlayerInputManager mPlayerInputmanager;

    //UnityEvent - Gets invoked when a PlayerConfig changed
    public UnityEvent<PlayerConfig> OnPlayerConfigChanged;

    //Unity - Awake
    private void Awake()
    {
        //Get reference to PlayerInputmanager
        mPlayerInputmanager = GetComponent<PlayerInputManager>();
    }

    //Unity - OnEnable
    private void OnEnable()
    {
        //Register C# event handlers
        mPlayerInputmanager.onPlayerJoined += OnPlayerJoined;
        mPlayerInputmanager.onPlayerLeft += OnPlayerLeft;
    }

    //Unity - OnDisable
    private void OnDisable()
    {
        //De-Register C# event handlers
        mPlayerInputmanager.onPlayerJoined -= OnPlayerJoined;
        mPlayerInputmanager.onPlayerLeft -= OnPlayerLeft;
    }

    //Event - Triggered when a new player is joined (Keyboard/Controller)
    private void OnPlayerJoined(PlayerInput input)
    {

        // Check if the list doesn't contain an entry with the same PlayerInput reference
        if (!mPlayerConfigs.Any(config => config.Input == input))
        {
            // Create a new PlayerConfig instance and add it to the list
            PlayerConfig config = new() { Input = input, PlayerIndex = input.playerIndex, PlayerConnected = true };
            //Add to the config list
            mPlayerConfigs.Add(config);

            //Throw callback
            OnPlayerConfigChanged?.Invoke(config);

            //Log here
            Debug.Log($"Player connected: {input.playerIndex}");
        }

    }

    //Event - Triggered when an existing player is disconnected (Keyboard/Controller
    private void OnPlayerLeft(PlayerInput input)
    {
        // Use LINQ to find the matching PlayerConfig based on the PlayerInput
        PlayerConfig config = mPlayerConfigs.FirstOrDefault(config => config.Input == input);

        //If valid config
        if (config != null)
        {
            //Update connection state
            config.PlayerConnected = false;

            //Throw callback
            OnPlayerConfigChanged?.Invoke(config);
        }
    }

}