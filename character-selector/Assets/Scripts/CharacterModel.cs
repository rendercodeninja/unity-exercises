using DG.Tweening;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField] private string characterKey;
    [SerializeField] private Color skinColor = new(0.9f, 0.9f, 0.9f);
    #endregion

    #region Public Fields
    public string CharacterKey { get => characterKey; }
    public int PlayerIndex { get; set; }
    #endregion

    #region Private Fields
    private Material[] mMaterials;
    private Animator mAnimator;
    
    #endregion

    private void Awake()
    {
        //Get reference to the animator component
        mAnimator = GetComponent<Animator>();        
        //Get reference to the renderer
        var renderer = GetComponentInChildren<Renderer>();
        //Cache reference to all materials
        mMaterials = renderer.materials;
        //Apply skin color
        mMaterials[1].color = skinColor;
    }

    private void OnEnable()
    {
        //Reset effect to 0
        SetEffectValue(0.0f);

        //Set random idle clip
        mAnimator.Play(CharacterManager.GetNonRepeatingRandomClip(PlayerIndex));

        //Star the FadeIn
        StartFade(0.0f, 1.0f);
    }

    private void StartFade(float from, float to, float duration=0.5f)
    {
        //Compose float Tween
        DOTween.To((val) =>
        {
            SetEffectValue(val);

        }, from, to, duration)
            .SetEase(Ease.InOutQuad);
    }

    //Sets the effect value for all materials
    private void SetEffectValue(float value)
    {
        //Iterate through each material
        foreach (var mat in mMaterials)
        {
            //If valid property available
            if (mat.HasProperty("_Opacity"))
                mat.SetFloat("_Opacity", value);
        }
    }
}
