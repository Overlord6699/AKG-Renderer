using System;
using System.Collections.Generic;
using System.Numerics;

namespace CGA_1
{

    class ObjModel
    { 
        public List<Vector3> Vertices { get; } = new List<Vector3>();
        public List<Tuple<Vector3, Vector3>> Edges { get; } = new List<Tuple<Vector3, Vector3>>();
    }
}
