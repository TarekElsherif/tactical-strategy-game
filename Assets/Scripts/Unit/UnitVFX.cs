using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class UnitVFX : MonoBehaviour
{
    UnitController _unit;

    [Header("References:")]
    [SerializeField] Outline _outline;
    [SerializeField] SkinnedMeshRenderer _mesh;

    [Header("Settings")]
    [SerializeField] Color _highlightColor;
    [SerializeField] Color _selectionColor;

    void Awake()
    {
        _unit = GetComponent<UnitController>();
    }

    void OnEnable()
    {
        _unit.OnHighlight += Highlight;
        _unit.OnUnhighlight += Unhighlight;
        _unit.OnSelect += Select;
        _unit.OnUnselect += Unselect;
        _unit.OnDamageTaken += HitEffect;
    }

    void OnDisable()
    {
        _unit.OnHighlight -= Highlight;
        _unit.OnUnhighlight -= Unhighlight;
        _unit.OnSelect -= Select;
        _unit.OnUnselect -= Unselect;
        _unit.OnDamageTaken -= HitEffect;
    }

    void Highlight()
    {
        _outline.enabled = true;
        _outline.OutlineColor = _highlightColor;
    }

    void Unhighlight()
    {
        _outline.enabled = false;
    }

    void Select()
    {
        _outline.OutlineColor = _selectionColor;
    }

    void Unselect()
    {
        _outline.OutlineColor = _highlightColor;
    }

    void HitEffect(float damage)
    {
        StartCoroutine(ApplyMaterialColorEffect(_mesh.material, 0.2f, Color.red));
    }

    IEnumerator ApplyMaterialColorEffect(Material mat, float fadeDuration, Color color)
    {
        float timer = 0;
        Color originalColor = mat.color;
        while (timer < fadeDuration)
        {
            float value = timer / fadeDuration;
            mat.color = Color.Lerp(originalColor, color, value);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timer = 0;
        while (timer < fadeDuration)
        {
            float value = timer / fadeDuration;
            mat.color = Color.Lerp(color, originalColor, value);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        mat.color = originalColor;
    }
}
