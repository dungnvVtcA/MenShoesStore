using System;
namespace MSS_Persistence
{
    [Serializable]
    public class Trademark
    {
        public int Trademark_id{set;get;}

        public string Name{set;get;}

        public string Origin{set;get;}

        public Trademark(){}
    }
}