using UnityEditor;
using UnityEngine;

public class S_Base_GUI: ShaderGUI
{
    protected MaterialEditor m_MaterialEditor;
    bool m_FirstTimeApply = true;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        m_MaterialEditor = materialEditor;
        FindProperties(properties);
        Material material = materialEditor.target as Material;
        if (m_FirstTimeApply)
        {
            MaterialChanged(material);
            m_FirstTimeApply = false;
        }

        ShaderProptertiesGUI(material);
    }

    protected virtual void FindProperties(MaterialProperty[] properties)
    {
    }

    protected virtual void MaterialChanged(Material mat)
    {
    }

    protected virtual void ShaderProptertiesGUI(Material mat)
    {
    }

    protected static bool Foldout(bool display, string title)
    {
        var style = new GUIStyle("ShurikenModuleTitle");
        style.font = new GUIStyle(EditorStyles.boldLabel).font;
        style.border = new RectOffset(15, 7, 4, 4);
        style.fixedHeight = 22;
        style.contentOffset = new Vector2(20f, -2f);

        var rect = GUILayoutUtility.GetRect(16f, 22f, style);
        GUI.Box(rect, title, style);

        var e = Event.current;

        var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
        if (e.type == EventType.Repaint)
        {
            EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
        }

        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            display = !display;
            e.Use();
        }

        return display;
    }

    protected static void SetKeyword(Material mat, string keyword, bool state)
    {
        if (state) mat.EnableKeyword(keyword);
        else mat.DisableKeyword(keyword);
    }
}