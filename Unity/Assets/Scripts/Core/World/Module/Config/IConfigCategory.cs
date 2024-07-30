using System.Collections.Generic;

namespace ET
{
    public interface IConfigCategory
    { 
        object GetConfig(int id);
    
        object GetAllConfig();
    }   
}