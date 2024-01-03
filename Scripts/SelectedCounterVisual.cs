using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChange += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectCounterChangedEventArgs e)
    {
        if(e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach(var item in visualGameObjectArray)
        {
            item.gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (var item in visualGameObjectArray)
        {
            item.gameObject.SetActive(false);
        }
    }
}
