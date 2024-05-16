using System.Collections.Generic;
using UnityEngine;

namespace Brushes
{
    public class SingleBrush : Brush
    {
        private bool _using;
        
        /*public override List<Vector3Int> Initiate(Vector3Int position)
        {
            if (_using)
                return default;

            _using = true;
            return new List<Vector3Int>() { position };
        }

        public override void Holding()
        {
            
        }

        public override List<Vector3Int> Release(Vector3Int position)
        {
            _using = false;
            return default;
        }*/
    }
}