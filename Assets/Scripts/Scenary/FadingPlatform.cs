using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
    [Header("<color=purple>Values</color>")]
    [SerializeField] private float _fadingTime = 3f;
    [SerializeField] private float _interval = 2f;
    [SerializeField] private float _respawnTime = 3f;

    private Color _ogColor;

    private Collider _collider;
    private Material _mat;

    private bool _hasBeenActivated;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _mat = GetComponent<Renderer>().material;

        _ogColor = _mat.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Player>() && !_hasBeenActivated)
        {
            StartCoroutine(FadeProcess());
        }
    }

    private IEnumerator FadeProcess()
    {
        _hasBeenActivated = true;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / _fadingTime;
            _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, Mathf.Lerp(1f, 0f, t));

            yield return null;
        }

        _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, 0f);
        _collider.enabled = false;

        yield return new WaitForSeconds(_interval);

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / _respawnTime;
            _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, Mathf.Lerp(0f, 1f, t));

            yield return null;
        }

        _mat.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, 1f);
        _collider.enabled = true;

        _hasBeenActivated = false;
    }
}
