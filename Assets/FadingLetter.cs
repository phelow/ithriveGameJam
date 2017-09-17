using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadingLetter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textMesh;

    enum FadeOutBehavior
    {
        PingPong,
        Blink,
        FadeOut,
        Jitter
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(FadeOutLetter());
    }

    private IEnumerator FadeOutLetter()
    {
        FadeOutBehavior randomBehavior;
        do
        {
            Array values = Enum.GetValues(typeof(FadeOutBehavior));
            System.Random random = new System.Random();
            randomBehavior = (FadeOutBehavior)values.GetValue(random.Next(values.Length));
            switch (randomBehavior)
            {
                case FadeOutBehavior.PingPong:
                    yield return PingPongRoutine();
                    break;
                case FadeOutBehavior.Blink:
                    yield return BlinkRoutine();
                    break;
                case FadeOutBehavior.Jitter:
                    yield return JitterRoutine();
                    break;
            }
        } while (randomBehavior != FadeOutBehavior.FadeOut);

        yield return FadeOutRoutine();

        Destroy(this.gameObject);
    }

    private IEnumerator JitterRoutine()
    {
        float tLeft = 1.0f;
        while (tLeft > 0.0f)
        {
            _textMesh.transform.position += new Vector3(
                UnityEngine.Random.RandomRange(-1.0f, 1.0f), 
                UnityEngine.Random.RandomRange(-1.0f, 1.0f), 
                0.0f);
            tLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BlinkRoutine()
    {
        float tLeft = 1.0f;
        while (tLeft > 0.0f)
        {
            tLeft -= .4f;
            _textMesh.alpha = 0.0f;
            yield return new WaitForSeconds(UnityEngine.Random.Range(.3f,.5f));
            _textMesh.alpha = .5f;
            yield return new WaitForSeconds(UnityEngine.Random.Range(.3f, .5f));
        }
    }

    private IEnumerator PingPongRoutine()
    {
        float tLeft = 1.0f;
        while (tLeft > 0.0f)
        {
            _textMesh.alpha = Mathf.Lerp(.5f, 0.0f, Mathf.PingPong(tLeft * 10.0f,1.0f));
            tLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        float tLeft = 1.0f;
        while (tLeft > 0.0f)
        {
            _textMesh.alpha = Mathf.Lerp(.5f, 0.0f, tLeft);
            tLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
