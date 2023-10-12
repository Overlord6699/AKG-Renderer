using CGA_1.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGA_1.Drawer
{
    public interface IDrawer
    {
        RenderInfo RendererContext { get; set; }
        void Draw(ColorRGB color, VertexBuffer vbx, int indice);
    }
}
