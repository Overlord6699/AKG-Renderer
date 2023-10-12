using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using CGA_1.CameraInstances;
using CGA_1.Utils;
using CGA_1.Render;

namespace CGA_1.FormComponents
{
    public partial class GraphicsGrid : UserControl
    {
        
        private RenderInfo renderContext { get; }

        private ICamera _camera;
        private IWorld world;
        private IProjection projection;
        private IRenderer renderer;
        private IPainter painter;

        private Bitmap bmp;

        string format = "Volumes:{0}\nTriangles:{1} - Back:{2} - Out:{3} - Behind:{4}\nPixels:{9} drawn:{5} - Z behind:{6}\nCalc time:{7} - Paint time:{8}";

        public RendererSettings RendererSettings
        {
            get => rendererSettings;
            set
            {
                if (PropertyChangedChecker.ValueChanged(ref rendererSettings, value))
                {
                    renderContext.RendererSettings = value;
                    rendererSettings = value;
                }
            }
        }
        public IWorld World
        {
            get => world;
            set
            {
                if (PropertyChangedChecker.ValueChanged(ref world, value))
                {
                    renderContext.World = world;
                    hookPaintEvent();
                }
            }
        }

        public IPainter Painter
        {
            get => painter;
            set
            {
                if (PropertyChangedChecker.ValueChanged(ref painter, value))
                {
                    if (painter != null)
                    {
                        painter.RendererContext = renderContext;
                    }
                    assign(renderer, painter);
                }
            }
        }

        public IRenderer Renderer
        {
            get => renderer;
            set
            {
                if (PropertyChangedChecker.ValueChanged(ref renderer, value))
                {
                    renderer.RenderContext = renderContext;
                    assign(renderer, painter);
                }
            }
        }

        public ICamera Camera
        {
            get => _camera;
            set
            {
                var oldCamera = _camera;

                if (PropertyChangedChecker.ValueChanged(ref _camera, value))
                {

                    if (oldCamera != null)
                        oldCamera.CameraChanged -= cameraChanged;

                    if (_camera != null)
                        _camera.CameraChanged += cameraChanged;

                    renderContext.Camera = value;
                    hookPaintEvent();
                }
            }
        }

        public IProjection Projection
        {
            get => projection;
            set
            {
                var prevProjection = projection;

                if (PropertyChangedChecker.ValueChanged(ref projection, value))
                {

                    if (prevProjection != null)
                        prevProjection.ProjectionChanged -= projectionChanged;

                    if (projection != null)
                        projection.ProjectionChanged += projectionChanged; ;

                    renderContext.Projection = value;
                    hookPaintEvent();
                }
            }
        }

        public GraphicsGrid()
        {
            InitializeComponent();

            renderContext = new RenderInfo();
            renderContext.Stats = new Stats();

            RendererSettings = new RendererSettings { BackFaceCulling = true };

            Renderer = new SimpleRenderer();
            Painter = new GouraudPainter();

            this.ResizeRedraw = true;

            this.Layout += GraphicsGrid_Layout;
        }

        void projectionChanged(object sender, EventArgs e) => this.Invalidate();
        void cameraChanged(object sender, EventArgs e) => this.Invalidate();

        void hookPaintEvent()
        {
            this.Paint -= Panel3D_Paint;
            if (_camera != null && world != null && projection != null)
            {
                this.Paint += Panel3D_Paint;
            }
        }
        void assign(IRenderer renderer, IPainter painter)
        {
            if (renderer == null)
                return;
            else
                renderer.Painter = painter;
        }

        StringBuilder sb = new StringBuilder();
        private RendererSettings rendererSettings;

        public int[] Render()
        {
            return renderer.Render();
        }

        void Panel3D_Paint(object sender, PaintEventArgs e)
        {

            var g = e.Graphics;

            // g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            // g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            // g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            buildFrame();
            g.DrawImage(bmp, Point.Empty);

            sb.Clear();
            sb.AppendFormat(format,
                world.Volumes.Count,
                renderContext.Stats.TotalTriangleCount,
                renderContext.Stats.FacingBackTriangleCount,
                renderContext.Stats.OutOfViewTriangleCount,
                renderContext.Stats.BehindViewTriangleCount,
                renderContext.Stats.DrawnPixelCount,
                renderContext.Stats.BehindZPixelCount,
                renderContext.Stats.CalculationTimeMs,
                renderContext.Stats.PainterTimeMs,
                renderContext.Stats.DrawnPixelCount + renderContext.Stats.BehindZPixelCount
            );

            TextRenderer.DrawText(g, sb.ToString(), this.Font, Point.Empty, Color.BlueViolet, this.BackColor, TextFormatFlags.ExpandTabs);
        }

        private void GraphicsGrid_Layout(object sender, LayoutEventArgs e)
        {
            if (this.Size.Height == 0 || this.Size.Width == 0)
                return;

            renderContext.Surface = new ScreenBuffer(this.Width, this.Height, renderContext);
            bmp = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppPArgb);
        }

        void buildFrame()
        {
            var buffer = renderer.Render();
            BitmapUtils.BuildBitmap(bmp, buffer, this.Width, this.Height);
        }
         
    }

}
