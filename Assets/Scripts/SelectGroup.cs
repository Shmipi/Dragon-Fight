
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectGroup : MonoBehaviour
{
    public Player player;
    [SerializeField] private List<Toggle> dragonToggleList;
    [SerializeField] private TextMeshProUGUI heading;

    private Coroutine randomSelectionCoroutine;

    private const string SELECT_DRAGON_TEXT = "SELECT YOUR DRAGON!";
    private const string AI_HEADING_TEXT = "CPU FIGHTER INCOMING!";

    private bool isPvP = true;

    private void Awake()
    {
        if (player == Player.Player2)
        {
            isPvP = GameMode.IsPVP;
        }
        Debug.Log(isPvP);
    }

    private void Start()
    {
        if (isPvP)
        {
            StartCoroutine(DelayedStart());
        }

        else
        {
            DisableDragonToggles();
            SelectRandomDragon();
        }

        SetHeading();
    }

    private void DisableDragonToggles()
    {
        foreach (Toggle toggle in dragonToggleList)
        {
            toggle.interactable = false;
        }
    }

    private void SetHeading()
    {
        heading.text = isPvP ? SELECT_DRAGON_TEXT : AI_HEADING_TEXT;
    }

    public void NewSelectedToggleButton(Toggle toggleButton)
    {
        foreach (Toggle toggle in dragonToggleList)
        {
            if(toggle != toggleButton)
            {
                toggle.isOn = false;
            }
        }
    }

    private void SelectFirstAvailableToggle()
    {
        foreach (var toggle in dragonToggleList)
        {
            if (toggle.gameObject.activeInHierarchy && toggle.interactable)
            {
                toggle.isOn = true;
                return;
            }
        }

        Debug.LogWarning("No dragon available");
    }

    private IEnumerator DelayedStart()
    {
        yield return WaitFor.Frames(5);
        SelectFirstAvailableToggle();
    }

    private void SelectRandomDragon()
    {
        if (randomSelectionCoroutine != null)
            StopCoroutine(randomSelectionCoroutine);

        randomSelectionCoroutine = StartCoroutine(RandomSelectionRoutine());
    }

    private IEnumerator RandomSelectionRoutine()
    {
        yield return new WaitForSeconds(3f);

        float duration = 7f; 
        float elapsed = 0f;
        float interval = 0.1f; 

        while (elapsed < duration)
        {
            // Pick a random dragon
            int randomIndex = Random.Range(0, dragonToggleList.Count);
            dragonToggleList[randomIndex].isOn = true;

            // Slow down the selection as time passes
            yield return new WaitForSeconds(interval);
            interval *= 1.15f; 
            elapsed += interval;
        }
    }
}
