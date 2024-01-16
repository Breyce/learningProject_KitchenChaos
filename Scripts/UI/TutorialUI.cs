using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Key_Move_Up;
    [SerializeField] private TextMeshProUGUI Key_Move_Down;
    [SerializeField] private TextMeshProUGUI Key_Move_Left;
    [SerializeField] private TextMeshProUGUI Key_Move_Right;
    [SerializeField] private TextMeshProUGUI Key_Interact;
    [SerializeField] private TextMeshProUGUI Key_Alt;
    [SerializeField] private TextMeshProUGUI Key_Pause;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnLocalPlayerReadyChanged += KitchenGameManager_OnLocalPlayerReadyChanged;

        UpdateVisual();
        Show();
    }

    private void KitchenGameManager_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsLocalPlayerReady())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        Key_Move_Up.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Up);
        Key_Move_Down.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Down);
        Key_Move_Left.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Left);
        Key_Move_Right.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Right);
        Key_Interact.text = GameInput.Instance.GetBingdingText(GameInput.Binding.InteractAlternate);
        Key_Alt.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Interact);
        Key_Pause.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Puse);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
