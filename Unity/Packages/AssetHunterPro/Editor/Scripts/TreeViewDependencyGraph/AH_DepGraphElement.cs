using HeurekaGames.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace HeurekaGames.AssetHunterPRO.BaseTreeviewImpl.DependencyGraph
{
    [System.Serializable]
    public class AH_DepGraphElement : TreeElement, ISerializationCallbackReceiver
    {
        #region Fields
        [SerializeField]
        private string relativePath;
        /*[SerializeField]
        private string assetName;*/
        [SerializeField]
        private Type assetType;
        private Texture icon;
        [SerializeField]
        private string assetTypeSerialized;
        #endregion

        #region Properties

        public string RelativePath
        {
            get
            {
                return relativePath;
            }
        }

        public string AssetName
        {
            get
            {
                return m_Name;
            }
        }

        public Type AssetType
        {
            get
            {
                return assetType;
            }
        }

        public Texture Icon
        {
            get
            {
                return icon;
            }
        }

        public string AssetTypeSerialized
        {
            get
            {
                return assetTypeSerialized;
            }
        }

        #endregion

        public AH_DepGraphElement(string name, int depth, int id, string relativepath) : base(name, depth, id)
        {
            this.relativePath = relativepath;
            var stringSplit = relativepath.Split('/');
            //this.assetName = stringSplit.Last();
            this.assetType = UnityEditor.AssetDatabase.GetMainAssetTypeAtPath(relativepath);
            if (this.assetType != null)
                this.assetTypeSerialized = Heureka_Serializer.SerializeType(assetType);
            this.icon = UnityEditor.EditorGUIUtility.ObjectContent(null, assetType).image;
        }

        #region Serialization callbacks
        //TODO Maybe we can store type infos in BuildInfoTreeView instead of on each individual element, might be performance heavy

        //Store serializable string so we can retrieve type after serialization
        public void OnBeforeSerialize()
        {
            if (assetType != null)
                assetTypeSerialized = Heureka_Serializer.SerializeType(assetType);
        }

        //Set type from serialized property
        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(AssetTypeSerialized))
            {
                this.assetType = Heureka_Serializer.DeSerializeType(AssetTypeSerialized);
            }
        }
        #endregion

    }
}