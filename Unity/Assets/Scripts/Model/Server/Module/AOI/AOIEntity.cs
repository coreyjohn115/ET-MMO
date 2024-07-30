using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Unity.Mathematics;

namespace ET.Server
{
    [ComponentOf(typeof (Unit))]
    public class AOIEntity: Entity, IAwake<int, float3>, IDestroy
    {
        /// <summary>
        /// 父物体
        /// </summary>
        [BsonIgnore]
        public Unit Unit => this.GetParent<Unit>();

        public int ViewDistance;

        private EntityRef<Cell> cell;

        [BsonIgnore]
        public Cell Cell
        {
            get
            {
                return this.cell;
            }
            set
            {
                this.cell = value;
            }
        }

        // 观察进入视野的Cell
        public HashSet<long> SubEnterCells = new();

        // 观察离开视野的Cell
        public HashSet<long> SubLeaveCells = new();

        // 观察进入视野的Cell
        public HashSet<long> enterHashSet = new();

        // 观察离开视野的Cell
        public HashSet<long> leaveHashSet = new();

        public List<long> SeeUnitList => this.SeeUnits.Keys.ToList();

        // 我看的见的Unit
        public Dictionary<long, EntityRef<Unit>> SeeUnits = new ();

        // 看见我的Unit
        public Dictionary<long, EntityRef<Unit>> BeSeeUnits = new ();

        // 我看的见的Player
        public Dictionary<long, EntityRef<Unit>> SeePlayers = new ();

        // 看见我的Player单独放一个Dict，用于广播
        public Dictionary<long, EntityRef<Unit>> BeSeePlayers = new ();
    }
}