using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class SelectionMenuController : MonoBehaviour
{
    #region Constants
    private readonly float moveDelay = 0.1f;
    private readonly float repeatRate = 0.1f;
    #endregion

    #region Inspector Fields
    //Reference to the ui document
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private SOCharacterData characterData;
    [SerializeField,Space(10)] private UnityEvent<SelectorHandler> OnCharacterSelect;
    #endregion

    #region Private Fields
    //2D array holding reference to selection swatches
    private VisualElement[,] mSwatchArray;
    private readonly SelectorHandler mSelectorPlayer1 = new(0);
    private readonly SelectorHandler mSelectorPlayer2 = new(1);
    #endregion

    //Custom class to handle selection data
    public class SelectorHandler
    {
        //Index of this selector object
        public int Index { get; private set; }
        //Whether the player is ready or not
        public bool IsReady { get; set; }
        //Vector2 to track WASD/LeftThumbStick axis value
        public Vector2 AxisInputValue { get; set; }
        //Current row index of the cursor
        public int Row { get; set; }
        //Current colum index of the cursor
        public int Column { get; set; }
        //Last time at which cursor moved
        public float LastMoveTime { get; set; }

        //Reference to the player label UI element
        private Label mLabel;
        //Tween which holds the idle animation for label (Until player joins)
        private Tween mLabelTween_Idle;

        //Constructor
        public SelectorHandler(int index) => Index = index;

        //Method to set the reference to the Label element
        public void SetLabelUIElement(Label label)
        {
            //Set label element reference
            mLabel = label;

            //Create the tween
            mLabelTween_Idle = DOTween.To(val => { mLabel.style.opacity = val; }, 0.0f, 1.0f, 1.0f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        //Method to apply the character name to the label
        public void SetCharacterName(string text)
        {
            //Kill anyu idle tween            
            mLabelTween_Idle.Kill();

            //Reset margin and opacity
            mLabel.style.opacity = 0;
            //Set margin based on index
            if (Index == 0)
                mLabel.style.marginLeft = 16;
            else if (Index == 1)
                mLabel.style.marginRight = 16;

            //Update label text value
            mLabel.text = text;

            //Use normalized value for multiple property tween
            DOTween.To(val =>
            {
                //Animate label opacity
                mLabel.style.opacity = val;

                //Animate margin based on index
                if (Index == 0)
                    mLabel.style.marginLeft = 16 + (0 - 16) * val;
                else if (Index == 1)
                    mLabel.style.marginRight = 16 + (0 - 16) * val;

            }, 0.0f, 1.0f, 0.2f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the array for the selection grid
        mSwatchArray = new VisualElement[characterData.rowCount, characterData.columnCount];

        //Get the root element in the ui doument
        var rootElement = uiDocument.rootVisualElement;
        //Get the continer element in the ui hierarchy
        var container = rootElement.Q<VisualElement>("container");

        //Cache the reference to label elements
        mSelectorPlayer1.SetLabelUIElement(rootElement.Q<Label>("label-P1"));
        mSelectorPlayer2.SetLabelUIElement(rootElement.Q<Label>("label-P2"));

        //Iterate through each row
        for (int row = 0; row < characterData.rowCount; row++)
        {
            //Get current row reference by name
            string rowName = $"row-{row}";
            var currentRow = container.Q<VisualElement>(rowName);

            //Iterate through each column
            for (int col = 0; col < characterData.columnCount; col++)
            {
                //Get swatch at current row, column index
                string swatchName = $"swatch-{col}";

                var swatchItem = currentRow.Q<VisualElement>(swatchName);

                var icon = swatchItem.Q<VisualElement>("icon");

                icon.style.backgroundImage = new StyleBackground(characterData.GetRowByIndex(row)[col].profileIcon);

                mSwatchArray[row, col] = swatchItem;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Update character selection for Player-1 if ready
        if (mSelectorPlayer1.IsReady)
            UpdateCharacterSelection(mSelectorPlayer1);

        if (mSelectorPlayer2.IsReady)
            UpdateCharacterSelection(mSelectorPlayer2);
    }

    //Updates the character selection for specific player 
    private void UpdateCharacterSelection(SelectorHandler selector)
    {
        //If input axis changed past last move time
        if (selector.AxisInputValue != Vector2.zero && Time.time > selector.LastMoveTime + repeatRate)
        {
            //Update last move time
            selector.LastMoveTime = Time.time + moveDelay;

            // Handle movement based on input (Y axis is inverted for up/down)
            int rowDelta = (int)Mathf.Clamp(-selector.AxisInputValue.y, -1, 1);  // Up/Down
            int colDelta = (int)Mathf.Clamp(selector.AxisInputValue.x, -1, 1);   // Left/Right

            //Clear current cursor
            SetCursor(selector.Index, selector.Row, selector.Column, false);

            // Update row and column indices with wrapping
            selector.Row = (selector.Row + rowDelta + characterData.rowCount) % characterData.rowCount;
            selector.Column = (selector.Column + colDelta + characterData.columnCount) % characterData.columnCount;

            //Set cursor to new index
            SetCursor(selector.Index, selector.Row, selector.Column, true);

            //Set Character name
            string name = characterData.GetRowByIndex(selector.Row)[selector.Column].characterName;
            selector.SetCharacterName(name);

            //Invoke event for external components
            OnCharacterSelect?.Invoke(selector);            
        }
    }

    //Method to set/clear cursor at specific index
    private void SetCursor(int playerIndex, int row, int col, bool isSet)
    {
        //Get the swatch at index
        var item = mSwatchArray[row, col];
        //Get the cursor element from swatch
        var cursor = item.Q<VisualElement>(name: playerIndex == 0 ? "cursor-p1" : "cursor-p2");
        //Get the tag element from swatch
        var tag = item.Q<VisualElement>(name: playerIndex == 0 ? "cursor-tag-p1" : "cursor-tag-p2");
        //Set cursor opacity
        cursor.style.opacity = tag.style.opacity = isSet ? 1 : 0;
    }

    //Callback Event - Invoked when player input configuration changes
    public void OnPlayerConfigChanged(PlayerConfigurationManager.PlayerConfig playerConfig)
    {
        //Get the UI action map
        var actionMap = playerConfig.Input.actions.FindActionMap("UI");
        //Get the AxisAction action
        var axisAction = actionMap.FindAction("AxisAction");

        //Get the target player selector
        SelectorHandler selector = playerConfig.PlayerIndex == 0 ? mSelectorPlayer1 : mSelectorPlayer2;

        //If Player connected
        if (playerConfig.PlayerConnected)
        {
            //Bind axis events
            axisAction.performed += ctx => selector.AxisInputValue = ctx.ReadValue<Vector2>();
            axisAction.canceled += ctx => selector.AxisInputValue = Vector2.zero;

            //If player selector not ready
            if (!selector.IsReady)
            {
                //Set staring row index
                selector.Row = 0;
                //Set column index as 5 for P1 and 6 for P2
                selector.Column = playerConfig.PlayerIndex == 0 ? 5 : 6;

                //Update last selector move time
                selector.LastMoveTime = Time.time + moveDelay;

                //Set cursor
                SetCursor(selector.Index, selector.Row, selector.Column, true);

                //Set label value
                string name = characterData.GetRowByIndex(selector.Row)[selector.Column].characterName;
                selector.SetCharacterName(name);

                //Invoke event for external components
                OnCharacterSelect?.Invoke(selector);

                //State player selector is ready
                selector.IsReady = true;
            }
        }
        //Else if disconnected
        else
        {
            //Unbind axis events
            axisAction.performed -= ctx => selector.AxisInputValue = ctx.ReadValue<Vector2>();
            axisAction.canceled -= ctx => selector.AxisInputValue = Vector2.zero;

            //State player is not ready
            selector.IsReady = false;
        }
    }
}
