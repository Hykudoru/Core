using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectPool
{
    public static class GameObjectPoolManager
    {
        public static Dictionary<string, GameObjectPool> Pools { get; private set; }

        static GameObjectPoolManager()
        {
            Pools = new Dictionary<string, GameObjectPool>();
        }

        public static GameObjectPool FindPool(string gameObjectName)
        {
            return Pools[gameObjectName] as GameObjectPool;
        }
    }
}
