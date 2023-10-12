using CGA_1.CameraInstances;

namespace CGA_1.Render
{
    public class RenderInfo
    {
        public ICamera Camera { get; set; }
        public IWorld World { get; set; }
        public IProjection Projection { get; set; }
        public RendererSettings RendererSettings { get; set; }
        public Stats Stats { get; set; }

        internal ScreenBuffer Surface { get; set; }
        internal WorldBuffer WorldBuffer { get; set; }
    }
}
