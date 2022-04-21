using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class CRTPostEffecter : MonoBehaviour
{
    public Material material;
    public int whiteNoiseFrequency = 1;
    public float whiteNoiseLength = 0.1f;
    private float whiteNoiseTimeLeft;

    public int screenJumpFrequency = 1;
    public float screenJumpLength = 0.2f;
    public float screenJumpMinLevel = 0.1f;
    public float screenJumpMaxLevel = 0.9f;
    private float screenJumpTimeLeft;

    public float flickeringStrength = 0.002f;
    public float flickeringCycle = 111f;

    public bool isSlippage = true;
    public bool isSlippageNoise = true;
    public float slippageStrength = 0.005f;
    public float slippageInterval = 1f;
    public float slippageScrollSpeed = 33f;
    public float slippageSize = 11f;

    public float chromaticAberrationStrength = 0.005f;
    public bool isChromaticAberration = true;

    public bool isMultipleGhost = true;
    public float multipleGhostStrength = 0.01f;

    public bool isScanline = true;
    public bool isMonochrome = false;

    public bool isLetterBox = false;
    public bool isLetterBoxEdgeBlur = false;
    public LeterBoxType letterBoxType;
    public enum LeterBoxType
    {
        Black,
        Blur
    }

    public bool isFilmDirt = false;
    public Texture2D filmDirtTex;

    public bool isDecalTex = false;
    public Texture2D decalTex;
    public Vector2 decalTexPos;
    public Vector2 decalTexScale;

    public bool isLowResolution = true;
    public Vector2Int resolutions;

    #region Properties in shader
    private int _WhiteNoiseOnOff;
    private int _ScanlineOnOff;
    private int _MonochormeOnOff;
    private int _ScreenJumpLevel;
    private int _FlickeringStrength;
    private int _FlickeringCycle;
    private int _SlippageStrength;
    private int _SlippageSize;
    private int _SlippageInterval;
    private int _SlippageScrollSpeed;
    private int _SlippageNoiseOnOff;
    private int _SlippageOnOff;
    private int _ChromaticAberrationStrength;
    private int _ChromaticAberrationOnOff;
    private int _MultipleGhostOnOff;
    private int _MultipleGhostStrength;
    private int _LetterBoxOnOff;
    private int _LetterBoxType;
    private int _LetterBoxEdgeBlurOnOff;
    private int _DecalTex;
    private int _DecalTexOnOff;
    private int _DecalTexPos;
    private int _DecalTexScale;
    private int _FilmDirtOnOff;
    private int _FilmDirtTex;
    #endregion

    private void Start()
    {
        _WhiteNoiseOnOff = Shader.PropertyToID("_WhiteNoiseOnOff");
        _ScanlineOnOff = Shader.PropertyToID("_ScanlineOnOff");
        _MonochormeOnOff = Shader.PropertyToID("_MonochormeOnOff");
        _ScreenJumpLevel = Shader.PropertyToID("_ScreenJumpLevel");
        _FlickeringStrength = Shader.PropertyToID("_FlickeringStrength");
        _FlickeringCycle = Shader.PropertyToID("_FlickeringCycle");
        _SlippageStrength = Shader.PropertyToID("_SlippageStrength");
        _SlippageSize = Shader.PropertyToID("_SlippageSize");
        _SlippageInterval = Shader.PropertyToID("_SlippageInterval");
        _SlippageScrollSpeed = Shader.PropertyToID("_SlippageScrollSpeed");
        _SlippageNoiseOnOff = Shader.PropertyToID("_SlippageNoiseOnOff");
        _SlippageOnOff = Shader.PropertyToID("_SlippageOnOff");
        _ChromaticAberrationStrength = Shader.PropertyToID("_ChromaticAberrationStrength");
        _ChromaticAberrationOnOff = Shader.PropertyToID("_ChromaticAberrationOnOff");
        _MultipleGhostOnOff = Shader.PropertyToID("_MultipleGhostOnOff");
        _MultipleGhostStrength = Shader.PropertyToID("_MultipleGhostStrength");
        _LetterBoxOnOff = Shader.PropertyToID("_LetterBoxOnOff");
        _LetterBoxType = Shader.PropertyToID("_LetterBoxType");
        _DecalTex = Shader.PropertyToID("_DecalTex");
        _DecalTexOnOff = Shader.PropertyToID("_DecalTexOnOff");
        _DecalTexPos = Shader.PropertyToID("_DecalTexPos");
        _DecalTexScale = Shader.PropertyToID("_DecalTexScale");
        _FilmDirtOnOff = Shader.PropertyToID("_FilmDirtOnOff");
        _FilmDirtTex = Shader.PropertyToID("_FilmDirtTex");
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        ///////White noise
        whiteNoiseTimeLeft -= 0.01f;
        if (whiteNoiseTimeLeft <= 0)
        {
            if (Random.Range(0, 1000) < whiteNoiseFrequency)
            {
                material.SetInteger(_WhiteNoiseOnOff, 1);
                whiteNoiseTimeLeft = whiteNoiseLength;
            }
            else
            {
                material.SetInteger(_WhiteNoiseOnOff, 0); 
            }
        }
        //////
        
        material.SetInteger(_LetterBoxOnOff, isLetterBox ? 0 : 1); 
        //material.SetInteger(_LetterBoxEdgeBlurOnOff, isLetterBoxEdgeBlur ? 0 : 1); 
        material.SetInteger(_LetterBoxType, (int)letterBoxType);

        material.SetInteger(_ScanlineOnOff, isScanline ? 1 : 0); 
        material.SetInteger(_MonochormeOnOff, isMonochrome ? 1 : 0);
        material.SetFloat(_FlickeringStrength, flickeringStrength);
        material.SetFloat(_FlickeringCycle, flickeringCycle);
        material.SetFloat(_ChromaticAberrationStrength, chromaticAberrationStrength);
        material.SetInteger(_ChromaticAberrationOnOff, isChromaticAberration ? 1 : 0);
        material.SetInteger(_MultipleGhostOnOff, isMultipleGhost ? 1 : 0);
        material.SetFloat(_MultipleGhostStrength, multipleGhostStrength); 
        material.SetInteger(_FilmDirtOnOff, isFilmDirt ? 1 : 0);
        material.SetTexture(_FilmDirtTex, filmDirtTex);

        //////Slippage
        material.SetInteger(_SlippageOnOff, isSlippage ? 1 : 0);
        material.SetFloat(_SlippageInterval, slippageInterval);
        material.SetFloat(_SlippageNoiseOnOff, isSlippageNoise ? Random.Range(0, 1f) : 1);
        material.SetFloat(_SlippageScrollSpeed, slippageScrollSpeed);
        material.SetFloat(_SlippageStrength, slippageStrength); 
        material.SetFloat(_SlippageSize, slippageSize);
        //////
        
        //////Screen Jump Noise
        screenJumpTimeLeft -= 0.01f;
        if (screenJumpTimeLeft <= 0)
        {
            if (Random.Range(0, 1000) < screenJumpFrequency)
            {
                var level = Random.Range(screenJumpMinLevel, screenJumpMaxLevel);
                material.SetFloat(_ScreenJumpLevel, level);
                screenJumpTimeLeft = screenJumpLength;
            }
            else
            {
                material.SetFloat(_ScreenJumpLevel, 0);
            }
        }
        //////

        //////Decal Texture
        material.SetTexture(_DecalTex, decalTex);
        material.SetInteger(_DecalTexOnOff, isDecalTex ? 1 : 0);
        material.SetVector(_DecalTexPos, decalTexPos);
        material.SetVector(_DecalTexScale, decalTexScale);
        //////

        //////Low resolution
        if (isLowResolution)
        {
            var target = RenderTexture.GetTemporary(src.width / 2, src.height / 2);
            Graphics.Blit(src, target);
            Graphics.Blit(target, dest, material);
            RenderTexture.ReleaseTemporary(target);
        }
        else
        {
            Graphics.Blit(src, dest, material);
        }
        //////

    }
}