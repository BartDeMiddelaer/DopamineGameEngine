using System.Drawing.Imaging;
using Dopamine.Core.Interfaces.EngineInterfaces;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.WinForms;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Dopamine.BatchRenderer.SplashScreenComponents.Entities
{
    public class CubeShadre : Form
    {
        readonly private IEngineFunctionalitys _functionalitys;
        private float angle = 45.0f;
        private GLControl glControl;
        int texturePtrID;

        public CubeShadre(Point location, Size size, IEngineFunctionalitys functionalitys)
        {
            _functionalitys = functionalitys;

            // GLControl it gives you the option to run a OpenGl window inside a Form
            // if you use openTk you will need to make a GameWindow. this cant run inside a Form
            // glControl allows you to use the Controls.Add(glControl) to add it as a Control in a Form
            glControl = new GLControl();

            // Set Api To run OpenGL
            glControl.API = ContextAPI.OpenGL;
            glControl.Flags = ContextFlags.Default;
            glControl.Profile = ContextProfile.Compatability;
            glControl.APIVersion = new Version(3, 3, 0, 0);

            // On load event to preload all the essets els you get compiling issus
            glControl.Load += GlControlLoad;
            glControl.IsEventDriven = true;

            // Control location in the form set to x0 y0
            glControl.Location = new(0, 0);

            // The size is the same size as the form
            glControl.Size = size;

            // Set FormBorderStyle to no None so you get the ilusion the shader is drawn on the splashscreen
            // your asking yourself why use a Form
            // it surves as a closed container to host the shader in
            FormBorderStyle = FormBorderStyle.None;
            Location = location;
            ClientSize = size;
            BackColor = Color.Red;

            // Add the shader in the form
            Controls.Add(glControl);
        }


        private void GlControlLoad(object? sender, EventArgs e)
        {          
            ShaderInit();
            // SetLighting();
            LoadInBoxTextture();
        }
        private void ShaderInit()
        {
            // Makes the Viewport to render on
            GL.Viewport(0, 0, glControl.ClientSize.Width, glControl.ClientSize.Height);

            // Set OpenGl to MatrixMode if you want to run a shader you need somting diferent
            //  dan you will need to use the GL.Shadermode but the rendering fuction provides a Matrix to render the cube
            GL.MatrixMode(MatrixMode.Projection);

            // Preperations to load the Matrix in
            float aspect_ratio = Math.Max(glControl.ClientSize.Width, 1) / (float)Math.Max(glControl.ClientSize.Height, 1);
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            GL.LoadMatrix(ref perpective);
        }
        private void LoadInBoxTextture()
        {
            // Enables 2D texture mapping, which allows you to apply a 2D image to the surface of a 3D object
            GL.Enable(EnableCap.Texture2D);

            // texturePtrID is the pointer adress (the handel to call the texture
            texturePtrID = GL.GenTexture();

            // Gets the path of the file i want to use
            var texPath = _functionalitys.FindPathFileNameInDopamineBatchRenderer("BoxTexture.bmp", "SplashScreenComponents/Images");

            // Store the data of the bitmap 
            BitmapData tex = LoadBitmap(texPath);

            // Specify the data for a two-dimensional texture image
            GL.TexImage2D(
                TextureTarget.Texture2D,    // <- Type of texture
                0,                          // <- Index
                PixelInternalFormat.Rgb,    // <- The type of bitmap in the BitmapData (I used a 24bit bitmap so its RGB)
                tex.Width, tex.Height,      // <- Size of the texture
                0,                          // <- The border size if you want to use it
                PixelFormat.Bgr,            // <- The output pixel type
                PixelType.UnsignedByte,     // <- Type of the pixels in BitmapData
                tex.Scan0);                 // <- Pointer of the BitmapData

            // Generates a UV map for textturinf a polygon 
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            // info: A UVmap is not bound to pixels its a megerment of a plain from 0 to 1 in x and Y
        }
        private void SetLighting()
        {
            // The float[] reprezents the xyz coords
            float[] lightPosition = { 20, 20, 80 };
            float[] lightColor = { 1, 1, 1 };
            float[] lightAmbient = { 1, 1, 1 };

            // Enable lighting
            GL.Enable(EnableCap.Lighting);

            // Set light parameters
            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightColor);
            GL.Light(LightName.Light0, LightParameter.Specular, lightColor);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightAmbient); // set ambient light

            // Flip on the Light
            GL.Enable(EnableCap.Light0);
        }

        public void Render()
        {
            angle++;
            // Makes glControl the current rendering target for OpenGL
            glControl.MakeCurrent();

            // GL.ClearColor function is used to specify the color that is used to clear the color buffer.
            GL.ClearColor(Color4.White);

            // The EnableCap.DepthTest capability, when enabled, enables depth testing for OpenGL rendering.
            // Depth testing is a technique that is used to determine which objects in a 3D scene are visible and which
            // are obscured by other objects that are closer to the viewer.
            GL.Enable(EnableCap.DepthTest);

            // This method is used to create a matrix that represents a "look at" transformation
            // that aligns the view direction with the negative z-axis and the up direction with the positive y-axis.
            // The LookAt method takes 9 arguments:
            //      the position of the eye point,
            //      the position of the target point,
            //      and the direction of the up vector.
            Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);

            // After creating the lookat matrix,
            // the code sets the current matrix mode to MatrixMode.Modelview and loads the lookat matrix
            // into the current matrix using the GL.LoadMatrix method.
            // This sets the modelview matrix to the lookat matrix,
            // which will affect how objects are rendered in the scene.
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);


            GL.Rotate(angle, angle, angle, angle); // <- Rotate's the matrix
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // <- clears the color and depth buffers
            // Starts a new quadrilateral primitive, which is a shape made up of four vertices that are connected to form a flat surface.
            GL.Begin(PrimitiveType.Quads);

            // The BindTexture function is used to bind a texture to the active texture unit in the OpenGL state machine.
            // This means that any subsequent texture operations will affect the bound texture. 
            // Set to Texture2D type and pass the texturePointer me made in the LoadInBoxTextture()
            GL.BindTexture(TextureTarget.Texture2D, texturePtrID);

            // the Box coords. TexCoord2 sets the texturs in the UV plain
            // Vertex3 sets a Vertex a Point in 3D spaeas

            #region Geometry  -----------------------------------------------

            // front face
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);

            // back face
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);

            // left face
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);

            // right face
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);

            // top face
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);

            // bottom face
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            #endregion

            // Signals the end of drawing commands for a particular object or scene
            GL.End();

            // The SwapBuffers() method is a function in the glControl class
            // that is used to swap the front and back buffers of the OpenGL rendering context.
            glControl.SwapBuffers();         
        }      
        BitmapData LoadBitmap(string path)
        {
            // Gets the bitmap
            Bitmap bitmap = new Bitmap(path);

            // Ceates a Rect to deturmen wat past of the bitmap needs to be used
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            // The LockBits method takes in a rectangle representing the portion of the bitmap to lock,
            // an ImageLockMode specifying whether the bits will be read-only or able to be modified,
            // and a pixel format specifying the format of the pixels in the bitmap. In this case, the pixel format is set to Format24bppRgb,
            // which indicates that each pixel is represented by 3 bytes in the bitmap, with one byte each for the red, green, and blue colors.
            BitmapData BitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            // The UnlockBits method is used to release the lock on the bitmap data that was previously obtained using the LockBits method.
            // This is important because the bitmap data is not accessible to other parts of the program while it is locked.
            // Once the data is unlocked, it is available for use again by other parts of the program.
            bitmap.UnlockBits(BitmapData);

            // Returns the bits from the bitmap
            return BitmapData;
        }
    }
}
