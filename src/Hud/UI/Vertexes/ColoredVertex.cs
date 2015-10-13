using SharpDX;
using SharpDX.Direct3D9;

namespace PoeHUD.Hud.UI.Vertexes
{
    public struct ColoredVertex
    {
        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, 0, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 8, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
            VertexElement.VertexDeclarationEnd
        };

        private readonly Vector2 position;

        private readonly ColorBGRA color;

        public ColoredVertex(float x, float y, ColorBGRA color)
        {
            position = new Vector2(x, y);
            this.color = color;
        }
    }
}