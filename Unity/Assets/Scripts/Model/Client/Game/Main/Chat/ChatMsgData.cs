using System.Collections.Generic;

namespace ET.Client
{
    public struct QuoteData
    {
        public int Id;
        public string Name;
        public string Msg;
    }
    
    public struct AtData
    {
        public long Id;
        public string Name;
    }
    
    public struct ChatMsgData
    {
        public string Msg;
        public int Emjo;
        public QuoteData Quote;
        public List<AtData> AtList;
    }
}

