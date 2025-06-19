using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

public class ControllerMenuNavigator : MonoBehaviour
{
    private EventSystem eventSystem;
    private Gamepad gamepad;
    private int currentSelectionIndex = 0;
    public GameObject[] menuButtons;

    private Color normalColor = Color.white;
    private Color highlightedColor = Color.red;
    private float inputCooldown = 0.2f;
    private float nextInputTime = 0f;
    
    public GameObject startPanel;
    public GameObject patchNotesPanel;
    
   [SerializeField]private GameObject currentPanel;
   [SerializeField]private GameObject previousPanel;
    void Start()
    {
        currentPanel = startPanel;
        eventSystem = EventSystem.current;
        gamepad = Gamepad.current;

        if (menuButtons.Length > 0)
        {
            SelectButton(currentSelectionIndex);
        }
    }

    void Update()
    {
        if (gamepad != null && Time.time >= nextInputTime)
        {
            Vector2 dpadInput = gamepad.dpad.ReadValue();

            if (dpadInput.y > 0.5f)
            {
                NavigateUp();
                nextInputTime = Time.time + inputCooldown;
            }
            else if (dpadInput.y < -0.5f)
            {
                NavigateDown();
                nextInputTime = Time.time + inputCooldown;
            }
        }
      
        if ((Keyboard.current.escapeKey.wasPressedThisFrame || gamepad.buttonEast.wasPressedThisFrame) && currentPanel == patchNotesPanel)
        {
            Debug.Log("working");
            GoBackToStart();
            
        }
    }

    void NavigateUp()
    {
        currentSelectionIndex--;
        if (currentSelectionIndex < 0) currentSelectionIndex = menuButtons.Length - 1;
        SelectButton(currentSelectionIndex);
    }

    void NavigateDown()
    {
        currentSelectionIndex++;
        if (currentSelectionIndex >= menuButtons.Length) currentSelectionIndex = 0;
        SelectButton(currentSelectionIndex);
    }

    void SelectButton(int index)
    {
        Debug.Log("Selected Button Index: " + index);
    
        if (index >= 0 && index < menuButtons.Length)
        {
            DeselectAllButtons();

            GameObject selectedObj = menuButtons[index];
            eventSystem.SetSelectedGameObject(selectedObj);

            Button selectedButton = selectedObj.GetComponent<Button>();
            if (selectedButton != null)
            {
           
                if (selectedButton.transition != Selectable.Transition.ColorTint)
                    selectedButton.transition = Selectable.Transition.ColorTint;


                ExecuteEvents.Execute(selectedObj, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);
            }
        }
    }


   void DeselectAllButtons()
    {
        foreach (GameObject buttonObj in menuButtons)
        {
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                ColorBlock colorBlock = button.colors;
                colorBlock.highlightedColor = normalColor;
                button.colors = colorBlock;  
            }
        }
    }
    void GoBackToStart()
    {
        if (startPanel != null && patchNotesPanel != null)
        {
            patchNotesPanel.SetActive(false);
            startPanel.SetActive(true);

            currentPanel = startPanel;
            previousPanel = patchNotesPanel;


            menuButtons = startPanel.GetComponentsInChildren<Button>(true)
                .Select(b => b.gameObject)
                .ToArray();

            currentSelectionIndex = 0;
            SelectButton(currentSelectionIndex);
        }
    }
}