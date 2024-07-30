using System;
using UnityEditor;
using UnityEngine;

public class S_SceneDiffuse_GUI: S_SceneBaseGUI
{
    public enum DiffuseType
    {
        diffuse,
        illumin_diffuse
    }

    private bool outline_Foldout = true;

    private MaterialProperty diffuseTypeProp = null;
    private MaterialProperty outlineColor = null;
    private MaterialProperty outlineLength = null;
    private MaterialProperty outlineLightness = null;
    private MaterialProperty outlineCameraVector = null;

    protected override void FindProperties(MaterialProperty[] properties)
    {
        base.FindProperties(properties);
        diffuseTypeProp = FindProperty("_DiffuseType", properties);
        outlineColor = FindProperty("_OutlineColor", properties);
        outlineLength = FindProperty("_OutlineLength", properties);
        outlineLightness = FindProperty("_OutlineLightness", properties);
        outlineCameraVector = FindProperty("_OutlineCameraVector", properties);
    }

    private void DoOutlineArea(Material mat)
    {
        outline_Foldout = Foldout(outline_Foldout, "描边");
        if (outline_Foldout)
        {
            EditorGUI.indentLevel++;
            m_MaterialEditor.ShaderProperty(outlineColor, "描边颜色");
            m_MaterialEditor.ShaderProperty(outlineLength, "描边宽度");
            m_MaterialEditor.ShaderProperty(outlineLightness, "描边颜色强度");
            m_MaterialEditor.ShaderProperty(outlineCameraVector, "描边摄像机参数");
            EditorGUI.indentLevel--;
        }
    }

    protected override void ShaderProptertiesGUI(Material mat)
    {
        base.ShaderProptertiesGUI(mat);

        EditorGUI.BeginChangeCheck();
        var diffuseType = (DiffuseType)EditorGUILayout.Popup("Diffuse 类型", (int)diffuseTypeProp.floatValue, Enum.GetNames(typeof (DiffuseType)));
        EditorGUILayout.Space();
        DoAlbedoArea(mat);
        ;
        EditorGUILayout.Space();
        DoEmissMapArea(mat);
        EditorGUILayout.Space();
        DoOutlineArea(mat);
        EditorGUILayout.Space();
        if (EditorGUI.EndChangeCheck())
        {
            MaterialChanged(mat);
            m_MaterialEditor.RegisterPropertyChangeUndo("Diffuse 类型");
            diffuseTypeProp.floatValue = (float)diffuseType;
        }

        EditorGUI.showMixedValue = false;
        EditorGUILayout.Space();

        GUILayout.Label(Styles.advancedText, EditorStyles.boldLabel);
        m_MaterialEditor.RenderQueueField();
        m_MaterialEditor.EnableInstancingField();
        m_MaterialEditor.DoubleSidedGIField();
    }
}