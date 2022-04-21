using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static CRTPostEffecter;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(CRTPostEffecter))]
public class CRTPostEffecterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CRTPostEffecter effect = target as CRTPostEffecter;
        effect.material = (Material)EditorGUILayout.ObjectField("Effect Material", effect.material, typeof(Material), false);

        using (new HorizontalScope(GUI.skin.box))
        {
            effect.whiteNoiseFrequency = EditorGUILayout.IntField("White Noise Freaquency (x/1000)", effect.whiteNoiseFrequency);
            effect.whiteNoiseLength = EditorGUILayout.FloatField("White Noise Time Left (sec)", effect.whiteNoiseLength);
        }
        using (new VerticalScope(GUI.skin.box))
        {
            effect.screenJumpFrequency = EditorGUILayout.IntField("Screen Jump Freaquency (x/1000)", effect.screenJumpFrequency);
            effect.screenJumpLength = EditorGUILayout.FloatField("Screen Jump Length", effect.screenJumpLength);
            using (new HorizontalScope())
            {
                effect.screenJumpMinLevel = EditorGUILayout.FloatField("min", effect.screenJumpMinLevel);
                effect.screenJumpMaxLevel = EditorGUILayout.FloatField("max", effect.screenJumpMaxLevel);
            }
        }
        using (new HorizontalScope(GUI.skin.box))
        {
            effect.isScanline = EditorGUILayout.Toggle("Scanline On / Off", effect.isScanline);
        }

        using (new HorizontalScope(GUI.skin.box))
        {
            effect.isMonochrome = EditorGUILayout.Toggle("Monochrome On / Off", effect.isMonochrome);
        }

        using (new HorizontalScope(GUI.skin.box))
        {
            effect.flickeringStrength = EditorGUILayout.FloatField("Flickering Strength", effect.flickeringStrength);
            effect.flickeringCycle = EditorGUILayout.FloatField("Flickering Cycle", effect.flickeringCycle);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isSlippage = EditorGUILayout.Toggle("Slippage On / Off", effect.isSlippage);
            effect.isSlippageNoise = EditorGUILayout.Toggle("Slippage Noise", effect.isSlippageNoise);
            effect.slippageStrength = EditorGUILayout.FloatField("Slippage Strength", effect.slippageStrength);
            effect.slippageInterval = EditorGUILayout.FloatField("Slippage Interval", effect.slippageInterval);
            effect.slippageScrollSpeed = EditorGUILayout.FloatField("Slippage Scroll Speed", effect.slippageScrollSpeed);
            effect.slippageSize = EditorGUILayout.FloatField("Slippage Size", effect.slippageSize);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isChromaticAberration = EditorGUILayout.Toggle("Chromatic Aberration On / Off", effect.isChromaticAberration);
            effect.chromaticAberrationStrength = EditorGUILayout.FloatField("Chromatic Aberration Strength", effect.chromaticAberrationStrength);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isMultipleGhost = EditorGUILayout.Toggle("Multiple Ghost On / Off", effect.isMultipleGhost);
            effect.multipleGhostStrength = EditorGUILayout.FloatField("Multiple Ghost Strength", effect.multipleGhostStrength);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isLetterBox = EditorGUILayout.Toggle("Letter Box On / Off", effect.isLetterBox);
            effect.letterBoxType = (LeterBoxType)EditorGUILayout.EnumPopup("Letter Box Type", effect.letterBoxType);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isDecalTex = EditorGUILayout.Toggle("Decal Tex On / Off", effect.isDecalTex);
            effect.decalTex = (Texture2D)EditorGUILayout.ObjectField("Decal Tex", effect.decalTex, typeof(Texture2D), false);
            effect.decalTexPos = EditorGUILayout.Vector2Field("Decal Tex Position", effect.decalTexPos);
            effect.decalTexScale = EditorGUILayout.Vector2Field("Decal Tex Scale", effect.decalTexScale);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isLowResolution = EditorGUILayout.Toggle("Low Resolution", effect.isLowResolution);
            //effect.resolutions = EditorGUILayout.Vector2IntField("Resolutions", effect.resolutions);
        }

        using (new VerticalScope(GUI.skin.box))
        {
            effect.isFilmDirt = EditorGUILayout.Toggle("Film Dirt", effect.isFilmDirt);
            effect.filmDirtTex = (Texture2D)EditorGUILayout.ObjectField("Film Dirt Tex", effect.filmDirtTex, typeof(Texture2D), false);
        }

        EditorUtility.SetDirty(target);
    }
}
