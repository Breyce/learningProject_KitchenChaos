using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance { get; private set; }

    [SerializeField] private Button soundEffectButton;
    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button chopButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI chopText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Transform pressToRebind;

    private void Awake()
    {
        Instance = this;
        soundEffectButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Up);

        });
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Down);

        });
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Left);

        });
        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Move_Right);

        });
        chopButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.InteractAlternate);

        });
        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact);

        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGamePause += KitchenGameManager_OnGamePause;
        UpdateVisual();
        HidePressToRebind();
        Hide();
    }

    private void KitchenGameManager_OnGamePause(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectText.text = "Sound Effects:  " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music:  " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Move_Right);
        chopText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.InteractAlternate);
        interactText.text = GameInput.Instance.GetBingdingText(GameInput.Binding.Interact);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebind()
    {
        pressToRebind.gameObject.SetActive(true);
    }

    private void HidePressToRebind()
    {
        pressToRebind.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebind();
        GameInput.Instance.RebindBinding(binding, () => { 
            HidePressToRebind(); 
            UpdateVisual(); 
        });
    }
}
