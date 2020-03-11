using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class YouDied : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;

    [SerializeField] float animationDuration = 6f;
    [SerializeField] private float fraction = 0f;

    public void PlayAnimation(UnityAction callback) => StartCoroutine(Play(callback));

    private IEnumerator Play(UnityAction callback)
    {
        text.gameObject.SetActive(true);

        fraction = 0f;

        while (fraction <= 1)
        {
            Color textColor = text.color;
            textColor.a = Mathf.Lerp(.5f, 1f, fraction);
            text.color = textColor;

            text.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.1f, 1.1f, 1.1f), fraction);

            fraction += Time.deltaTime / animationDuration;
            yield return new WaitForEndOfFrame();
        }

        callback?.Invoke();
    }

}
