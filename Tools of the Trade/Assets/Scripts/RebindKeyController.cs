using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class KeyRebinder : MonoBehaviour
{
    [Header("Input Action Reference")]
    [SerializeField] private InputSystem_Actions inputActions;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI rebindText;

    private InputAction moveAction;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    private enum RebindState
    {
        Idle,
        WaitingForOldKey,
        WaitingForNewKey
    }

    private RebindState state = RebindState.Idle;

    private int bindingIndex = 0;

    private string oldKeyName = "";
    private string newKeyName = "";

    private void Awake()
    {
        if (inputActions == null)
            inputActions = new InputSystem_Actions();

        moveAction = inputActions.Player.Move;

        Debug.Log("[Rebind] Initialized");
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
        CancelRebind();
    }

    public void StartRebind()
    {
        Debug.Log("[Rebind] Starting rebind process");

        //inputActions.Player.Disable();

        oldKeyName = "";
        newKeyName = "";

        state = RebindState.WaitingForOldKey;

        rebindText.text = "Press the key you want to replace...";

        StartRebinding();
    }

    private void StartRebinding()
    {
        CancelRebind();
        moveAction.Disable();
        rebindOperation = moveAction.PerformInteractiveRebinding(bindingIndex)
            .WithControlsHavingToMatchPath("<Keyboard>")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                string selectedPath = operation.selectedControl.path;
                string displayName = operation.selectedControl.displayName;

                Debug.Log($"[Rebind] Selected: {displayName} ({selectedPath})");

                if (state == RebindState.WaitingForOldKey)
                {
                    oldKeyName = displayName;

                    state = RebindState.WaitingForNewKey;

                    rebindText.text =
                        $"Old key: {oldKeyName}\nNow press the new key...";

                    operation.Dispose();

                    StartRebinding();
                }
                else if (state == RebindState.WaitingForNewKey)
                {
                    newKeyName = displayName;

                    moveAction.ApplyBindingOverride(bindingIndex, selectedPath);

                    rebindText.text =
                        $"{oldKeyName} → {newKeyName}";

                    state = RebindState.Idle;

                    operation.Dispose();

                    inputActions.Player.Enable();
                }
            })
            .OnCancel(operation =>
            {
                Debug.LogWarning("[Rebind] Rebind cancelled");

                state = RebindState.Idle;
                rebindText.text = "Rebind cancelled";

                inputActions.Player.Enable();
            });

        rebindOperation.Start();
    }

    private void CancelRebind()
    {
        if (rebindOperation != null)
        {
            rebindOperation.Cancel();
            rebindOperation.Dispose();
            rebindOperation = null;
        }
    }
}