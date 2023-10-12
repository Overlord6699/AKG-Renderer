using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CGA_1.Render
{

    public class ScreenBuffer
    {
        private const int MAX_SIZE = 65535; // width * height

        public int[] Screen { get; }
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; set; } = MAX_SIZE;

        private readonly RenderInfo _context;

        // для перетирания
        private readonly int[] _emptyZBuffer;
        private readonly int[] _emptyBuffer;

        private float _widthCenter { get; }
        private float _heightCenter { get; }

        private int[] _zBuffer;

        public Vector3 ToScreen3(Vector4 p)
        {
            return new Vector3(
                _widthCenter * (p.X / p.W + 1), 
                -_heightCenter * (p.Y / p.W - 1), 
                Depth * p.Z / p.W);
        }

        public ScreenBuffer(int width, int height, RenderInfo context)
        {
            this.Screen = new int[width * height];
            this._zBuffer = new int[width * height];

            this._emptyBuffer = new int[width * height];
            this._emptyZBuffer = new int[width * height];
            // внимание
            for(int i =0; i < _emptyZBuffer.Length; i++)
                _emptyZBuffer[i] = Depth;

            this.Width = width;
            this.Height = height;
            this._widthCenter = (width - 1) / 2f;
            this._heightCenter = (height - 1) / 2f;

            this._context = context;
        }

        public void Clear()
        {
            Array.Copy(_emptyBuffer, Screen, Screen.Length);
            Array.Copy(_emptyZBuffer, _zBuffer, _zBuffer.Length);
        }

        // пиксельная отрисовка
        public void PutPixel(in int x, in int y, in int z, ColorRGB color)
        {
            var index = x + y * Width;

            if (z > _zBuffer[index])
            {
                _context.Stats.BehindZPixelCount++;
                return;
            }

            _context.Stats.DrawnPixelCount++;

            _zBuffer[index] = z;

            Screen[index] = color.Color;
        }

        //своя линия 
        public void DrawLine(Vector3 p0, Vector3 p1, ColorRGB color)
        {

            var x0 = (int)p0.X; var y0 = (int)p0.Y; var z0 = (int)p0.Z;
            var x1 = (int)p1.X; var y1 = (int)p1.Y; var z1 = (int)p1.Z;

            var dx = Math.Abs(x1 - x0); var dy = Math.Abs(y1 - y0); var dz = Math.Abs(z1 - z0);

            var sx = x0 < x1 ? 1 : -1; var sy = y0 < y1 ? 1 : -1; var sz = z0 < z1 ? 1 : -1;

            var ex = 0; var ey = 0; var ez = 0;

            var dmax = Math.Max(dx, dy);

            int i = 0;
            while (i++ < dmax)
            {
                ex += dx; if (ex >= dmax) { ex -= dmax; x0 += sx; PutPixel(x0, y0, z0, color); }
                ey += dy; if (ey >= dmax) { ey -= dmax; y0 += sy; PutPixel(x0, y0, z0, color); }
                ez += dz; if (ez >= dmax) { ez -= dmax; z0 += sz; PutPixel(x0, y0, z0, color); }
            }
        }
    }
}