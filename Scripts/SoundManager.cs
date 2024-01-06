using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipsRefsSO audioClipsRefsSO;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickUpSomething += Player_OnPickUpSomething;
        BaseCounter.OnAntObjectPlaceHere += BaseCounter_OnAntObjectPlaceHere;
        TrashCounter.OnAnyObjectTrash += TrashCounter_OnAnyObjectTrash;
    }

    private void TrashCounter_OnAnyObjectTrash(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = (TrashCounter)sender;
        PlaySound(audioClipsRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAntObjectPlaceHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = (BaseCounter)sender;
        PlaySound(audioClipsRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickUpSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipsRefsSO.objectPickUp, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = (CuttingCounter)sender;
        PlaySound(audioClipsRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipsRefsSO.deliveryFailed, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipsRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip audio, Vector3 position, float volume = 1f) 
    {
        AudioSource.PlayClipAtPoint(audio, position, volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0, audioClipArray.Length - 1)], position, volume);
    }

    public void PlayFootStepSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipsRefsSO.footstep, position, volume);
    }
}
