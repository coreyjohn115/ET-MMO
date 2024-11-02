using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ET.Client
{
    public class EditCustomFontWindow: EditorWindow
    {
        private Font font;
        private Texture2D tex;
        private string fontPath;
        private int num = 10;
        private Vector2Int size;
        private string fntFilePath;
        private TextAsset packCfg;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("字体：");
            font = (Font)EditorGUILayout.ObjectField(font, typeof (Font), true);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("字体图片：");
            tex = (Texture2D)EditorGUILayout.ObjectField(tex, typeof (Texture2D), true);
            if (tex)
            {
                size.x = tex.width / num;
                size.y = tex.height;
            }

            GUILayout.EndHorizontal();
            num = EditorGUILayout.IntField("数目", num);

            GUILayout.BeginHorizontal();
            GUILayout.Label("字体配置：");
            packCfg = (TextAsset)EditorGUILayout.ObjectField(packCfg, typeof (TextAsset), true);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            size = EditorGUILayout.Vector2IntField("size", size);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("更新文本"))
            {
                this.UpdateFont();
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void UpdateFont()
        {
            var path = AssetDatabase.GetAssetPath(font);
            var dir = Path.GetDirectoryName(path);
            var matPath = Path.Combine(dir, Path.GetFileNameWithoutExtension(path) + ".mat");
            List<object> cfgList = null;
            if (packCfg != null)
            {
                cfgList = MongoHelper.FromJson<List<object>>(packCfg.text);
            }

            var mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
            if (!mat)
            {
                mat = new Material(Shader.Find($"GUI/Text {nameof (Shader)}"));
                AssetDatabase.CreateAsset(mat, matPath);
            }

            mat.SetTexture(MainTex, tex);
            EditorUtility.SetDirty(mat);

            var list = new List<CharacterInfo>();
            var eachWidth = (float)tex.width / num;
            for (var i = 0; i < num; ++i)
            {
                var charInfo = new CharacterInfo();
                charInfo.index = i;
                charInfo.uvBottomLeft = new Vector2(eachWidth * i / tex.width, 0);
                charInfo.uvTopRight = new Vector2(eachWidth * i / tex.width + eachWidth / tex.width, 1);

                charInfo.minX = 0;
                charInfo.minY = tex.height / 2;
                charInfo.maxX = (int)eachWidth;
                charInfo.maxY = -tex.height / 2;
                
                charInfo.advance = (int)eachWidth;
                int index = 0;
                if (cfgList != null && cfgList.Count > i && int.TryParse(cfgList[i].ToString(), out index)) charInfo.index = index;
                list.Add(charInfo);
            }

            font.material = mat;
            font.characterInfo = list.ToArray();
            EditorUtility.SetDirty(font);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("更新成功");
        }
    }
}