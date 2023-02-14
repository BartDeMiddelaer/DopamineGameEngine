using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Dopamine.Core.SFMLTypes
{
    public class RoundedRectangleShape : Transformable, Drawable
    {
        public VertexArray Vertices { get; set; }
        public FloatRect Rect { get; set; }
        public float Radius { get; set; }
        public Color FillColor { get; set; }
        public Color StrokeColor { get; set; }
        public float StrokeThickness { get; set; }

        public RoundedRectangleShape(FloatRect rect, float radius, Color fillColor, Color strokeColor, float strokeThickness = 1)
        {
            Rect = rect;
            Radius = radius;
            FillColor = fillColor;
            StrokeColor = strokeColor;
            StrokeThickness = strokeThickness;

            Update();
        }
        public void Update()
        {
            Vertices = new VertexArray(PrimitiveType.TrianglesFan);
            Vertices.Append(new Vertex(new Vector2f(Rect.Left + Radius, Rect.Top), FillColor));

            for (float angle = 0; angle <= 360; angle += 45)
            {
                Vertices.Append(new Vertex(new Vector2f(
                    Rect.Left + Radius + Radius * (float)System.Math.Cos(System.Math.PI * angle / 180),
                    Rect.Top + Radius + Radius * (float)System.Math.Sin(System.Math.PI * angle / 180)), FillColor));
            }

            Vertices.Append(new Vertex(new Vector2f(Rect.Left + Rect.Width - Radius, Rect.Top), FillColor));

            for (float angle = 90; angle <= 360; angle += 45)
            {
                Vertices.Append(new Vertex(new Vector2f(
                    Rect.Left + Rect.Width - Radius + Radius * (float)System.Math.Cos(System.Math.PI * angle / 180),
                    Rect.Top + Radius + Radius * (float)System.Math.Sin(System.Math.PI * angle / 180)), FillColor));
            }

            Vertices.Append(new Vertex(new Vector2f(Rect.Left + Rect.Width, Rect.Top + Rect.Height - Radius), FillColor));

            for (float angle = 180; angle <= 360; angle += 45)
            {
                Vertices.Append(new Vertex(new Vector2f(
                    Rect.Left + Rect.Width - Radius + Radius * (float)System.Math.Cos(System.Math.PI * angle / 180),
                    Rect.Top + Rect.Height - Radius + Radius * (float)System.Math.Sin(System.Math.PI * angle / 180)), FillColor));
            }

            Vertices.Append(new Vertex(new Vector2f(Rect.Left + Radius, Rect.Top + Rect.Height), FillColor));

            for (float angle = 270; angle <= 360; angle += 45)
            {
                Vertices.Append(new Vertex(new Vector2f(
                    Rect.Left + Radius + Radius * (float)System.Math.Cos(System.Math.PI * angle / 180),
                    Rect.Top + Rect.Height - Radius + Radius * (float)System.Math.Sin(System.Math.PI * angle / 180)), FillColor));
            }


        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            target.Draw(Vertices, states);        
        }
    }
}
