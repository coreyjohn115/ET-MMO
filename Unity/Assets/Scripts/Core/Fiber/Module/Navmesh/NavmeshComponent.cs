﻿using System;
using System.Collections.Generic;

namespace ET
{
    public class NavmeshComponent: Singleton<NavmeshComponent>, ISingletonAwake
    {
        public struct RecastFileLoader
        {
            public string Name { get; set; }
        }

        private readonly Dictionary<string, byte[]> navmeshDict = new();

        public void Awake()
        {
        }

        public byte[] Get(string name)
        {
            lock (this)
            {
                if (this.navmeshDict.TryGetValue(name, out byte[] bytes))
                {
                    return bytes;
                }

                byte[] buffer = EventSystem.Instance.Invoke<RecastFileLoader, byte[]>(new RecastFileLoader() { Name = name });
                if (buffer.Length == 0)
                {
                    throw new Exception($"no navmesh data: {name}");
                }

                this.navmeshDict[name] = buffer;
                return buffer;
            }
        }
    }
}