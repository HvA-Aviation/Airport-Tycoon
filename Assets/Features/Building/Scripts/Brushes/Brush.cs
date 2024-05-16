using System.Collections.Generic;
using UnityEngine;

namespace Brushes
{
    public abstract class Brush
    {
        protected Vector3Int _origin;
        private bool _holding;

        public void Press(Vector3Int position)
        {
            _origin = position;
            _holding = true;
        }

        public abstract void Holding();

        public void Release(Vector3Int position)
        {
            _origin = position;
            _holding = false;
        }
    }
}