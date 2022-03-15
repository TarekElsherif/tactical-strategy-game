using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitVFX : AbstractUnitComponent
{
    [Header("References:")]
    [SerializeField] Outline _outline;
    [SerializeField] SkinnedMeshRenderer _mesh;
    [SerializeField] ParticleSystem _hitParticles;
    [SerializeField] ParticleSystem _shootParticles;

    [Header("Settings")]
    [SerializeField] Color _highlightColor;
    [SerializeField] Color _selectionColor;

    void OnEnable()
    {
        _unit.OnHighlight += Highlight;
        _unit.OnUnhighlight += Unhighlight;
        _unit.OnSelect += Select;
        _unit.OnUnselect += Unselect;
        _unit.OnDamageTaken += HitEffect;
        _unit.OnStartAttack += Shoot;
    }

    void OnDisable()
    {
        _unit.OnHighlight -= Highlight;
        _unit.OnUnhighlight -= Unhighlight;
        _unit.OnSelect -= Select;
        _unit.OnUnselect -= Unselect;
        _unit.OnDamageTaken -= HitEffect;
        _unit.OnStartAttack -= Shoot;
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
        _hitParticles.gameObject.SetActive(true);
        _hitParticles.Play();
        StartCoroutine(ApplyMaterialColorEffect(_mesh.material, 0.2f, Color.red));
    }

    void Shoot(UnitController unit)
    {
        _shootParticles.gameObject.SetActive(true);
        _shootParticles.Play();
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
