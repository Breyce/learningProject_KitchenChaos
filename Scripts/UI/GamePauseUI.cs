using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeDelliveredText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });
        menuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGamePause += GamePause_OnGamePause;
        KitchenGameManager.Instance.OnGameUnpause += GameUnpause_OnGameUnpause;

        Hide();
    }

    private void GameUnpause_OnGameUnpause(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GamePause_OnGamePause(object sender, System.EventArgs e)
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
