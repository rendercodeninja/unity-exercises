using CombatEngine.Schemas;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectionMenuController : MonoBehaviour
{
    //Reference to the ui document
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private SOCharacterData characterData;

    //2D array holding reference to individual selection swatch
    private VisualElement[,] mItemsArray;

    //Current row and column index
    private int currentRowIndex = 0;
    private int currentColIndex = 0;

    private readonly float moveDelay = 0.1f;
    private readonly float repeatRate = 0.1f;
    private float lastMoveTime;

    private Vector2 mAxisInputValue;

    //Reference to InputActions class
    PlayerInputActions mInputActions;

    //Unity - Awake
    private void Awake()
    {
        //Create new InputActions instance
        mInputActions = new PlayerInputActions();
    }

    //Unity - OnEnable
    private void OnEnable()
    {
        //Enable input actions
        mInputActions.Enable();

        //Bind input action events
        mInputActions.UI.AxisInput.performed += ctx => mAxisInputValue = ctx.ReadValue<Vector2>();
        mInputActions.UI.AxisInput.canceled += ctx => mAxisInputValue = Vector2.zero;
    }

    //Unity - OnDisable
    private void OnDisable()
    {
        //Disable input actions
        mInputActions.Disable();

        //Unbind input action events
        mInputActions.UI.AxisInput.performed -= ctx => mAxisInputValue = ctx.ReadValue<Vector2>();
        mInputActions.UI.AxisInput.canceled -= ctx => mAxisInputValue = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the array for the selection grid
        mItemsArray = new VisualElement[characterData.rowCount, characterData.columnCount];

        //Get the root element in the ui doument
        var rootElement = uiDocument.rootVisualElement;
        //Get the continer element in the ui hierarchy
        var container = rootElement.Q<VisualElement>("container");

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

                mItemsArray[row, col] = swatchItem;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (mAxisInputValue != Vector2.zero && Time.time > lastMoveTime + repeatRate)
        {
            lastMoveTime = Time.time + moveDelay;

            // Handle movement based on input (Y axis is inverted for up/down)
            int rowDelta = (int)Mathf.Clamp(-mAxisInputValue.y, -1, 1);  // Up/Down
            int colDelta = (int)Mathf.Clamp(mAxisInputValue.x, -1, 1);   // Left/Right

            MoveCursor(rowDelta, colDelta);
        }
    }

    //Moves the cursor to a particular index
    private void MoveCursor(int rowDelta, int colDelta)
    {
        //Clear current cursor
        ClearCursor(currentRowIndex, currentColIndex);

        // Update row and column indices with wrapping
        currentRowIndex = (currentRowIndex + rowDelta + characterData.rowCount) % characterData.rowCount;
        currentColIndex = (currentColIndex + colDelta + characterData.columnCount) % characterData.columnCount;

        //Set cursor to new index
        SetCursor(currentRowIndex, currentColIndex);
    }

    //Method to set cursor at specific index
    private void SetCursor(int row, int col)
    {
        //Get the swatch at index
        var item = mItemsArray[row, col];
        //Get the cursor element from swatch
        var cursor = item.Q<VisualElement>(name: "cursor");
        //Set cursor opacity
        cursor.style.opacity = 1;
    }

    //Method to clear cursor at specific index
    private void ClearCursor(int row, int col)
    {
        //Get the swatch at index
        var item = mItemsArray[row, col];
        //Get the cursor element from swatch
        var cursor = item.Q<VisualElement>(name: "cursor");
        //Clear cursor opacity
        cursor.style.opacity = 0;
    }

}
