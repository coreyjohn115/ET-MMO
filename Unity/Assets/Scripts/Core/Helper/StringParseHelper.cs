using System;
using System.Collections.Generic;

namespace ET
{
    public class CmdArgs
    {
        public string Cmd { get; set; }

        public List<long> Args { get; set; }
    }

    public class ItemArgs
    {
        public int Id;
        
        public long Count;

        public bool Bind;
    }

    public class AttrArgs
    {
        public int AttrType;

        public long AttrValue;
    }
    
    public class WeightArgs
    {
        public int Id;

        public int Weight;
    }

    public static class StringParseHelper
    {
        public static List<CmdArgs> ParseCmdArgs(this string cmdStr)
        {
            List<CmdArgs> cmdArgs = new List<CmdArgs>();
            if (string.IsNullOrEmpty(cmdStr))
            {
                return cmdArgs;
            }

            var ll1 = cmdStr.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in ll1)
            {
                CmdArgs args = new CmdArgs();
                string[] ll2 = s.Split(':', StringSplitOptions.RemoveEmptyEntries);
                args.Cmd = ll2[0];
                args.Args = new List<long>();
                for (int i = 1; i < ll2.Length; i++)
                {
                    long.TryParse(ll2[i], out long v);
                    args.Args.Add(v);
                }

                cmdArgs.Add(args);
            }

            return cmdArgs;
        }

        public static List<ItemArgs> ParseItemArgs(this string cmdStr)
        {
            List<ItemArgs> itemArgs = new List<ItemArgs>();
            if (string.IsNullOrEmpty(cmdStr))
            {
                return itemArgs;
            }

            var ll1 = cmdStr.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in ll1)
            {
                ItemArgs args = new ItemArgs();
                string[] ll2 = s.Split(':', StringSplitOptions.RemoveEmptyEntries);
                args.Id = Convert.ToInt32(ll2[0]);
                args.Count = ll2.Length > 1? Convert.ToInt64(ll2[1]) : 0;
                args.Bind = ll2.Length > 2 && Convert.ToBoolean(ll2[2]);
                itemArgs.Add(args);
            }

            return itemArgs;
        }

        public static List<AttrArgs> ParseAttrArgs(this string attrStr)
        {
            List<AttrArgs> attrArgs = new List<AttrArgs>();
            if (string.IsNullOrEmpty(attrStr))
            {
                return attrArgs;
            }

            var ll1 = attrStr.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in ll1)
            {
                AttrArgs args = new AttrArgs();
                string[] ll2 = s.Split(':', StringSplitOptions.RemoveEmptyEntries);
                args.AttrType = Convert.ToInt32(ll2[0]);
                args.AttrValue = ll2.Length > 1? Convert.ToInt64(ll2[1]) : 0;
                attrArgs.Add(args);
            }

            return attrArgs;
        }
        
        public static List<WeightArgs> ParseWeightArgs(this string weightStr)
        {
            List<WeightArgs> weightArgs = new List<WeightArgs>();
            if (string.IsNullOrEmpty(weightStr))
            {
                return weightArgs;
            }

            var ll1 = weightStr.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in ll1)
            {
                WeightArgs args = new();
                string[] ll2 = s.Split(':', StringSplitOptions.RemoveEmptyEntries);
                args.Id = Convert.ToInt32(ll2[0]);
                args.Weight = ll2.Length > 1? Convert.ToInt32(ll2[1]) : 0;
                weightArgs.Add(args);
            }

            return weightArgs;
        }
    }
}