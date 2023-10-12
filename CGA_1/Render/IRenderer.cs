

using System.Reflection;

namespace CGA_1.Render
{
    public interface IRenderer
    {
        RenderInfo RenderContext { get; set; }
        IPainter Painter { get; set; }
        int[] Render();
    }
}
