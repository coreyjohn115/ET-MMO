using UnityEditor;
using UnityEngine;

public class S_SceneBaseGUI: S_Base_GUI
{
    protected static class Styles
    {
        public static GUIContent albedoText = EditorGUIUtility.TrTextContent("主贴图", "漫反射主贴图");
        public static GUIContent emissMapText = EditorGUIUtility.TrTextContent("自发光贴图", "自发光贴图(r:自发光部分 g:内部Mask b:扰动)");

        public static string advancedText = "Advanced Options";
    }

    private MaterialProperty color = null;
    private MaterialProperty mainTex = null;
    private MaterialProperty emissMap = null;
    private MaterialProperty emissColor = null;
    private MaterialProperty emissMaskSpeedX = null;
    private MaterialProperty emissMaskSpeedY = null;
    private MaterialProperty emissNoiseFactor = null;
    private MaterialProperty emissNoiseStrength = null;
    private MaterialProperty emissBreathSpeed = null;
    private MaterialProperty emissBreathMap = null;

    private bool mainTex_Foldout = true;
    private bool emissMap_Foldout = true;
    private static readonly int EmissMap = Shader.PropertyToID("_EmissMap");

    protected override void FindProperties(MaterialProperty[] properties)
    {
        base.FindProperties(properties);
        color = FindProperty("_Color", properties);
        mainTex = FindProperty("_MainTex", properties);
        emissMap = FindProperty("_EmissMap", properties);
        emissColor = FindProperty("_EmissColor", properties);
        emissMaskSpeedX = FindProperty("_EmissMaskSpeedX", properties);
        emissMaskSpeedY = FindProperty("_EmissMaskSpeedY", properties);
        emissNoiseFactor = FindProperty("_EmissNoiseFactor", properties);
        emissNoiseStrength = FindProperty("_EmissNoiseStrength", properties);
        emissBreathSpeed = FindProperty("_EmissBreathSpeed", properties);
        emissBreathMap = FindProperty("_EmissBreathMap", properties);
    }

    protected override void MaterialChanged(Material mat)
    {
        base.MaterialChanged(mat);
        var texture = mat.GetTexture("_EmissMap");
        SetKeyword(mat, "EmissMap_ON", texture != null);
        var breathMap = mat.GetTexture("_EmissBreathMap");
        SetKeyword(mat, "EmissBreathMap_ON", breathMap != null);
    }

    protected virtual void DoAlbedoArea(Material mat)
    {
        mainTex_Foldout = Foldout(mainTex_Foldout, "主贴图");
        if (!this.mainTex_Foldout)
        {
            return;
        }

        EditorGUI.indentLevel++;
        this.m_MaterialEditor.TexturePropertySingleLine(Styles.albedoText, this.mainTex, this.color);
        if (mat.GetTexture("_MainTex") != null)
        {
            this.m_MaterialEditor.TextureScaleOffsetProperty(this.mainTex);
        }

        this.DoAlbedoArea_Add(mat);
        EditorGUI.indentLevel--;
    }

    protected virtual void DoAlbedoArea_Add(Material mat)
    {
    }

    protected virtual void DoEmissMapArea(Material mat)
    {
        emissMap_Foldout = Foldout(emissMap_Foldout, "自发光贴图(r:自发光部分 g:自发光Mask b:扰动)");
        if (!this.emissMap_Foldout)
        {
            return;
        }

        EditorGUI.indentLevel++;
        this.m_MaterialEditor.TexturePropertySingleLine(Styles.emissMapText, this.emissMap, this.emissColor);
        if (mat.GetTexture(EmissMap) != null)
        {
            this.m_MaterialEditor.TextureScaleOffsetProperty(this.emissMap);
            this.m_MaterialEditor.ShaderProperty(this.emissMaskSpeedX, "Mask X方向速度");
            this.m_MaterialEditor.ShaderProperty(this.emissMaskSpeedY, "Mask Y方向速度");
            this.m_MaterialEditor.ShaderProperty(this.emissNoiseFactor, "扰动时间参数");
            this.m_MaterialEditor.ShaderProperty(this.emissNoiseStrength, "扰动强度");
            this.m_MaterialEditor.ShaderProperty(this.emissBreathSpeed, "呼吸速度");
            this.m_MaterialEditor.ShaderProperty(this.emissBreathMap, "呼吸噪声图");
        }

        EditorGUI.indentLevel--;
    }
}