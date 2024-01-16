using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeDelliveredText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button optionsButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });
        menuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionUI.Instance.Show();
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnLocalGamePause += GamePause_OnLocalGamePause;
        KitchenGameManager.Instance.OnLocalGameUnpause += GameUnpause_OnLocalGameUnpause;

        Hide();
    }

    private void GameUnpause_OnLocalGameUnpause(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GamePause_OnLocalGamePause(object sender, System.EventArgs e)
    {
        Show();
        recipeDelliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipeAmount().ToString();
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
