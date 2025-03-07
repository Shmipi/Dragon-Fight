using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    [SerializeField] private int startCount = 10;
    [SerializeField] private UnityEvent onCountdownFinished;
    private TextMeshProUGUI textMeshPro;
    private Animator animator;
    private readonly int animationID = Animator.StringToHash("ShowNumber");
    private Coroutine countdownCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        StartCountDown();
    }

    private IEnumerator CountdownTimer()
    {
        int currentCount = startCount;

        while (currentCount > 0)
        {
            animator.Play(animationID);
            textMeshPro.text = currentCount.ToString();
            yield return new WaitForSeconds(1f);
            currentCount--;
        }

        yield return null;
        onCountdownFinished?.Invoke();
        countdownCoroutine = null;
    }

    public void StartCountDown()
    {
        if (countdownCoroutine == null)
        {
            countdownCoroutine = StartCoroutine(CountdownTimer());
        }
    }

    
}
