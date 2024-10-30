using System.IO;
using UnityEngine;
using YooAsset;

namespace ET.Client
{
    /// <summary>
    /// 资源文件流加载解密类
    /// </summary>
    public class FileStreamDecryption: IDecryptionServices
    {
        /// <summary>
        /// 同步方式获取解密的资源包对象
        /// 注意：加载流对象在资源包对象释放的时候会自动释放
        /// </summary>
        AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
        {
            BundleStream bundleStream = new(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            managedStream = bundleStream;
            return AssetBundle.LoadFromStream(bundleStream, fileInfo.ConentCRC, GetManagedReadBufferSize());
        }

        /// <summary>
        /// 异步方式获取解密的资源包对象
        /// 注意：加载流对象在资源包对象释放的时候会自动释放
        /// </summary>
        AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
        {
            BundleStream bundleStream = new(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            managedStream = bundleStream;
            return AssetBundle.LoadFromStreamAsync(bundleStream, fileInfo.ConentCRC, GetManagedReadBufferSize());
        }

        private static uint GetManagedReadBufferSize()
        {
            return 1024;
        }
    }

    /// <summary>
    /// 资源文件偏移加载解密类
    /// </summary>
    public class FileOffsetDecryption: IDecryptionServices
    {
        /// <summary>
        /// 同步方式获取解密的资源包对象
        /// 注意：加载流对象在资源包对象释放的时候会自动释放
        /// </summary>
        AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
        {
            managedStream = null;
            return AssetBundle.LoadFromFile(fileInfo.FileLoadPath, fileInfo.ConentCRC, GetFileOffset());
        }

        /// <summary>
        /// 异步方式获取解密的资源包对象
        /// 注意：加载流对象在资源包对象释放的时候会自动释放
        /// </summary>
        AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
        {
            managedStream = null;
            return AssetBundle.LoadFromFileAsync(fileInfo.FileLoadPath, fileInfo.ConentCRC, GetFileOffset());
        }

        private static ulong GetFileOffset()
        {
            return 32;
        }
    }

    /// <summary>
    /// 资源文件解密流
    /// </summary>
    public class BundleStream: FileStream
    {
        public const byte KEY = 64;

        public BundleStream(string path, FileMode mode, FileAccess access, FileShare share): base(path, mode, access, share)
        {
        }

        public BundleStream(string path, FileMode mode): base(path, mode)
        {
        }

        public override int Read(byte[] array, int offset, int count)
        {
            var index = base.Read(array, offset, count);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] ^= KEY;
            }

            return index;
        }
    }
}