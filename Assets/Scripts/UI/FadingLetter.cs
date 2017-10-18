using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadingLetter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textMesh;

    private float _spookiness = 1.0f;

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
        //StartCoroutine(FadeOutLetter());
    }

    public void SetSpookiness(float spookiness)
    {
        _spookiness = spookiness;
    }

    private IEnumerator FadeOutLetter()
    {
        FadeOutBehavior randomBehavior;
        int iterations = 1;
        do
        {
            if (!TypeWriter.s_instance.IsTextPlaying())
            {
                break;
            }

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
                case FadeOutBehavior.FadeOut:
                    yield return RotateRoutine();
                    break;
            }

            iterations++;
        } while (iterations < Mathf.Min(_spookiness,5) * 3);

        yield return FadeOutRoutine();

        Destroy(this.gameObject);
    }

    private IEnumerator RotateRoutine()
    {
        float tPassed = 0.0f;
        float targetTime = UnityEngine.Random.Range(0.3f, 1.5f);
        Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(-360.0f, 360.0f));

        while (tPassed < targetTime)
        {
            if (!TypeWriter.s_instance.IsTextPlaying())
            {
                break;
            }
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, tPassed / targetTime);
            tPassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator JitterRoutine()
    {
        float tLeft = 1.0f;
        while (tLeft > 0.0f)
        {
            if (!TypeWriter.s_instance.IsTextPlaying())
            {
                break;
            }
            _textMesh.transform.position += new Vector3(
                UnityEngine.Random.Range(-_spookiness, _spookiness),
                UnityEngine.Random.Range(-_spookiness, _spookiness),
                0.0f);
            tLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator BlinkRoutine()
    {
        float tLeft = 1.0f * Mathf.Min(_spookiness,5.0f);
        while (tLeft > 0.0f)
        {
            if (!TypeWriter.s_instance.IsTextPlaying())
            {
                break;
            }
            tLeft -= .4f;
            _textMesh.alpha = 0.0f;
            yield return new WaitForSeconds(UnityEngine.Random.Range(.3f, .5f));
            _textMesh.alpha = .5f;
            yield return new WaitForSeconds(UnityEngine.Random.Range(.3f, .5f));
        }
    }

    private IEnumerator PingPongRoutine()
    {
        float tLeft = 1.0f * _spookiness;
        while (tLeft > 0.0f)
        {
            if (!TypeWriter.s_instance.IsTextPlaying())
            {
                break;
            }
            _textMesh.alpha = Mathf.Lerp(.5f, 0.0f, Mathf.PingPong(tLeft * _spookiness, 1.0f));
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
