using AegirCore.Helper;
using AegirCore.Simulation.Boyancy;
using AegirType;
using System;
using System.Collections.Generic;

namespace AegirCore.Simulation.Water
{
    public class WaterCell
    {
        // User specified options to configure the water object
        private WaterOptions mOptions;

        // Tells the water object if it needs to update the refraction
        // map itself or not. Since refraction just needs the scene drawn
        // regularly, we can:
        // --Draw the objects we want refracted
        // --Resolve the back buffer and send it to the water
        // --Skip computing the refraction map in the water object
        // This is useful if you are already drawing the scene to a render target
        // Prevents from rendering the scene objects multiple times
        private bool mGrabRefractionFromFB = false;

        // Ajouts
        public Vector3[] mVertex;

        private int mNbVertices;
        public List<SimulationTriangle> mTri;
        private int mNbTris;

        private WaveOptions[] wave;
        private int NBWAVES;
        private MersenneTwister mRand = new MersenneTwister();
        private float tWave;
        public float mMin, mMax;
        private const float SINCOEFF1 = -1f / 6f;
        private const float SINCOEFF2 = 1f / 120f;
        private const float SINCOEFF3 = -1f / 5040f;
        private float UpdateSea2_Ld, UpdateSea2_Lf;
        private Matrix mWorld;
        private int gTypeOfSea;

        /// <summary>
        /// Options to configure the water. Must be set before
        /// the water is initialized. Should be set immediately
        /// following the instantiation of the object.
        /// </summary>
        public WaterOptions Options
        {
            get { return mOptions; }
            set { mOptions = value; }
        }

        /// <summary>
        /// The world matrix of the water.
        /// </summary>
        public Matrix World
        {
            get { return mWorld; }
            set { mWorld = value; }
        }

        public WaterCell()
        {
        }

        public void Initialize()
        {
            // Build the water mesh
            mNbVertices = mOptions.SizeX * mOptions.SizeZ;
            mNbTris = (mOptions.SizeX - 1) * (mOptions.SizeZ - 1) * 2;
            mVertex = new Vector3[mNbVertices];

            // Create the water vertex grid positions and indices
            Vector3[] verts;
            int[] indices;
            GenTriGrid(mOptions.SizeZ, mOptions.SizeX, mOptions.CellSpacing, mOptions.CellSpacing, Vector3.Zero, out verts, out indices);

            //// Copy the verts into our PositionTextured array
            //for (int i = 0; i < mOptions.SizeX; ++i)
            //{
            //    for (int j = 0; j < mOptions.SizeZ; ++j)
            //    {
            //        int index = i * mOptions.SizeX + j;
            //        mVertex[index].Position = verts[index];
            //        mVertex[index].TextureCoordinate = new Vector2((float)j / mOptions.SizeX, (float)i / mOptions.SizeZ);
            //    }
            //}

            //mVertexBuffer = new VertexBuffer(Game.GraphicsDevice,
            //                                 VertexPositionNormalTexture.SizeInBytes * mOptions.SizeX * mOptions.SizeZ,
            //                                 BufferUsage.None); // BufferUsage.WriteOnly
            //mVertexBuffer.SetData(mVertex);

            //mIndexBuffer = new IndexBuffer(Game.GraphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            //mIndexBuffer.SetData(indices);

            ////mDecl = new VertexDeclaration(Game.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

            // Initialize Waves
            Initialize6Waves();
            //InitializeRandomWaves1(3);
            UpdateSea2_Ld = mOptions.SizeX;
        }

        //protected override void LoadContent()
        //{
        //    base.LoadContent();

        //    //load the wave maps
        //    mWaveMap0 = Game.Content.Load<Texture2D>(mOptions.WaveMapAsset0);
        //    mWaveMap1 = Game.Content.Load<Texture2D>(mOptions.WaveMapAsset1);

        //    //get the attributes of the back buffer
        //    PresentationParameters pp = Game.GraphicsDevice.PresentationParameters;
        //    SurfaceFormat format = pp.BackBufferFormat;
        //    MultiSampleType msType = pp.MultiSampleType;
        //    int msQuality = pp.MultiSampleQuality;

        //    //create the reflection and refraction render targets
        //    //using the backbuffer attributes
        //    mRefractionMap = new RenderTarget2D(Game.GraphicsDevice, mOptions.RenderTargetSize, mOptions.RenderTargetSize,
        //                                        1, format, msType, msQuality);
        //    mReflectionMap = new RenderTarget2D(Game.GraphicsDevice, mOptions.RenderTargetSize, mOptions.RenderTargetSize,
        //                                        1, format, msType, msQuality);

        //    mEffect = Game.Content.Load<Effect>(mEffectAsset);

        //    //set the parameters that shouldn't change.
        //    //Some of these might need to change every once in awhile,
        //    //move them to updateEffectParams function if you need that functionality.
        //    if (mEffect != null)
        //    {
        //        mEffect.Parameters["WaveMap0"].SetValue(mWaveMap0);
        //        mEffect.Parameters["WaveMap1"].SetValue(mWaveMap1);

        //        mEffect.Parameters["TexScale"].SetValue(mOptions.WaveMapScale);

        //        mEffect.Parameters["WaterColor"].SetValue(mOptions.WaterColor);
        //        mEffect.Parameters["SunColor"].SetValue(mOptions.SunColor);
        //        mEffect.Parameters["SunDirection"].SetValue(Vector3.Normalize(mOptions.SunDirection));
        //        mEffect.Parameters["SunFactor"].SetValue(mOptions.SunFactor);
        //        mEffect.Parameters["SunPower"].SetValue(mOptions.SunPower);

        //        mEffect.Parameters["World"].SetValue(mWorld);
        //    }
        //}
        //public override void Update(GameTime gameTime)
        //{
        //    float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        //    // Update the wave map offsets so that they will scroll across the water
        //    mOptions.WaveMapOffset0 += mOptions.WaveMapVelocity0 * timeDelta;
        //    mOptions.WaveMapOffset1 += mOptions.WaveMapVelocity1 * timeDelta;

        //    if (mOptions.WaveMapOffset0.X >= 1.0f || mOptions.WaveMapOffset0.X <= -1.0f)
        //        mOptions.WaveMapOffset0.X = 0.0f;
        //    if (mOptions.WaveMapOffset1.X >= 1.0f || mOptions.WaveMapOffset1.X <= -1.0f)
        //        mOptions.WaveMapOffset1.X = 0.0f;
        //    if (mOptions.WaveMapOffset0.Y >= 1.0f || mOptions.WaveMapOffset0.Y <= -1.0f)
        //        mOptions.WaveMapOffset0.Y = 0.0f;
        //    if (mOptions.WaveMapOffset1.Y >= 1.0f || mOptions.WaveMapOffset1.Y <= -1.0f)
        //        mOptions.WaveMapOffset1.Y = 0.0f;
        //}
        //public override void Draw(GameTime gameTime)
        //{
        //    // Don't cull back facing triangles since we want the water to be visible
        //    // from beneath the water plane too
        //    Game.GraphicsDevice.RenderState.CullMode = CullMode.None;

        //    if (Global.gWaterShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
        //    UpdateEffectParams();
        //    mEffect.Parameters["Time"].SetValue(tWave);

        //    Game.GraphicsDevice.Indices = mIndexBuffer;
        //    Game.GraphicsDevice.Vertices[0].SetSource(mVertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
        //    Game.GraphicsDevice.VertexDeclaration = mDecl;

        //    mEffect.Begin(SaveStateMode.None);
        //    foreach (EffectPass pass in mEffect.CurrentTechnique.Passes)
        //    {
        //        pass.Begin();
        //        Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mNbVertices, 0, mNbTris);
        //        pass.End();
        //    }
        //    mEffect.End();

        //    Game.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
        //    if (Global.gWaterShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //}
        //public void SetCamera(Matrix4d viewProj, Vector3d pos)
        //{
        //    // Set the ViewProjection matrix and position of the Camera.
        //    mViewProj = viewProj;
        //    mViewPos = pos;
        //}
        //public void UpdateWaterMaps(GameTime gameTime)
        //{
        //    // Updates the reflection and refraction maps. Called on update.
        //    /*------------------------------------------------------------------------------------------
        //     * Render to the Reflection Map
        //     */
        //    //clip objects below the water line, and render the scene upside down
        //    GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;

        //    GraphicsDevice.SetRenderTarget(0, mReflectionMap);
        //    GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, mOptions.WaterColor, 1.0f, 0);

        //    //reflection plane in local space
        //    //the w value can be used to raise or lower the plane to hide gaps between objects and their
        //    //reflection on the water.
        //    Vector4 waterPlaneL = new Vector4(0.0f, -1.0f, 0.0f, 0.0f);

        //    Matrix wInvTrans = Matrix.Invert(mWorld);
        //    wInvTrans = Matrix.Transpose(wInvTrans);

        //    //reflection plane in world space
        //    Vector4d waterPlaneW = Vector4.Transform(waterPlaneL, wInvTrans);

        //    Matrix wvpInvTrans = Matrix.Invert(mWorld * mViewProj);
        //    wvpInvTrans = Matrix.Transpose(wvpInvTrans);

        //    //reflection plane in homogeneous space
        //    Vector4 waterPlaneH = Vector4.Transform(waterPlaneL, wvpInvTrans);

        //    GraphicsDevice.ClipPlanes[0].IsEnabled = true;
        //    GraphicsDevice.ClipPlanes[0].Plane = new Plane(waterPlaneH);

        //    Matrix4d reflectionMatrix = Matrix4d.CreateReflection(new Plane(waterPlaneW));

        //    if (mDrawFunc != null)
        //        mDrawFunc(reflectionMatrix);

        //    GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
        //    GraphicsDevice.ClipPlanes[0].IsEnabled = false;

        //    GraphicsDevice.SetRenderTarget(0, null);

        //    /*------------------------------------------------------------------------------------------
        //     * Render to the Refraction Map
        //     */

        //    //if the application is going to send us the refraction map
        //    //exit early. The refraction map must be given to the water component
        //    //before it renders.
        //    //***This option can be handy if you're already drawing your scene to a render target***
        //    if (mGrabRefractionFromFB)
        //    {
        //        return;
        //    }

        //    //update the refraction map, clip objects above the water line
        //    //so we don't get artifacts
        //    GraphicsDevice.SetRenderTarget(0, mRefractionMap);
        //    GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, mOptions.WaterColor, 1.0f, 1);

        //    //only clip if the camera is above the water plane
        //    if (mViewPos.Y > World.Translation.Y)
        //    {
        //        //refrection plane in local space
        //        //here w=1.1f is a fudge factor so that we don't get gaps between objects and their refraction
        //        //on the water. It effective raises or lowers the height of the clip plane. w=0.0 will be the clip plane
        //        //at the water level. 1.1f raises the clip plane above the water level.
        //        waterPlaneL = new Vector4(0.0f, -1.0f, 0.0f, 1.5f);

        //        //refrection plane in world space
        //        waterPlaneW = Vector4.Transform(waterPlaneL, wInvTrans);

        //        //refrection plane in homogeneous space
        //        waterPlaneH = Vector4.Transform(waterPlaneL, wvpInvTrans);

        //        GraphicsDevice.ClipPlanes[0].IsEnabled = true;
        //        GraphicsDevice.ClipPlanes[0].Plane = new Plane(waterPlaneH);
        //    }

        //    if (mDrawFunc != null)
        //        mDrawFunc(Matrix.Identity);

        //    GraphicsDevice.ClipPlanes[0].IsEnabled = false;
        //    GraphicsDevice.SetRenderTarget(0, null);
        //}
        //void UpdateEffectParams()
        //{
        //    // Updates effect parameters related to the water shader

        //    // Update the reflection and refraction textures
        //    mEffect.Parameters["ReflectMap"].SetValue(mReflectionMap.GetTexture());
        //    mEffect.Parameters["RefractMap"].SetValue(mRefractionMap.GetTexture());

        //    // Normal map offsets
        //    mEffect.Parameters["WaveMapOffset0"].SetValue(mOptions.WaveMapOffset0);
        //    mEffect.Parameters["WaveMapOffset1"].SetValue(mOptions.WaveMapOffset1);

        //    mEffect.Parameters["WorldViewProj"].SetValue(mWorld * mViewProj);

        //    // Pass the position of the camera to the shader
        //    mEffect.Parameters["EyePos"].SetValue(mViewPos);
        //}
        private void GenTriGrid(int numVertRows, int numVertCols, float dx, float dz, Vector3 center, out Vector3[] verts, out int[] indices)
        {
            // Generates a grid of vertices to use for the water plane.

            // "numVertRows" > Number of rows. Must be 2^n + 1. Ex. 129, 257, 513
            // "numVertCols" > Number of columns. Must be 2^n + 1. Ex. 129, 257, 513
            // "dx"          > Cell spacing in the x dimension
            // "dz"          > Cell spacing in the y dimension
            // "center"      > Center of the plane
            // "verts"       > Outputs the constructed vertices for the plane
            // "indices"     > Outpus the constructed triangle indices for the plane

            int numVertices = numVertRows * numVertCols;

            int numCellRows = numVertRows - 1;
            int numCellCols = numVertCols - 1;

            int mNumTris = numCellRows * numCellCols * 2;

            float width = (float)numCellCols * dx;
            float depth = (float)numCellRows * dz;

            //===========================================
            // Build vertices.

            // We first build the grid geometry centered about the origin and on
            // the xz-plane, row-by-row and in a top-down fashion.  We then translate
            // the grid vertices so that they are centered about the specified
            // parameter 'center'.

            //verts.resize(numVertices);
            verts = new Vector3[numVertices];

            // Offsets to translate grid from quadrant 4 to center of coordinate system.
            float xOffset = -width * 0.5f;
            float zOffset = depth * 0.5f;

            int k = 0;
            for (float i = 0; i < numVertRows; ++i)
            {
                for (float j = 0; j < numVertCols; ++j)
                {
                    // Negate the depth coordinate to put in quadrant four.
                    // Then offset to center about coordinate system.
                    verts[k] = new Vector3(0, 0, 0);
                    verts[k].X = j * dx + xOffset;
                    verts[k].Z = -i * dz + zOffset;
                    verts[k].Y = 0.0f;

                    Matrix translation = Matrix.CreateTranslation(center);
                    verts[k] = Vector3.Transform(verts[k], translation);

                    ++k; // Next vertex
                }
            }

            //===========================================
            // Build indices.

            //indices.resize(mNumTris * 3);
            indices = new int[mNumTris * 3];

            // Generate indices for each quad
            k = 0;
            for (int i = 0; i < numCellRows; ++i)
            {
                for (int j = 0; j < numCellCols; ++j)
                {
                    indices[k] = i * numVertCols + j;
                    indices[k + 1] = i * numVertCols + j + 1;
                    indices[k + 2] = (i + 1) * numVertCols + j;

                    indices[k + 3] = (i + 1) * numVertCols + j;
                    indices[k + 4] = i * numVertCols + j + 1;
                    indices[k + 5] = (i + 1) * numVertCols + j + 1;

                    // next quad
                    k += 6;
                }
            }

            // Build List of triangles
            SimulationTriangle Face = new SimulationTriangle();
            mTri = new List<SimulationTriangle>();
            k = 0;
            for (int i = 0; i < numCellRows; ++i)
            {
                for (int j = 0; j < numCellCols; ++j)
                {
                    Face.I0 = indices[k];
                    Face.I1 = indices[k + 1];
                    Face.I2 = indices[k + 2];
                    mTri.Add(Face);
                    Face.I0 = indices[k + 3];
                    Face.I1 = indices[k + 4];
                    Face.I2 = indices[k + 5];
                    mTri.Add(Face);
                    k += 6;
                }
            }
        }

        public void UpdateSea1(SimulationTime simTime)
        {
            int demi_width = mOptions.SizeX / 2;
            int demi_height = mOptions.SizeZ / 2;

            int k = 0;
            for (int i = 0; i < mOptions.SizeX; i++)
            {
                for (int j = 0; j < mOptions.SizeZ; j++)
                {
                    mVertex[k].X = i - demi_width;
                    mVertex[k].Z = j - demi_height;
                    mVertex[k].Y = 0;
                    k++;
                }
            }
            ComputeMinMax();
            //bSoliton = false;
        }

        public void UpdateSea2(SimulationTime simTime)
        {
            //if (bSoliton) return;

            float HWave = 1f;
            float LWave = 10f;
            float CWave = 5f;

            int demi_sizeX = mOptions.SizeX / 2;
            int demi_sizeZ = mOptions.SizeZ / 2;

            float dtWave = (float)simTime.DeltaTime * 1000;

            UpdateSea2_Ld -= CWave * dtWave;
            UpdateSea2_Lf = UpdateSea2_Ld - LWave;

            int k = 0;
            float t1, t2, h;
            Vector3 n;
            for (int x = 0; x != mOptions.SizeX; x++)
            {
                if (x >= UpdateSea2_Lf && x <= UpdateSea2_Ld)
                {
                    t1 = (UpdateSea2_Ld - x) * (float)Math.PI / LWave;
                    t2 = (float)Math.Sin(t1);
                    h = HWave * t2;

                    n = new Vector3(-(float)Math.Cos(t1), t2, 0);
                }
                else
                {
                    h = 0f;
                    n = new Vector3(0, 1, 0);
                }

                for (int z = 0; z != mOptions.SizeZ; z++)
                {
                    mVertex[k].X = x - demi_sizeX;
                    mVertex[k].Z = z - demi_sizeZ;
                    mVertex[k].Y = h;
                    k++;
                }
            }

            if (UpdateSea2_Ld < 0)
            {
                gTypeOfSea = 1;
                UpdateSea2_Ld = mOptions.SizeX;
            }

            //mVertexBuffer.SetData(mVertex);
            ComputeMinMax();
        }

        public void UpdateSea3(SimulationTime SimTime)
        {
            float HWave = 1f;
            float LWave = 25f;  // 15
            float CWave = 4f;   // 4
            float LambdaWave = 13.5f;    // 3.5
            Vector3[] v = new Vector3[mOptions.SizeX];

            float r = HWave / 2;
            float k = 2f * (float)Math.PI / LWave;
            float w = CWave * k;

            float dtWave = (float)SimTime.DeltaTime * 1000;
            tWave += 1f * dtWave;

            for (int i = 0; i < mOptions.SizeX; i++)
            {
                // Houle de Gerstner
                double t = v[i].Y = 0 - r * (float)Math.Cos(k * (i - mOptions.SizeX / 2));
                double cambrure = LambdaWave * t * dtWave;
                v[i].X = i - mOptions.SizeX / 2 + r * (float)Math.Sin(k * (i - mOptions.SizeX / 2) + w * tWave + cambrure);
                v[i].Y = 0 - r * (float)Math.Cos(k * (i - mOptions.SizeX / 2) + w * tWave + cambrure);
                v[i].Z = i - mOptions.SizeX / 2;
            }
            Vector3[] n = new Vector3[mOptions.SizeX];

            for (int i = 1; i < mOptions.SizeX - 1; i++)
            {
                Vector3 v1 = v[i + 1];
                Vector3 v2 = v[i - 1];
                v2.Z = i + 1 - mOptions.SizeX / 2;

                v1 = v[i] - v1;
                v2 = v[i] - v2;
                n[i] = Vector3.Cross(v1, v2);
                n[i] = Vector3.Normalize(n[i]);
            }

            int index = 0;
            for (int i = 0; i < mOptions.SizeX; ++i)
            {
                for (int j = 0; j < mOptions.SizeZ; ++j)
                {
                    mVertex[index].X = v[i].X * mOptions.CellSpacing;
                    mVertex[index].Y = v[i].Y * mOptions.CellSpacing;
                    mVertex[index].Z = v[j].Z * mOptions.CellSpacing;
                    index++;
                }
            }
            //mVertexBuffer.SetData(mVertex);
            ComputeMinMax();
        }

        public void UpdateSea4(SimulationTime simTime)
        {
            float dtWave = (float)simTime.DeltaTime * 1000;
            tWave += 1f * dtWave;

            int demi_width = mOptions.SizeX / 2;
            int demi_height = mOptions.SizeZ / 2;

            float ddx, ddy;
            float attenuationNormal = 0.25f;

            float t1, t2;
            float d2, d3, d4, d5, d6, d7;

            int k = 0;
            for (int i = 0; i < mOptions.SizeX; i++)
            {
                for (int j = 0; j < mOptions.SizeZ; j++)
                {
                    mVertex[k].X = i - demi_width;
                    mVertex[k].Z = j - demi_height;
                    mVertex[k].Y = 0;

                    ddx = 0f;
                    ddy = 0f;

                    for (int w = 0; w < NBWAVES; w++)
                    {
                        t1 = Vector2.Dot(wave[w].Dir, new Vector2(i, j)) * wave[w].Freq + tWave * wave[w].Phase;

                        // Mod into range [-Pi..Pi]
                        t1 = MathHelper.WrapAngle(t1);

                        // Fonction Sinus réduite
                        d2 = t1 * t1;
                        d3 = d2 * t1;
                        d4 = d2 * d2;
                        d5 = d3 * d2;
                        d6 = d3 * d3;
                        d7 = d4 * d3;
                        float sin = t1 + d3 * SINCOEFF1 + d5 * SINCOEFF2 + d7 * SINCOEFF3;

                        // Houle de Gerstner
                        // wave.amp * sin(dot(wave.dir, position) * wave.freq + time * wave.phase)
                        mVertex[k].Y += wave[w].Amp * sin;

                        // Dérivée de la houle de Gerstner
                        // wave.freq * wave.amp * cos(dot(wave.dir, position) * wave.freq + time * wave.phase)
                        t2 = wave[w].Freq * wave[w].Amp * (1f - sin);
                        ddx += t2 * wave[w].Dir.X * attenuationNormal;
                        ddy += t2 * wave[w].Dir.Y * attenuationNormal;
                        //Vector3 B = new Vector3(1, ddx, 0);
                        //Vector3 T = new Vector3(0, ddy, 1);
                        //vertices[k].Normal = Vector3.Cross(B, -T);
                    }
                    mVertex[k] *= mOptions.CellSpacing;
                    k++;
                }
            }
            //mVertexBuffer.SetData(mVertex);
            ComputeMinMax();
        }

        public void ComputeMinMax()
        {
            mMin = float.MaxValue;
            mMax = float.MinValue;
            float h;
            int k = 0;
            for (int i = 0; i < mOptions.SizeX; i++)
            {
                for (int j = 0; j < mOptions.SizeZ; j++)
                {
                    h = mVertex[k].Y;
                    if (h < mMin) mMin = h;
                    if (h > mMax) mMax = h;
                    k++;
                }
            }
        }

        private void Initialize6Waves()
        {
            // frequence = 2 * PI / wavelength
            // phase = speed * frequence

            NBWAVES = 6;
            wave = new WaveOptions[NBWAVES];

            wave[0] = new WaveOptions();
            wave[0].Len = 3.46f;
            wave[0].Amp = 0.09f;
            wave[0].Speed = 1.5f;
            wave[0].Angle = -18.37f;

            wave[0].Dir = new Vector2((float)Math.Cos(MathHelper.ToRadians(wave[0].Angle)), (float)Math.Sin(MathHelper.ToRadians(wave[0].Angle)));
            wave[0].Freq = 2f * (float)Math.PI / wave[0].Len;
            wave[0].Phase = wave[0].Speed * wave[0].Freq;

            wave[1] = new WaveOptions();
            wave[1].Len = 2.38f;
            wave[1].Amp = 0.06f;
            wave[1].Speed = 0.98f;
            wave[1].Angle = 11.31f;

            wave[1].Dir = new Vector2((float)Math.Cos(MathHelper.ToRadians(wave[1].Angle)), (float)Math.Sin(MathHelper.ToRadians(wave[1].Angle)));
            wave[1].Freq = 2f * (float)Math.PI / wave[1].Len;
            wave[1].Phase = wave[1].Speed * wave[1].Freq;

            wave[2] = new WaveOptions();
            wave[2].Len = 5.93f;
            wave[2].Amp = 0.15f;
            wave[2].Speed = 0.49f;
            wave[2].Angle = 1.03f;

            wave[2].Dir = new Vector2((float)Math.Cos(MathHelper.ToRadians(wave[2].Angle)), (float)Math.Sin(MathHelper.ToRadians(wave[2].Angle)));
            wave[2].Freq = 2f * (float)Math.PI / wave[2].Len;
            wave[2].Phase = wave[2].Speed * wave[2].Freq;

            wave[3] = new WaveOptions();
            wave[3].Len = 6.2f;
            wave[3].Amp = 0.15f;
            wave[3].Speed = 1.75f;
            wave[3].Angle = 21.47f;

            wave[3].Dir = new Vector2((float)Math.Cos(MathHelper.ToRadians(wave[3].Angle)), (float)Math.Sin(MathHelper.ToRadians(wave[3].Angle)));
            wave[3].Freq = 2f * (float)Math.PI / wave[3].Len;
            wave[3].Phase = wave[3].Speed * wave[3].Freq;

            wave[4] = new WaveOptions();
            wave[4].Len = 3.78f;
            wave[4].Amp = 0.09f;
            wave[4].Speed = 1.06f;
            wave[4].Angle = 24.05f;

            wave[4].Dir = new Vector2((float)Math.Cos(MathHelper.ToRadians(wave[4].Angle)), (float)Math.Sin(MathHelper.ToRadians(wave[4].Angle)));
            wave[4].Freq = 2f * (float)Math.PI / wave[4].Len;
            wave[4].Phase = wave[4].Speed * wave[4].Freq;

            wave[5] = new WaveOptions();
            wave[5].Len = 8.82f;
            wave[5].Amp = 0.22f;
            wave[5].Speed = 0.00f;
            wave[5].Angle = 12.55f;

            wave[5].Dir = new Vector2((float)Math.Cos(MathHelper.ToRadians(wave[5].Angle)), (float)Math.Sin(MathHelper.ToRadians(wave[5].Angle)));
            wave[5].Freq = 2f * (float)Math.PI / wave[5].Len;
            wave[5].Phase = wave[5].Speed * wave[5].Freq;
        }

        public void InitializeRandomWaves1(int nbWaves)
        {
            // Aucune vague n'est liée à une autre = toutes les vagues sont indépendantes
            // frequence = 2 * PI / wavelength
            // phase = speed * frequence

            NBWAVES = nbWaves;
            wave = new WaveOptions[nbWaves];

            for (int w = 0; w < nbWaves; w++)
            {
                //mRand.Initialize();

                float t = mRand.NextFloatPositive();
                wave[w] = new WaveOptions();
                wave[w].Len = 15f * t * mRand.NextFloatPositive();
                wave[w].Amp = wave[w].Len * 0.025f;
                wave[w].Speed = wave[w].Len * mRand.NextFloatPositive();
                wave[w].Angle = 30f * mRand.NextFloat() * Math.Sign(mRand.Next(-2, 2));
                wave[w].Dir = new Vector2((float)Math.Cos(MathHelper.ToRadians(wave[w].Angle)), (float)Math.Sin(MathHelper.ToRadians(wave[w].Angle)));
                wave[w].Freq = 2f * (float)Math.PI / wave[w].Len;
                wave[w].Phase = wave[w].Speed * wave[w].Freq;
                System.Diagnostics.Debug.WriteLine("{" + w + "} " + wave[w].ToString());
            }
        }

        public void InitializeRandomWaves2(int nbWaves)
        {
            // Les vagues sont liées à une première vague = toutes les vagues ne sont pas indépendantes
            // frequence = 2 * PI / wavelength
            // phase = speed * frequence

            NBWAVES = nbWaves;
            wave = new WaveOptions[nbWaves];

            float t = mRand.NextFloatPositive();
            wave[0] = new WaveOptions();
            wave[0].Len = 15f;// +5f * mRand.NextFloatPositive();
            wave[0].Amp = wave[0].Len * 0.025f;
            wave[0].Speed = 0.15f * wave[0].Len;// wave[0].len * mRand.NextFloatPositive();
            wave[0].Angle = 30f * mRand.NextFloat() * Math.Sign(mRand.Next(-2, 2));
            wave[0].Dir = new Vector2((float)Math.Cos(MathHelper.ToRadians(wave[0].Angle)), (float)Math.Sin(MathHelper.ToRadians(wave[0].Angle)));
            wave[0].Freq = 2f * (float)Math.PI / wave[0].Len;
            wave[0].Phase = wave[0].Speed * wave[0].Freq;

            //NBWAVES = nbWaves = 1; return;
            for (int w = 1; w < nbWaves; w++)
            {
                t = mRand.NextFloatPositive();
                wave[w] = new WaveOptions();
                wave[w].Len = (float)w + mRand.NextFloatPositive();
                wave[w].Amp = wave[w].Len * 0.025f;
                wave[w].Speed = 0.15f * wave[w].Len;// wave[w].len * mRand.NextFloatPositive();
                wave[w].Angle = 30f * mRand.NextFloat() * Math.Sign(mRand.Next(-2, 2));
                wave[w].Dir = new Vector2((float)Math.Cos(MathHelper.ToRadians(wave[w].Angle)), (float)Math.Sin(MathHelper.ToRadians(wave[w].Angle)));
                wave[w].Freq = 2f * (float)Math.PI / wave[w].Len;
                wave[w].Phase = wave[w].Speed * wave[w].Freq;
            }
        }

        public float GetWaterHeight(Vector3 p)
        {
            // Recherche du quad où se situe le point p
            int t0 = GetWaterQuad(p);
            int t1 = t0 + 1;

            float h = 0;

            if (t0 != -1)
            {
                // 1er triangle du quad
                if (GetHeightInTriangle(mTri[t0], p, out h))
                    return h;

                // 2ème triangle du quad
                if (GetHeightInTriangle(mTri[t1], p, out h))
                    return h;
            }
            // Dans tous les cas, il faut retourner une valeur
            return h;
        }

        private int GetWaterQuad(Vector3 p)
        {
            // Identifie le quad (= 2 triangles) où se trouve le point P
            // Renvoie le numéro du premier triangle
            // Le second triangle a un indice supérieur de 1 au précédent
            int halfWidth = (int)((mOptions.SizeX - 1) / 2);
            int triangle = -1;

            int k = 0;
            for (int i = 0; i != mOptions.SizeX; i++)
            {
                for (int j = 0; j != mOptions.SizeZ; j++)
                {
                    if (mVertex[k].X > p.X && mVertex[k].Z > p.Z)
                    {
                        triangle = 2 * ((i - 1) * (halfWidth * 2) + (j - 1));
                        return triangle;
                    }
                    k++;
                }
            }
            return triangle;
        }

        private bool GetHeightInTriangle(SimulationTriangle t, Vector3 p, out float h)
        {
            Vector2 A, B, C, P;
            Vector2 v0, v1, v2;
            float invDenom, u, v;
            bool IsInTriangle = false;

            h = 0;

            A = new Vector2(mVertex[t.I0].X, mVertex[t.I0].Z);
            B = new Vector2(mVertex[t.I1].X, mVertex[t.I1].Z);
            C = new Vector2(mVertex[t.I2].X, mVertex[t.I2].Z);
            P = new Vector2(p.X, p.Z);

            v0 = C - A;
            v1 = B - A;
            v2 = P - A;

            // Compute dot products
            float dot00, dot01, dot02, dot11, dot12;
            Vector2.Dot(ref v0, ref v0, out dot00);
            Vector2.Dot(ref v0, ref v1, out dot01);
            Vector2.Dot(ref v0, ref v2, out dot02);
            Vector2.Dot(ref v1, ref v1, out dot11);
            Vector2.Dot(ref v1, ref v2, out dot12);

            // Compute barycentric coordinates
            invDenom = 1f / (dot00 * dot11 - dot01 * dot01);
            u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Check if point is in triangle
            if ((u >= 0) & (v >= 0) & (u + v <= 1))
            {
                IsInTriangle = true;
                h = mVertex[t.I0].Y + u * (mVertex[t.I2].Y - mVertex[t.I0].Y) + v * (mVertex[t.I1].Y - mVertex[t.I0].Y);
            }
            return IsInTriangle;
        }

        private bool GetHeightInTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 p, out float h)
        {
            Vector2 A, B, C, P;
            Vector2 v0, v1, v2;
            float invDenom, u, v;
            bool IsInTriangle = false;

            h = 0;

            A = new Vector2(a.X, a.Z);
            B = new Vector2(b.X, b.Z);
            C = new Vector2(c.X, c.Z);
            P = new Vector2(p.X, p.Z);

            v0 = C - A;
            v1 = B - A;
            v2 = P - A;

            // Compute dot products
            float dot00, dot01, dot02, dot11, dot12;
            Vector2.Dot(ref v0, ref v0, out dot00);
            Vector2.Dot(ref v0, ref v1, out dot01);
            Vector2.Dot(ref v0, ref v2, out dot02);
            Vector2.Dot(ref v1, ref v1, out dot11);
            Vector2.Dot(ref v1, ref v2, out dot12);

            // Compute barycentric coordinates
            invDenom = 1f / (dot00 * dot11 - dot01 * dot01);
            u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Check if point is in triangle
            if ((u >= 0) & (v >= 0) & (u + v <= 1))
            {
                IsInTriangle = true;
                h = a.Y + u * (c.Y - a.Y) + v * (b.Y - a.Y);
            }
            return IsInTriangle;
        }
    }
}