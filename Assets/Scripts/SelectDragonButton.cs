using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectDragonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] DragonSkinData dragonSkinData;
    [SerializeField] UIClickSound clickSound;

    private Color defaultBackgroundColor;
    private Color selectedBackgroundColor;
    private const float selectedAlpha = 0.65f;

    private Image parentBackground;
    private Material outlineMaterial;
    private const float highlightAmount = 0.015f;
    private readonly int _OutlineSize = Shader.PropertyToID("_OutlineSize");
    private const float highlightDuration = 0.1f;
    private Coroutine highlightCoroutine;

    private Toggle toggleButton;
    private SelectGroup selectGroup;

    private Player player;

    private void Start()
    {
        selectGroup = transform.parent.parent.GetComponent<SelectGroup>();
        player = selectGroup.player;

        outlineMaterial = new Material(GetComponent<Image>().material);
        GetComponent<Image>().material = outlineMaterial;
        outlineMaterial.SetFloat(_OutlineSize, 0f);

        parentBackground = transform.parent.GetComponent<Image>();
        defaultBackgroundColor = parentBackground.color;
        selectedBackgroundColor = new Color(defaultBackgroundColor.r, defaultBackgroundColor.g, defaultBackgroundColor.b, selectedAlpha);

        toggleButton = GetComponent<Toggle>();
        toggleButton.onValueChanged.AddListener(delegate { ToggleButtonValue(toggleButton); });

    }

    private void ToggleButtonValue(Toggle toggle)
    {
        if (toggle.isOn)
        {
            OnSelected();
        }
        else
        {
            OnDeselected();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toggleButton.interactable)
        {
            StartHighlightTransition(highlightAmount);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toggleButton.interactable)
        {
            StartHighlightTransition(0f);
        }
    }

    private void StartHighlightTransition(float targetValue)
    {
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
        }
        highlightCoroutine = StartCoroutine(HighlightTransition(targetValue));
    }

    private IEnumerator HighlightTransition(float targetValue)
    {
        float startValue = outlineMaterial.GetFloat(_OutlineSize);
        float elapsedTime = 0f;

        while (elapsedTime < highlightDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / highlightDuration);
            outlineMaterial.SetFloat(_OutlineSize, Mathf.Lerp(startValue, targetValue, t));
            yield return null;
        }

        outlineMaterial.SetFloat(_OutlineSize, targetValue);
    }

    private void OnSelected()
    {
        parentBackground.color = selectedBackgroundColor;
        selectGroup.NewSelectedToggleButton(toggleButton);
        SelectSkin();
        clickSound.PlayClickSound();
    }

    private void OnDeselected()
    {
        parentBackground.color = defaultBackgroundColor;
    }

    private void SelectSkin()
    {
        switch (player)
        {
            case Player.Player1:
                SelectDragon.Player1Skin = dragonSkinData.skin;
                break;

            case Player.Player2:
                SelectDragon.Player2Skin = dragonSkinData.skin;
                break;
        }
    }
}

