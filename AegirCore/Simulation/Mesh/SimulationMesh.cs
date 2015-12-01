using AegirCore.Simulation.Water;
using AegirType;
using System;
using System.Collections.Generic;
using System.IO;

namespace AegirCore.Simulation.Mesh
{
    public class SimulationMesh
    {
        private List<Vector3> mL_Vertex;
        private Vector3[] mVertex;
        private List<Vector3> mL_VertexWorld;
        private Vector3[] mVertexWorld;

        private List<Triangle> mL_Triangle;
        private Triangle[] mTri;

        private Vector3 mMin, mMax;
        private Vector3 mCentroid;

        // Moments of inertia
        private float Ixx = 0;

        private float Iyy = 0;
        private float Izz = 0;
        private float Iyx = 0;
        private float Izx = 0;
        private float Izy = 0;

        // Calcul de surfaces immergées
        private List<int> mL_WaterTriToTest;

        private List<int> mL_MeshTriToTest;
        private List<int> mL_MeshTriSubmerged;
        private List<int> mL_MeshTriEmerged;
        private List<int> mL_VerticesTested;    // 0 = en dessous de l'eau, 1 = au dessus de l'eau
        private List<Triangle> mL_MeshNewTri;
        private List<Vector3> mL_MeshNewVertex;
        private List<float> mL_MeshNewHeight;
        private Dictionary<int, List<Vector3>> mD_TriPts;   // int = N° du triangle, List = liste des points d'intersection
        public bool IsComputed = false;
        private const float ZERO = float.Epsilon;

        public List<TriangleWetted> mL_AllTriWetted;
        public float AreaWetted;
        private float AreaWettedMax;
        private float AreaXZ;
        private float AreaXZ_RacCub;

        // Forces
        public Force Archimede;

        public Force Gravity;
        public Vector3 GravityOffset;
        public Force Total;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        private const float mGRAVITY = 9.81f;
        private const float mWATERDENSITY = 1027f;  // SI = kg / m3

        protected BoundingBox mBoundBox;
        protected BoundingSphere mBoundSphere;

        private WaterCell waterMesh;

        protected Model mModel;

        public Model Model
        {
            get { return mModel; }
            set { mModel = value; }
        }

        protected string mModelName;

        public string ModelName
        {
            get { return mModelName; }
            set { mModelName = value; }
        }

        protected string mModelDisplayName;

        public string ModelDisplayName
        {
            get { return mModelDisplayName; }
            set { mModelDisplayName = value; }
        }

        protected int mNbVertices;

        public int NbVertices
        {
            get { return mNbVertices; }
            set { mNbVertices = value; }
        }

        protected int mNbTriangles;

        public int NbTriangles
        {
            get { return mNbTriangles; }
            set { mNbTriangles = value; }
        }

        protected Vector3 mPosition;

        public Vector3 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        protected float mLength;

        public float Length
        {
            get { return mLength; }
            set { mLength = value; }
        }

        protected float mWidth;

        public float Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }

        protected float mVolume;

        public float Volume
        {
            get { return mVolume; }
            set { mVolume = value; }
        }

        protected float mMass;

        public float Mass
        {
            get { return mMass; }
            set { mMass = value; }
        }

        protected Vector3 Center
        {
            get
            {
                Vector3 min = Vector3.Transform(mBoundBox.Min, mWorld);
                Vector3 max = Vector3.Transform(mBoundBox.Max, mWorld);

                return (min + max) * .5f;
            }
        }

        protected float mScale;

        public float Scale
        {
            get { return mScale; }
            set { mScale = value; }
        }

        protected float mRotationX;

        public float RotationX
        {
            get { return mRotationX; }
            set { mRotationX = value; }
        }

        protected float mRotationY;

        public float RotationY
        {
            get { return mRotationY; }
            set { mRotationY = value; }
        }

        public float mRotationZ;

        public float RotationZ
        {
            get { return mRotationZ; }
            set { mRotationZ = value; }
        }

        protected Vector3 mRotAxis;

        public Vector3 RotationAxis
        {
            get { return mRotAxis; }
            set { mRotAxis = value; }
        }

        protected string mEffectAsset;

        public string EffectAsset
        {
            get { return mEffectAsset; }
            set { mEffectAsset = value; }
        }

        protected string mTexAsset;

        public string TextureAsset
        {
            get { return mTexAsset; }
            set { mTexAsset = value; }
        }

        protected string mEnvTexAsset;

        public string EnvironmentTextureAsset
        {
            get { return mEnvTexAsset; }
            set { mEnvTexAsset = value; }
        }

        protected bool mLightingEnabled;

        public bool EnableLighting
        {
            get { return mLightingEnabled; }
            set { mLightingEnabled = value; }
        }

        protected bool mSpecularLighting;

        public bool SpecularLighting
        {
            get { return mSpecularLighting; }
            set { mSpecularLighting = value; }
        }

        protected Matrix mWorld;

        public Matrix World
        {
            get { return mWorld; }
            set { mWorld = value; }
        }

        protected Matrix mView;

        public Matrix View
        {
            set { mView = value; }
        }

        protected Matrix mProj;

        public Matrix Projection
        {
            set { mProj = value; }
        }

        protected Vector3 mViewPos;

        public Vector3 ViewPosition
        {
            set { mViewPos = value; }
        }

        protected bool mToDraw = true;

        public bool ToDraw
        {
            get { return mToDraw; }
            set { mToDraw = value; }
        }

        protected bool mBuoyancy = true;

        public bool Buoyancy
        {
            get { return mBuoyancy; }
            set { mBuoyancy = value; }
        }

        protected bool mToCompute = false;

        public bool ToCompute
        {
            get { return mToCompute; }
            set { mToCompute = value; }
        }

        protected string mType = "";

        public string Type
        {
            get { return mType; }
            set { mType = value; }
        }

        public SimulationMesh()
        {
            mPosition = Vector3.Zero;
            mScale = 1.0f;
            mRotationX = 0.0f;
            mRotationY = 0.0f;
            mRotationZ = 0.0f;
            mRotAxis = Vector3.Up;
        }

        protected void LoadContent()
        {
            //if (mEffectAsset != null)
            //{
            //    mEffect = Game.Content.Load<Effect>(mEffectAsset);

            //    int i = 0;
            //    foreach (Light light in mLights)
            //    {
            //        mEffect.Parameters["LightDir" + i].SetValue(light.Direction);
            //        mEffect.Parameters["LightDiffuse" + i].SetValue(light.DiffuseColor);
            //        mEffect.Parameters["LightAmbient" + i].SetValue(light.AmbientColor);
            //        i++;
            //    }
            //}

            //if (mModelName != null)
            //{
            //    mModel = Game.Content.Load<Model>(mModelName);
            //    if (mModelDisplayName != null) mModelDisplay = Game.Content.Load<Model>(mModelDisplayName);

            //    if (mModel.Tag != null) mBoundBox = (BoundingBox)mModel.Tag;
            //    else mBoundBox = new BoundingBox();
            //TODO: Do calcs based on passed data
            ComputeBoundingSphere();
            //}

            if (mToCompute)
            {
                ExtractAllData();
                ComputeProperties();
                //WriteMeshInfo();
            }
        }

        public void Update(SimulationTime gameTime)
        {
            mWorld = Matrix.CreateFromYawPitchRoll(mRotationY, mRotationX, mRotationZ) * Matrix.CreateScale(mScale) * Matrix.CreateTranslation(mPosition);

            if (mVertex != null && mToCompute)
            {
                TransformAllVerticesToWorld();
                ComputeAABB();
                TestAllVertices();
                ListMeshTriToTest();
                ListWaterTriToTest();
                ComputeIntersections();
                CreateMeshNewTri();
                ComputeArchimede();
                ComputeAllForces(gameTime);
                //if (!IsComputed)  IsComputed = true;
            }
        }

        #region Drawing

        //public override void Draw(GameTime gameTime)
        //{
        //    if (!mToDraw) return;

        //    if (mToCompute)
        //    {
        //        if (Global.gShipDisplayModel && mModelDisplayName != null)
        //        {
        //            DrawModelDisplay();
        //            return;
        //        }
        //        if (Global.gShipShowTriangles)
        //        {
        //            //DrawVolumeEmerged();
        //            DrawWettedTri();
        //        }
        //        else
        //        {
        //            DrawBasicEffect();
        //        }
        //        if (Global.gShipShowForces)
        //        {
        //            DrawWettedNormals();
        //            DrawForces();
        //        }
        //        if (Global.gShipShowNormals) DrawAllNormals();

        //        return;
        //    }

        //    // Draw with the basic effect
        //    if (mEffect == null) DrawBasicEffect();
        //    else DrawCustomEffect();
        //}
        //void DrawBasicEffect()
        //{
        //    Matrix[] boneTransforms = new Matrix[mModel.Bones.Count];
        //    mModel.CopyAbsoluteBoneTransformsTo(boneTransforms);
        //    if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
        //    Game.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
        //    foreach (ModelMesh mesh in mModel.Meshes)
        //    {
        //        foreach (BasicEffect effect in mesh.Effects)
        //        {
        //            effect.World = boneTransforms[mesh.ParentBone.Index] * mWorld;
        //            effect.View = mView;
        //            effect.Projection = mProj;

        //            if (mTexture != null)
        //            {
        //                effect.Texture = mTexture;
        //                effect.TextureEnabled = true;
        //            }
        //            if (mLightingEnabled)
        //            {
        //                effect.EnableDefaultLighting();
        //                effect.PreferPerPixelLighting = true;

        //                if (!mSpecularLighting) effect.SpecularColor = Vector3.Zero;
        //            }
        //            else effect.LightingEnabled = false;
        //        }
        //        mesh.Draw();
        //    }
        //    if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //}
        //void DrawCustomEffect()
        //{
        //    Matrix[] boneTransforms = new Matrix[mModel.Bones.Count];
        //    mModel.CopyAbsoluteBoneTransformsTo(boneTransforms);
        //    mEffect.Begin(SaveStateMode.None);

        //    foreach (ModelMesh mesh in mModel.Meshes)
        //    {
        //        Matrix world = boneTransforms[mesh.ParentBone.Index] * mWorld;

        //        Matrix worldInvTrans = Matrix.Invert(world);
        //        worldInvTrans = Matrix.Transpose(worldInvTrans);

        //        mEffect.Parameters["WorldInvTrans"].SetValue(worldInvTrans);
        //        mEffect.Parameters["WorldViewProj"].SetValue(world * mView * mProj);
        //        if (mTexture == null) mEffect.Parameters["DiffuseTex"].SetValue(((BasicEffect)mesh.Effects[0]).Texture);
        //        else mEffect.Parameters["DiffuseTex"].SetValue(mTexture);

        //        // Set the index buffer
        //        Game.GraphicsDevice.Indices = mesh.IndexBuffer;

        //        foreach (EffectPass pass in mEffect.CurrentTechnique.Passes)
        //        {
        //            pass.Begin();
        //            foreach (ModelMeshPart meshPart in mesh.MeshParts)
        //            {
        //                Game.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, meshPart.StreamOffset, meshPart.VertexStride);
        //                Game.GraphicsDevice.VertexDeclaration = meshPart.VertexDeclaration;
        //                Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.BaseVertex, 0,
        //                                                            meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
        //            }
        //            pass.End();
        //        }
        //    }
        //    mEffect.End();
        //}
        //void DrawModelDisplay()
        //{
        //    Matrix[] boneTransforms = new Matrix[mModelDisplay.Bones.Count];
        //    mModelDisplay.CopyAbsoluteBoneTransformsTo(boneTransforms);
        //    if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
        //    Game.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
        //    foreach (ModelMesh mesh in mModelDisplay.Meshes)
        //    {
        //        foreach (BasicEffect effect in mesh.Effects)
        //        {
        //            effect.World = boneTransforms[mesh.ParentBone.Index] * mWorld;
        //            effect.View = mView;
        //            effect.Projection = mProj;

        //            if (mTexture != null)
        //            {
        //                effect.Texture = mTexture;
        //                effect.TextureEnabled = true;
        //            }
        //            if (mLightingEnabled)
        //            {
        //                effect.EnableDefaultLighting();
        //                effect.PreferPerPixelLighting = true;

        //                if (!mSpecularLighting) effect.SpecularColor = Vector3.Zero;
        //            }
        //            else effect.LightingEnabled = false;
        //        }
        //        mesh.Draw();
        //    }
        //    if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //}
        //void DrawWaterToTest()
        //{
        //    if (mL_WaterTriToTest.Count != 0)
        //    {
        //        Game.GraphicsDevice.RenderState.CullMode = CullMode.None;
        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;

        //        VertexPositionColor[] verts = new VertexPositionColor[mL_WaterTriToTest.Count * 3];
        //        int i = 0;
        //        foreach (int t in mL_WaterTriToTest)
        //        {
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mGame.mWaterMesh.mVertex[mGame.mWaterMesh.mTri[t].I0].Position;
        //            verts[i++].Color = Color.Navy;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mGame.mWaterMesh.mVertex[mGame.mWaterMesh.mTri[t].I1].Position;
        //            verts[i++].Color = Color.Navy;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mGame.mWaterMesh.mVertex[mGame.mWaterMesh.mTri[t].I2].Position;
        //            verts[i++].Color = Color.Navy;
        //        }
        //        Game.GraphicsDevice.VertexDeclaration = new VertexDeclaration(Game.GraphicsDevice, VertexPositionColor.VertexElements);

        //        BasicEffect effect = new BasicEffect(Game.GraphicsDevice, null);
        //        effect.World = Matrix.Identity;
        //        effect.View = mView;
        //        effect.Projection = mProj;
        //        effect.Begin();
        //        effect.VertexColorEnabled = true;

        //        foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //        {
        //            CurrentPass.Begin();
        //            Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, mL_WaterTriToTest.Count);
        //            CurrentPass.End();
        //        }
        //        effect.End();

        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //    }
        //}
        //void DrawVolumeToTest()
        //{
        //    if (mL_MeshTriToTest.Count != 0)
        //    {
        //        Game.GraphicsDevice.RenderState.CullMode = CullMode.None;
        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;

        //        VertexPositionColor[] verts = new VertexPositionColor[mL_MeshTriToTest.Count * 3];
        //        int i = 0;
        //        foreach (int t in mL_MeshTriToTest)
        //        {
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mVertex[mTri[t].I0];
        //            verts[i++].Color = Color.White;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mVertex[mTri[t].I1];
        //            verts[i++].Color = Color.White;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mVertex[mTri[t].I2];
        //            verts[i++].Color = Color.White;
        //        }
        //        Game.GraphicsDevice.VertexDeclaration = new VertexDeclaration(Game.GraphicsDevice, VertexPositionColor.VertexElements);

        //        BasicEffect effect = new BasicEffect(Game.GraphicsDevice, null);
        //        effect.World = mWorld;
        //        effect.View = mView;
        //        effect.Projection = mProj;
        //        effect.VertexColorEnabled = true;
        //        effect.Begin();
        //        foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //        {
        //            CurrentPass.Begin();
        //            Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, mL_MeshTriToTest.Count);
        //            CurrentPass.End();
        //        }
        //        effect.End();

        //        // Second passage pour souligner les côtés
        //        if (!Global.gShipShowWireFrame)
        //        {
        //            Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
        //            for (int j = 0; j != verts.Length; j++) verts[j].Color = Color.Black;

        //            effect.Begin();
        //            foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //            {
        //                CurrentPass.Begin();
        //                Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, mL_MeshTriToTest.Count);
        //                CurrentPass.End();
        //            }
        //            effect.End();
        //            Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //        }
        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //    }
        //}
        //void DrawVolumeEmerged()
        //{
        //    if (mL_MeshTriEmerged.Count != 0)
        //    {
        //        Game.GraphicsDevice.RenderState.CullMode = CullMode.None;
        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;

        //        VertexPositionColor[] verts = new VertexPositionColor[mL_MeshTriEmerged.Count * 3];
        //        int i = 0;
        //        foreach (int t in mL_MeshTriEmerged)
        //        {
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mVertex[mTri[t].I0];
        //            verts[i++].Color = Color.Green;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mVertex[mTri[t].I1];
        //            verts[i++].Color = Color.Green;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mVertex[mTri[t].I2];
        //            verts[i++].Color = Color.Green;
        //        }
        //        Game.GraphicsDevice.VertexDeclaration = new VertexDeclaration(Game.GraphicsDevice, VertexPositionColor.VertexElements);

        //        BasicEffect effect = new BasicEffect(Game.GraphicsDevice, null);
        //        effect.World = mWorld;
        //        effect.View = mView;
        //        effect.Projection = mProj;
        //        effect.VertexColorEnabled = true;
        //        effect.Begin();
        //        foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //        {
        //            CurrentPass.Begin();
        //            Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, mL_MeshTriEmerged.Count);
        //            CurrentPass.End();
        //        }
        //        effect.End();

        //        // Second passage pour souligner les côtés
        //        if (!Global.gShipShowWireFrame)
        //        {
        //            Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
        //            for (int j = 0; j != verts.Length; j++) verts[j].Color = Color.Black;

        //            effect.Begin();
        //            foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //            {
        //                CurrentPass.Begin();
        //                Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, mL_MeshTriEmerged.Count);
        //                CurrentPass.End();
        //            }
        //            effect.End();
        //            Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //        }
        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //    }
        //}
        //void DrawVolumeNewTri()
        //{
        //    if (mL_MeshNewTri.Count != 0)
        //    {
        //        Game.GraphicsDevice.RenderState.CullMode = CullMode.None;
        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;

        //        VertexPositionColor[] verts = new VertexPositionColor[mL_MeshNewTri.Count * 3];
        //        int i = 0;
        //        foreach (Triangle t in mL_MeshNewTri)
        //        {
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mL_MeshNewVertex[t.I0];
        //            verts[i++].Color = t.color;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mL_MeshNewVertex[t.I1];
        //            verts[i++].Color = t.color;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mL_MeshNewVertex[t.I2];
        //            verts[i++].Color = t.color;
        //        }
        //        Game.GraphicsDevice.VertexDeclaration = new VertexDeclaration(Game.GraphicsDevice, VertexPositionColor.VertexElements);

        //        BasicEffect effect = new BasicEffect(Game.GraphicsDevice, null);
        //        effect.World = Matrix.Identity;
        //        effect.View = mView;
        //        effect.Projection = mProj;
        //        effect.VertexColorEnabled = true;
        //        effect.Begin();

        //        int NB = mL_MeshNewTri.Count;
        //        foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //        {
        //            CurrentPass.Begin();
        //            Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, NB);
        //            CurrentPass.End();
        //        }
        //        effect.End();

        //        // Second passage pour souligner les côtés
        //        if (!Global.gShipShowWireFrame)
        //        {
        //            Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
        //            for (int j = 0; j != verts.Length; j++) verts[j].Color = Color.Black;

        //            effect.Begin();
        //            foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //            {
        //                CurrentPass.Begin();
        //                Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, NB);
        //                CurrentPass.End();
        //            }
        //            effect.End();
        //            Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //        }
        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //    }
        //}
        //void DrawVolumeSubmerged()
        //{
        //    if (mL_MeshTriSubmerged.Count != 0)
        //    {
        //        Game.GraphicsDevice.RenderState.CullMode = CullMode.None;
        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;

        //        VertexPositionColor[] verts = new VertexPositionColor[mL_MeshTriSubmerged.Count * 3];
        //        int i = 0;
        //        foreach (int t in mL_MeshTriSubmerged)
        //        {
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mVertex[mTri[t].I0];
        //            verts[i++].Color = Color.Gray;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mVertex[mTri[t].I1];
        //            verts[i++].Color = Color.Gray;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mVertex[mTri[t].I2];
        //            verts[i++].Color = Color.Gray;
        //        }
        //        Game.GraphicsDevice.VertexDeclaration = new VertexDeclaration(Game.GraphicsDevice, VertexPositionColor.VertexElements);

        //        BasicEffect effect = new BasicEffect(Game.GraphicsDevice, null);
        //        effect.World = mWorld;
        //        effect.View = mView;
        //        effect.Projection = mProj;
        //        effect.VertexColorEnabled = true;
        //        effect.Begin();
        //        foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //        {
        //            CurrentPass.Begin();
        //            Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, mL_MeshTriSubmerged.Count);
        //            CurrentPass.End();
        //        }
        //        effect.End();

        //        // Second passage pour souligner les côtés
        //        if (!Global.gShipShowWireFrame)
        //        {
        //            Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
        //            for (int j = 0; j != verts.Length; j++) verts[j].Color = Color.Black;

        //            effect.Begin();
        //            foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //            {
        //                CurrentPass.Begin();
        //                Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, mL_MeshTriSubmerged.Count);
        //                CurrentPass.End();
        //            }
        //            effect.End();
        //            Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //        }
        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //    }
        //}
        //void DrawWettedTri()
        //{
        //    if (mL_AllTriWetted.Count != 0)
        //    {
        //        Game.GraphicsDevice.RenderState.CullMode = CullMode.None;
        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;

        //        VertexPositionColor[] verts = new VertexPositionColor[mL_AllTriWetted.Count * 3];
        //        int i = 0;
        //        Color c = new Color();
        //        foreach (TriangleWetted TW in mL_AllTriWetted)
        //        {
        //            if (TW.bNoChange) c = Color.Gray; else c = TW.color;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mL_MeshNewVertex[TW.I0];
        //            verts[i++].Color = c;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mL_MeshNewVertex[TW.I1];
        //            verts[i++].Color = c;
        //            verts[i] = new VertexPositionColor();
        //            verts[i].Position = mL_MeshNewVertex[TW.I2];
        //            verts[i++].Color = c;
        //        }

        //        BasicEffect effect = new BasicEffect(Game.GraphicsDevice, null);
        //        effect.World = Matrix.Identity;
        //        effect.View = mView;
        //        effect.Projection = mProj;
        //        effect.VertexColorEnabled = true;
        //        effect.Begin();
        //        int NB = mL_AllTriWetted.Count;
        //        foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //        {
        //            CurrentPass.Begin();
        //            Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, NB);
        //            CurrentPass.End();
        //        }
        //        effect.End();

        //        // Second passage pour souligner les côtés
        //        if (!Global.gShipShowWireFrame)
        //        {
        //            Game.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;

        //            for (int j = 0; j != verts.Length; j++) verts[j].Color = Color.Black;

        //            effect.Begin();
        //            foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //            {
        //                CurrentPass.Begin();
        //                Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, NB);
        //                CurrentPass.End();
        //            }
        //            effect.End();
        //            Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //        }

        //        if (Global.gShipShowWireFrame) Game.GraphicsDevice.RenderState.FillMode = FillMode.Solid;
        //    }
        //}
        //void DrawWettedNormals()
        //{
        //    if (mL_AllTriWetted.Count != 0)
        //    {
        //        VertexPositionColor[] pnts = new VertexPositionColor[mL_AllTriWetted.Count * 2];

        //        int i = -1;
        //        foreach (TriangleWetted TW in mL_AllTriWetted)
        //        {
        //            i++;
        //            pnts[i].Position = TW.pCPressure;
        //            pnts[i].Color = Color.Red;
        //            i++;
        //            pnts[i].Position = TW.pCPressure - TW.vPressure / 10000f;
        //            pnts[i].Color = Color.Red;
        //        }
        //        BasicEffect effect = new BasicEffect(Game.GraphicsDevice, null);
        //        effect.World = Matrix.Identity;
        //        effect.View = mView;
        //        effect.Projection = mProj;
        //        effect.VertexColorEnabled = true;
        //        effect.Begin();
        //        foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //        {
        //            CurrentPass.Begin();
        //            Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, pnts, 0, mL_AllTriWetted.Count);
        //            CurrentPass.End();
        //        }
        //        effect.End();
        //    }
        //}
        //void DrawAllNormals()
        //{
        //    VertexPositionColor[] pnts = new VertexPositionColor[mL_Triangle.Count * 2];

        //    int i = -1;
        //    foreach (Triangle t in mL_Triangle)
        //    {
        //        i++;
        //        pnts[i].Position = t.vCG;
        //        pnts[i].Color = Color.Red;
        //        i++;
        //        pnts[i].Position = t.vCG + t.vNormal;
        //        pnts[i].Color = Color.Red;
        //    }
        //    BasicEffect effect = new BasicEffect(Game.GraphicsDevice, null);
        //    effect.World = mWorld;
        //    effect.View = mView;
        //    effect.Projection = mProj;
        //    effect.VertexColorEnabled = true;
        //    effect.Begin();
        //    foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //    {
        //        CurrentPass.Begin();
        //        Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, pnts, 0, mL_Triangle.Count);
        //        CurrentPass.End();
        //    }
        //    effect.End();
        //}
        //void DrawForces()
        //{
        //    float f = 2f * Gravity.Vector.Length();
        //    DrawLine3D(Archimede.Position, Archimede.Position + Archimede.Vector * Length / f, Color.White);
        //    DrawLine3D(Gravity.Position, Gravity.Position + Gravity.Vector * Length / f, Color.Black);
        //}
        //void DrawLine3D(Vector3 from, Vector3 to, Color color)
        //{
        //    VertexPositionColor[] pnts = {new VertexPositionColor(new Vector3(from.X, from.Y, from.Z), color),
        //                                  new VertexPositionColor(new Vector3(to.X, to.Y, to.Z), color)};
        //    BasicEffect effect = new BasicEffect(Game.GraphicsDevice, null);
        //    effect.World = Matrix.Identity;
        //    effect.View = mView;
        //    effect.Projection = mProj;
        //    effect.VertexColorEnabled = true;
        //    effect.Begin();
        //    foreach (EffectPass CurrentPass in effect.CurrentTechnique.Passes)
        //    {
        //        CurrentPass.Begin();
        //        Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, pnts, 0, 1);
        //        CurrentPass.End();
        //    }
        //    effect.End();
        //}

        #endregion Drawing

        // Compute Once
        private void ExtractAllData()
        {
            mL_Vertex = new List<Vector3>();
            mL_Triangle = new List<Triangle>();

            //foreach (ModelMesh mm in mModel.Meshes)
            //{
            //    foreach (ModelMeshPart mmp in mm.MeshParts)
            //    {
            //        // Get number of vertices already used
            //        int vertexOffset = mL_Vertex.Count;

            //        // List of vertices in this part
            //        Vector3[] partVertices = new Vector3[mmp.NumVertices];

            //        // Get data from model mesh
            //        mm.VertexBuffer.GetData<Vector3>(mmp.StreamOffset + mmp.BaseVertex * mmp.VertexStride, partVertices, 0, mmp.NumVertices, mmp.VertexStride);

            //        // Add vertex data
            //        mL_Vertex.AddRange(partVertices);
            //        mVertex = mL_Vertex.ToArray();
            //        mVertexWorld = new Vector3[mVertex.Length];

            //        // Get raw triangle index data
            //        short[] rawTriData = new short[mmp.PrimitiveCount * 3];
            //        mm.IndexBuffer.GetData<short>(mmp.StartIndex * 2, rawTriData, 0, mmp.PrimitiveCount * 3);

            //        // Get triangle data for this part
            //        mTri = new Triangle[mmp.PrimitiveCount];
            //        Random rand = new Random();
            //        for (int i = 0; i != mTri.Length; i++)
            //        {
            //            mTri[i].I0 = rawTriData[i * 3 + 0] + vertexOffset;
            //            mTri[i].I1 = rawTriData[i * 3 + 1] + vertexOffset;
            //            mTri[i].I2 = rawTriData[i * 3 + 2] + vertexOffset;
            //            mTri[i].color = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
            //            Vector3 u = mVertex[mTri[i].I1] - mVertex[mTri[i].I0];
            //            Vector3 v = mVertex[mTri[i].I2] - mVertex[mTri[i].I0];
            //            Vector3 a = Vector3.Cross(v, u);
            //            mTri[i].fArea = (float)(0.5 * Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z));
            //            mTri[i].vNormal = Vector3.Normalize(a);
            //            mTri[i].vCG = (mVertex[mTri[i].I0] + mVertex[mTri[i].I1] + mVertex[mTri[i].I2]) / 3;
            //        }

            //        // Add triangle data
            //        mL_Triangle.AddRange(mTri);
            //    }
            //}
            NbVertices = mVertex.Length;
            NbTriangles = mTri.Length;
        }

        private void WriteMeshInfo()
        {
            string value = mModelName;

            char[] delimiters = new char[] { '/' };
            string[] parts = value.Split(delimiters, StringSplitOptions.None);

            StreamWriter writer = new StreamWriter(parts[parts.Length - 1] + ".txt");

            writer.WriteLine("Vertices");
            writer.WriteLine("--------");

            writer.WriteLine();

            for (int i = 0; i != mVertex.Length; i++)
                writer.WriteLine(i + " : " +
                    mVertex[i].X.ToString("0.000000") + "\t" +
                    mVertex[i].Y.ToString("0.000000") + "\t" +
                    mVertex[i].Z.ToString("0.000000"));

            writer.WriteLine();
            writer.WriteLine("Triangles");
            writer.WriteLine("--------");

            writer.WriteLine();

            for (int i = 0; i != mTri.Length; i++)
                writer.WriteLine(i + " : " +
                    mTri[i].I0.ToString() + "," +
                    mTri[i].I1.ToString() + "," +
                    mTri[i].I2.ToString());

            writer.Close();
        }         // NOT USED

        private void ComputeProperties()
        {
            // Size
            // ====
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minZ = float.MaxValue;
            float maxZ = float.MinValue;

            for (int i = 0; i < mVertex.Length; i++)
            {
                minX = Math.Min(minX, mVertex[i].X);
                maxX = Math.Max(maxX, mVertex[i].X);
                minZ = Math.Min(minZ, mVertex[i].Z);
                maxZ = Math.Max(maxZ, mVertex[i].Z);
            }
            mLength = maxX - minX;
            mWidth = maxZ - minZ;

            // Volume et Moments d'inertie
            // ===========================
            float _m = 0;      // Mass
            float _Cx = 0;     // Centroid
            float _Cy = 0;
            float _Cz = 0;
            float _xx = 0;     // Moment of inertia tensor
            float _yy = 0;
            float _zz = 0;
            float _yx = 0;
            float _zx = 0;
            float _zy = 0;

            Vector3 a = new Vector3();
            Vector3 b = new Vector3();
            Vector3 c = new Vector3();

            for (int i = 0; i < mTri.Length; i++)
            {
                a.X = mVertex[mTri[i].I0].X;
                a.Y = mVertex[mTri[i].I0].Y;
                a.Z = mVertex[mTri[i].I0].Z;

                b.X = mVertex[mTri[i].I2].X;
                b.Y = mVertex[mTri[i].I2].Y;
                b.Z = mVertex[mTri[i].I2].Z;

                c.X = mVertex[mTri[i].I1].X;
                c.Y = mVertex[mTri[i].I1].Y;
                c.Z = mVertex[mTri[i].I1].Z;

                // Signed volume of this tetrahedron.
                float v = a.X * b.Y * c.Z + a.Y * b.Z * c.X + b.X * c.Y * a.Z - (c.X * b.Y * a.Z + b.X * a.Y * c.Z + c.Y * b.Z * a.X);

                // Contribution to the mass
                _m += v;

                // Contribution to the centroid
                Vector3 d = new Vector3();
                d.X = a.X + b.X + c.X; _Cx += (v * d.X);
                d.Y = a.Y + b.Y + c.Y; _Cy += (v * d.Y);
                d.Z = a.Z + b.Z + c.Z; _Cz += (v * d.Z);

                // Contribution to moment of inertia monomials
                _xx += v * (a.X * a.X + b.X * b.X + c.X * c.X + d.X * d.X);
                _yy += v * (a.Y * a.Y + b.Y * b.Y + c.Y * c.Y + d.Y * d.Y);
                _zz += v * (a.Z * a.Z + b.Z * b.Z + c.Z * c.Z + d.Z * d.Z);
                _yx += v * (a.Y * a.X + b.Y * b.X + c.Y * c.X + d.Y * d.X);
                _zx += v * (a.Z * a.X + b.Z * b.X + c.Z * c.X + d.Z * d.X);
                _zy += v * (a.Z * a.Y + b.Z * b.Y + c.Z * c.Y + d.Z * d.Y);
            }

            // Centroid.
            // The case _m = 0 needs to be addressed here.
            float r = 1f / (4f * _m);
            mCentroid.X = _Cx * r;
            mCentroid.Y = _Cy * r;
            mCentroid.Z = _Cz * r;

            // Volume
            mVolume = _m / 6f;

            // Moment of inertia about the centroid.
            r = 1f / 120f;
            Iyx = _yx * r - mVolume * mCentroid.Y * mCentroid.X;
            Izx = _zx * r - mVolume * mCentroid.Z * mCentroid.X;
            Izy = _zy * r - mVolume * mCentroid.Z * mCentroid.Y;

            _xx = _xx * r - mVolume * mCentroid.X * mCentroid.X;
            _yy = _yy * r - mVolume * mCentroid.Y * mCentroid.Y;
            _zz = _zz * r - mVolume * mCentroid.Z * mCentroid.Z;

            Ixx = _yy + _zz;
            Iyy = _zz + _xx;
            Izz = _xx + _yy;

            // Surface mouillée maxi
            AreaWettedMax = 0f;
            for (int i = 0; i < mTri.Length; i++)
            {
                a.X = mVertex[mTri[i].I0].X;
                a.Y = mVertex[mTri[i].I0].Y;
                a.Z = mVertex[mTri[i].I0].Z;

                b.X = mVertex[mTri[i].I2].X;
                b.Y = mVertex[mTri[i].I2].Y;
                b.Z = mVertex[mTri[i].I2].Z;

                c.X = mVertex[mTri[i].I1].X;
                c.Y = mVertex[mTri[i].I1].Y;
                c.Z = mVertex[mTri[i].I1].Z;

                Vector3 u = b - a;
                Vector3 v = c - a;
                Vector3 aa = Vector3.Cross(v, u);
                AreaWettedMax += (float)(0.5 * Math.Sqrt(aa.X * aa.X + aa.Y * aa.Y + aa.Z * aa.Z));
            }

            // Divers
            AreaXZ = mLength * mWidth;
            AreaXZ_RacCub = (float)Math.Pow(AreaXZ, 1f / 3f);
        }

        // Compute All time
        private void TransformAllVerticesToWorld()
        {
            for (int i = 0; i < mVertex.Length; i++) mVertexWorld[i] = Vector3.Transform(mVertex[i], mWorld);
        }

        private void ComputeBoundingSphere()
        {
            mBoundSphere = new BoundingSphere();

            // Accumulate all the bounding spheres to find the total
            //foreach (ModelMesh mesh in mModel.Meshes)
            //{
            //    BoundingSphere meshBoundingSphere = mesh.BoundingSphere;
            //    BoundingSphere.CreateMerged(ref mBoundSphere, ref meshBoundingSphere, out mBoundSphere);
            //}
        }

        private void ComputeAABB()
        {
            // Compute Axis Aligned Boundig Box
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            for (int v = 0; v != mVertexWorld.Length; v++)
            {
                min = Vector3.Min(min, mVertexWorld[v]);
                max = Vector3.Max(max, mVertexWorld[v]);
            }

            // Arrondit Min aux entiers inférieurs et Max aux entiers supérieurs
            mMin.X = (float)Math.Floor(min.X);
            mMin.Y = (float)Math.Floor(min.Y);
            mMin.Z = (float)Math.Floor(min.Z);
            mMax.X = (float)Math.Ceiling(max.X);
            mMax.Y = (float)Math.Ceiling(max.Y);
            mMax.Z = (float)Math.Ceiling(max.Z);
        }

        private void TestAllVertices()
        {
            mL_VerticesTested = new List<int>();
            mL_MeshNewHeight = new List<float>();
            int w;
            for (int v = 0; v != mVertexWorld.Length; v++)
            {
                float h = waterMesh.GetWaterHeight(mVertexWorld[v]);
                if (mVertexWorld[v].Y <= h) w = 0;      // En dessous de l'eau
                else w = 1;  // Au dessus de l'eau

                mL_VerticesTested.Add(w);
                mL_MeshNewHeight.Add(mVertexWorld[v].Y - h);
            }
        }

        private void ListMeshTriToTest()
        {
            mL_MeshTriSubmerged = new List<int>();
            mL_MeshTriToTest = new List<int>();
            mL_MeshTriEmerged = new List<int>();

            for (int t = 0; t != mTri.Length; t++)
            {
                // Si tous les points sont en dessous de l'eau alors le triangle est sous l'eau
                if (mL_VerticesTested[mTri[t].I0] == 0 && mL_VerticesTested[mTri[t].I1] == 0 && mL_VerticesTested[mTri[t].I2] == 0)
                    mL_MeshTriSubmerged.Add(t);

                // Autrement si un des points est au dessous de l'eau, alors le triangle doit être testé
                else if ((mL_VerticesTested[mTri[t].I0] == 0 || mL_VerticesTested[mTri[t].I1] == 0 || mL_VerticesTested[mTri[t].I2] == 0))
                    mL_MeshTriToTest.Add(t);

                // Autrement tous les points sont au dessus de l'eau, alors le triangle ne doit pas être testé
                else mL_MeshTriEmerged.Add(t);
            }
        }

        private void ListWaterTriToTest()
        {
            mL_WaterTriToTest = new List<int>();
            int halfWidth = (int)((waterMesh.Options.SizeX - 1) / 2);
            int OffsetX = (waterMesh.Options.SizeX - 1) / 2;
            int OffsetZ = (waterMesh.Options.SizeZ - 1) / 2;
            int t;
            int deltaX = 1;
            int deltaY = 0;
            int debX = (int)mMin.X + OffsetX - deltaX;
            int finX = (int)mMax.X + OffsetX + deltaX;
            int debZ = (int)mMin.Z + OffsetZ - deltaY;
            int finZ = (int)mMax.Z + OffsetZ + deltaY;

            for (int x = debX; x != finX; x++)
            {
                for (int z = debZ; z != finZ; z++)
                {
                    t = 2 * (x * (halfWidth * 2) + z);

                    mL_WaterTriToTest.Add(t);
                    mL_WaterTriToTest.Add(t + 1);
                }
            }
        }

        private void ComputeIntersections()
        {
            Vector3 m0, m1, m2, w0, w1, w2;
            mD_TriPts = new Dictionary<int, List<Vector3>>();

            mL_VertexWorld = new List<Vector3>();
            for (int i = 0; i != mVertex.Length; i++) mL_VertexWorld.Add(mVertexWorld[i]);

            foreach (int m in mL_MeshTriToTest)
            {
                foreach (int w in mL_WaterTriToTest)
                {
                    // Triangle du mesh du bateau
                    m0 = mVertexWorld[mTri[m].I0];
                    m1 = mVertexWorld[mTri[m].I1];
                    m2 = mVertexWorld[mTri[m].I2];

                    // Triangle de l'eau
                    w0 = waterMesh.mVertex[waterMesh.mTri[w].I0];
                    w1 = waterMesh.mVertex[waterMesh.mTri[w].I1];
                    w2 = waterMesh.mVertex[waterMesh.mTri[w].I2];

                    // Si les 2 triangles ont une intersection
                    if (TriangleIntersection.Intersection_3d(m0, m1, m2, w0, w1, w2) == 1)
                    {
                        // Ajouter les 2 points d'intersection
                        if (TriangleIntersection.source != TriangleIntersection.target)
                        {
                            if (!mD_TriPts.ContainsKey(m))
                            {
                                List<Vector3> L = new List<Vector3>();
                                L.Add(TriangleIntersection.source);
                                L.Add(TriangleIntersection.target);
                                mD_TriPts.Add(m, L);
                            }
                            else
                            {
                                List<Vector3> L = new List<Vector3>();
                                mD_TriPts.TryGetValue(m, out L);
                                if (!L.Contains(TriangleIntersection.source)) L.Add(TriangleIntersection.source);
                                if (!L.Contains(TriangleIntersection.target)) L.Add(TriangleIntersection.target);
                                mD_TriPts[m] = L;
                            }
                        }
                    }
                }
            }
        }

        private static int CompareVectorByX(Vector3 u, Vector3 v)
        {
            if (u.X < v.X) return -1;
            else if (u.X == v.X) return 0;
            else return 1;
        }

        private static int CompareVectorByY(Vector3 u, Vector3 v)
        {
            if (u.Y < v.Y) return -1;
            else if (u.Y == v.Y) return 0;
            else return 1;
        }

        private static int CompareVectorByZ(Vector3 u, Vector3 v)
        {
            if (u.Z < v.Z) return -1;
            else if (u.Z == v.Z) return 0;
            else return 1;
        }

        private void CreateMeshNewTri()
        {
            mL_MeshNewTri = new List<Triangle>();
            mL_MeshNewVertex = new List<Vector3>();
            List<Vector3> ListTemp = new List<Vector3>();

            for (int v = 0; v != mVertexWorld.Length; v++) mL_MeshNewVertex.Add(mVertexWorld[v]);

            foreach (KeyValuePair<int, List<Vector3>> kv in mD_TriPts)
            {
                int t = kv.Key;
                List<Vector3> ListPtsIntersec = kv.Value;

                Triangle T;
                Color colorOfTri = mTri[t].color;
                // Vecteur normal de la face d'origine

                Vector3 uV = mVertexWorld[mTri[t].I1] - mVertexWorld[mTri[t].I0];
                Vector3 vV = mVertexWorld[mTri[t].I2] - mVertexWorld[mTri[t].I0];
                Vector3 a = Vector3.Cross(vV, uV);
                mTri[t].vNormal = Vector3.Normalize(a);

                #region 1 seul point en dessous de l'eau

                // ================================
                if (mL_VerticesTested[mTri[t].I0] + mL_VerticesTested[mTri[t].I1] + mL_VerticesTested[mTri[t].I2] == 2)
                {
                    // Détermination du pt en dessous de l'eau
                    int Ind = 0;
                    if (mL_VerticesTested[mTri[t].I0] == 0) Ind = mTri[t].I0;
                    else if (mL_VerticesTested[mTri[t].I1] == 0) Ind = mTri[t].I1;
                    else if (mL_VerticesTested[mTri[t].I2] == 0) Ind = mTri[t].I2;

                    // Tri des points d'intersection (mer - mesh)
                    Vector3 Min, Max;
                    Min.X = Min.Y = Min.Z = float.MaxValue;
                    Max.X = Max.Y = Max.Z = float.MinValue;
                    foreach (Vector3 v in ListPtsIntersec)
                    {
                        if (v.X < Min.X) Min.X = v.X;
                        if (v.Y < Min.Y) Min.Y = v.Y;
                        if (v.Z < Min.Z) Min.Z = v.Z;
                        if (v.X > Max.X) Max.X = v.X;
                        if (v.Y > Max.X) Max.Y = v.Y;
                        if (v.Z > Max.Z) Max.Z = v.Z;
                    }

                    Vector3 Delta;
                    Delta.X = Max.X - Min.X;
                    Delta.Y = Max.Y - Min.Y;
                    Delta.Z = Max.Z - Min.Z;

                    if (Delta.X > Delta.Y && Delta.X > Delta.Z) ListPtsIntersec.Sort(CompareVectorByX);
                    else if (Delta.Y > Delta.X && Delta.Y > Delta.Z) ListPtsIntersec.Sort(CompareVectorByY);
                    else if (Delta.Z > Delta.X && Delta.Z > Delta.Y) ListPtsIntersec.Sort(CompareVectorByZ);

                    // Création des nouveaux triangles
                    for (int p = 0; p != ListPtsIntersec.Count - 1; p++)
                    {
                        T = new Triangle();
                        T.I0 = Ind;
                        mL_MeshNewVertex.Add(ListPtsIntersec[p]); mL_MeshNewHeight.Add(0f);
                        T.I1 = mL_MeshNewVertex.Count - 1;
                        mL_MeshNewVertex.Add(ListPtsIntersec[p + 1]); mL_MeshNewHeight.Add(0f);
                        T.I2 = mL_MeshNewVertex.Count - 1;
                        T.color = colorOfTri;
                        T.vNormal = mTri[t].vNormal;
                        mL_MeshNewTri.Add(T);
                    }
                }


                // ============================
                else
                {
                    int Ind0 = 0;
                    int Ind1 = 0;
                    int IndAboveWater = 0;

                    // Détermination des 2 pts en dessous de l'eau
                    if (mL_VerticesTested[mTri[t].I0] == 0 && mL_VerticesTested[mTri[t].I1] == 0 && mL_VerticesTested[mTri[t].I2] == 1)
                    {
                        Ind0 = mTri[t].I0;
                        Ind1 = mTri[t].I1;
                        IndAboveWater = mTri[t].I2;
                    }
                    else if (mL_VerticesTested[mTri[t].I0] == 0 && mL_VerticesTested[mTri[t].I1] == 1 && mL_VerticesTested[mTri[t].I2] == 0)
                    {
                        Ind0 = mTri[t].I0;
                        Ind1 = mTri[t].I2;
                        IndAboveWater = mTri[t].I1;
                    }
                    else if (mL_VerticesTested[mTri[t].I0] == 1 && mL_VerticesTested[mTri[t].I1] == 0 && mL_VerticesTested[mTri[t].I2] == 0)
                    {
                        Ind0 = mTri[t].I1;
                        Ind1 = mTri[t].I2;
                        IndAboveWater = mTri[t].I0;
                    }

                    // Tri des points d'intersection (mer - mesh)
                    Vector3 Min, Max;
                    Min.X = Min.Y = Min.Z = float.MaxValue;
                    Max.X = Max.Y = Max.Z = float.MinValue;
                    foreach (Vector3 v in ListPtsIntersec)
                    {
                        if (v.X < Min.X) Min.X = v.X;
                        if (v.Y < Min.Y) Min.Y = v.Y;
                        if (v.Z < Min.Z) Min.Z = v.Z;
                        if (v.X > Max.X) Max.X = v.X;
                        if (v.Y > Max.X) Max.Y = v.Y;
                        if (v.Z > Max.Z) Max.Z = v.Z;
                    }

                    Vector3 Delta;
                    Delta.X = Max.X - Min.X;
                    Delta.Y = Max.Y - Min.Y;
                    Delta.Z = Max.Z - Min.Z;

                    if (Delta.X > Delta.Y && Delta.X > Delta.Z) ListPtsIntersec.Sort(CompareVectorByX);
                    else if (Delta.Y > Delta.X && Delta.Y > Delta.Z) ListPtsIntersec.Sort(CompareVectorByY);
                    else if (Delta.Z > Delta.X && Delta.Z > Delta.Y) ListPtsIntersec.Sort(CompareVectorByZ);

                    float dist0 = DistancePt2Line(mVertexWorld[Ind0], mVertexWorld[IndAboveWater], ListPtsIntersec[0]);
                    float dist1 = DistancePt2Line(mVertexWorld[Ind1], mVertexWorld[IndAboveWater], ListPtsIntersec[0]);
                    if (dist0 > dist1)
                    {
                        int swap = Ind0;
                        Ind0 = Ind1;
                        Ind1 = swap;
                    }

                    if (ListPtsIntersec.Count < 3)
                    {
                        // Il n'y a que 2 triangles à ajouter
                        //  +---+
                        //  |  /|
                        //  | / |
                        //  +---+
                        // Premier triangle
                        T = new Triangle();
                        T.I0 = Ind0;
                        mL_MeshNewVertex.Add(ListPtsIntersec[0]); mL_MeshNewHeight.Add(0f);
                        T.I1 = mL_MeshNewVertex.Count - 1;
                        mL_MeshNewVertex.Add(ListPtsIntersec[1]); mL_MeshNewHeight.Add(0f);
                        T.I2 = mL_MeshNewVertex.Count - 1;
                        T.color = colorOfTri;
                        T.vNormal = mTri[t].vNormal;
                        mL_MeshNewTri.Add(T);
                        // Second triangle
                        T = new Triangle();
                        T.I0 = Ind0;
                        T.I1 = mL_MeshNewVertex.Count - 1;
                        T.I2 = Ind1;
                        T.color = colorOfTri;
                        T.vNormal = mTri[t].vNormal;
                        mL_MeshNewTri.Add(T);
                    }
                    else
                    {
                        //  +---+---+---+
                        //  |  /|  /|  /|
                        //  | / | / | / |
                        //  +---+---+---+
                        // Création d'une liste de pts espacés entre Ind0 et Ind1 stockés dans ListTemp
                        ListTemp.Clear();
                        float NbIntervalles = ListPtsIntersec.Count - 1;
                        Vector3 intervalle = (mVertexWorld[Ind1] - mVertexWorld[Ind0]) / NbIntervalles;
                        for (int p = 1; p != ListPtsIntersec.Count - 1; p++)
                        {
                            Vector3 temp = mVertexWorld[Ind0] + intervalle * p;
                            ListTemp.Add(temp);
                        }
                        // Création des triangles
                        // Premier triangle
                        T = new Triangle();
                        T.I0 = Ind0;
                        mL_MeshNewVertex.Add(ListPtsIntersec[0]); mL_MeshNewHeight.Add(0f);
                        T.I1 = mL_MeshNewVertex.Count - 1;
                        mL_MeshNewVertex.Add(ListPtsIntersec[1]); mL_MeshNewHeight.Add(0f);
                        T.I2 = mL_MeshNewVertex.Count - 1;
                        T.color = colorOfTri;
                        T.vNormal = mTri[t].vNormal;
                        mL_MeshNewTri.Add(T);
                        // Deuxième triangle
                        T = new Triangle();
                        T.I0 = Ind0;
                        T.I1 = mL_MeshNewVertex.Count - 1;
                        mL_MeshNewVertex.Add(ListTemp[0]); mL_MeshNewHeight.Add(ListTemp[0].Y - waterMesh.GetWaterHeight(ListTemp[0]));
                        T.I2 = mL_MeshNewVertex.Count - 1;
                        T.color = colorOfTri;
                        T.vNormal = mTri[t].vNormal;
                        mL_MeshNewTri.Add(T);

                        // Triangles suivants
                        if (ListTemp.Count > 3)
                        {
                            for (int p = 1; p != ListTemp.Count - 2; p++)
                            {
                                // Premier triangle
                                T = new Triangle();
                                T.I0 = mL_MeshNewVertex.Count - 1;
                                T.I1 = mL_MeshNewVertex.Count - 2;
                                mL_MeshNewVertex.Add(ListPtsIntersec[1 + p]); mL_MeshNewHeight.Add(0f);
                                T.I2 = mL_MeshNewVertex.Count - 1;
                                T.color = colorOfTri;
                                T.vNormal = mTri[t].vNormal;
                                mL_MeshNewTri.Add(T);
                                // Deuxième triangle
                                T = new Triangle();
                                T.I0 = mL_MeshNewVertex.Count - 2;
                                T.I1 = mL_MeshNewVertex.Count - 1;
                                mL_MeshNewVertex.Add(ListTemp[p]); mL_MeshNewHeight.Add(ListTemp[p].Y - waterMesh.GetWaterHeight(ListTemp[p]));
                                T.I2 = mL_MeshNewVertex.Count - 1;
                                T.color = colorOfTri;
                                T.vNormal = mTri[t].vNormal;
                                mL_MeshNewTri.Add(T);
                            }
                        }
                        // Avant dernier triangle
                        T = new Triangle();
                        T.I0 = mL_MeshNewVertex.Count - 1;
                        T.I1 = mL_MeshNewVertex.Count - 2;
                        mL_MeshNewVertex.Add(ListPtsIntersec[ListPtsIntersec.Count - 1]); mL_MeshNewHeight.Add(0f);
                        T.I2 = mL_MeshNewVertex.Count - 1;
                        T.color = colorOfTri;
                        T.vNormal = mTri[t].vNormal;
                        mL_MeshNewTri.Add(T);
                        // Dernier triangle
                        T = new Triangle();
                        T.I0 = mL_MeshNewVertex.Count - 2;
                        T.I1 = mL_MeshNewVertex.Count - 1;
                        T.I2 = Ind1;
                        T.color = colorOfTri;
                        T.vNormal = mTri[t].vNormal;
                        mL_MeshNewTri.Add(T);
                    }
                }

                #endregion 2 points en dessous de l'eau
            }
        }

        private float DistancePt2Line(Vector3 A, Vector3 B, Vector3 P)
        {
            // Retourne la distance entre le point P et la ligne définie par les pts A et B

            // La ligne est définie par l'équation L(t) = A + t * Direction
            Vector3 Direction = B - A;

            // Le pt le plus proche de P sur la ligne est la projection de P sur la ligne, Q = A + t0 * Direction, où
            // t0 = Direction ·(P − A) / Direction · Direction
            float t0 = Vector3.Dot(Direction, P - A) / Vector3.Dot(Direction, Direction);

            if (t0 <= 0) return (P - A).Length();
            else if (t0 >= 1) return (P - B).LengthSquared();
            else return (P - (A + t0 * Direction)).LengthSquared();
        }

        private void ComputeArchimede()
        {
            mL_AllTriWetted = new List<TriangleWetted>();
            AreaWetted = 0f;

            #region Fusion des deux listes de triangles immergés

            // i.e. les nouveaux créés par intersection avec la surface de l'eau et ceux qui sont complètement immergés

            foreach (Triangle T in mL_MeshNewTri)
            {
                TriangleWetted TW = new TriangleWetted();
                TW.I0 = T.I0;
                TW.I1 = T.I1;
                TW.I2 = T.I2;
                TW.color = T.color;
                TW.bNoChange = false;
                Vector3 u = mL_MeshNewVertex[T.I1] - mL_MeshNewVertex[T.I0];
                Vector3 v = mL_MeshNewVertex[T.I2] - mL_MeshNewVertex[T.I0];
                Vector3 a = Vector3.Cross(v, u);
                TW.fArea = (float)(0.5 * Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z));
                AreaWetted += TW.fArea;

                // Culling check
                if (TW.fArea != 0f)
                {
                    Vector3 b = Vector3.Cross(u, v);
                    Vector3 va = a - T.vNormal;
                    Vector3 vb = b - T.vNormal;
                    // Si le nouveau triangle est dans le même de rotation des indices, alors les vecteurs a et T.vNormal sont dans le même sens,
                    // donc va (la différence) doit être sensiblement nul. Si tel n'est pas le cas, c'est que le sens de rotation des indices
                    // doit être changé en intervertissant deux indices.
                    if (va.LengthSquared() > vb.LengthSquared())
                    {
                        TW.I1 = T.I2;
                        TW.I2 = T.I1;
                        TW.vNormal = -1 * a;
                    }
                    else TW.vNormal = a;
                    TW.vNormal.Normalize();
                }
                else TW.vNormal = Vector3.Zero;

                TW.pCPressure = (mL_MeshNewVertex[T.I0] + mL_MeshNewVertex[T.I1] + mL_MeshNewVertex[T.I2]) / 3;
                mL_AllTriWetted.Add(TW);
            }
            foreach (int i in mL_MeshTriSubmerged)
            {
                TriangleWetted TW = new TriangleWetted();
                TW.I0 = mTri[i].I0;
                TW.I1 = mTri[i].I1;
                TW.I2 = mTri[i].I2;
                TW.color = mTri[i].color;
                TW.bNoChange = true;
                TW.fArea = mTri[i].fArea;
                AreaWetted += TW.fArea;
                Vector3 u = mL_MeshNewVertex[TW.I1] - mL_MeshNewVertex[TW.I0];
                Vector3 v = mL_MeshNewVertex[TW.I2] - mL_MeshNewVertex[TW.I0];
                Vector3 a = Vector3.Cross(v, u);
                TW.vNormal = Vector3.Normalize(a);
                TW.pCPressure = (mL_MeshNewVertex[TW.I0] + mL_MeshNewVertex[TW.I1] + mL_MeshNewVertex[TW.I2]) / 3;
                mL_AllTriWetted.Add(TW);
            }

            #endregion Fusion des deux listes de triangles immergés

            // Compute Archimede Force
            // =======================
            Archimede = new Force();
            Archimede.Position = Position;
            Archimede.Vector = Vector3.Zero;
            Archimede.Magnitude = 0;
            if (mL_AllTriWetted.Count > 0)
            {
                float tmpSumPressure = 0;
                for (int tw = 0; tw != mL_AllTriWetted.Count; tw++)
                {
                    TriangleWetted TW = mL_AllTriWetted[tw];
                    TW.Depth = -(mL_MeshNewHeight[TW.I0] + mL_MeshNewHeight[TW.I1] + mL_MeshNewHeight[TW.I2]) / 3f;
                    TW.fPressure = mWATERDENSITY * mGRAVITY * TW.Depth * TW.fArea;
                    TW.vPressure = -TW.vNormal * TW.fPressure;
                    mL_AllTriWetted[tw] = TW;

                    Archimede.Vector += TW.vPressure;
                    Archimede.Position += TW.pCPressure * TW.fPressure;
                    tmpSumPressure += TW.fPressure;
                }
                Archimede.Vector.X = Archimede.Vector.Z = 0;
                Archimede.Position /= tmpSumPressure;
                Archimede.Magnitude = Archimede.Vector.Length();
            }
        }

        private void ComputeAllForces(SimulationTime time)
        {
            //Deltatime in seconds
            float deltaTime = (float)time.DeltaTime * 1000f;
            Vector3 Thrust = new Vector3(0f, 0f, 0f);

            // Gravity
            Matrix M = Matrix.CreateFromYawPitchRoll(mRotationY, mRotationX, mRotationZ);
            Vector3 temp = mCentroid + GravityOffset;
            temp = Vector3.Transform(temp, M);

            Gravity.Position = mPosition + temp;
            Gravity.Magnitude = mMass * mGRAVITY;
            Gravity.Vector = Gravity.Position + new Vector3(0, -Gravity.Magnitude, 0);

            // Frictional resistance
            Vector3 Resistance = Vector3.Zero;
            float friction = 0;
            if (AreaWetted > 0)
            {
                friction = mMass * Velocity.Y * AreaXZ_RacCub * AreaWetted / AreaWettedMax;
                Resistance = Archimede.Position + new Vector3(0, -friction, 0);
            }

            // Total = Gravity + Archimede + Friction
            Total = new Force();
            Total.Vector = Gravity.Vector + Archimede.Vector + Resistance + Thrust;

            Vector3 Acc = Total.Vector / mMass;
            Velocity += Acc * deltaTime;
            Position += Velocity * deltaTime;

            // Rotations / Inerties
            Vector3 AG = new Vector3(Archimede.Position.X - Gravity.Position.X, 0f, Archimede.Position.Z - Gravity.Position.Z);
            M = Matrix.Invert(M);
            AG = Vector3.Transform(AG, M);
            float force = (Archimede.Magnitude + Gravity.Magnitude) * 0.5f;

            // CoupleZ
            float accRotZ = 0.001f * AG.X * force / Izz;
            float deltaZ = 0.5f;
            accRotZ -= AngularVelocity.Z * deltaZ;

            AngularVelocity.Z += accRotZ * deltaTime;
            mRotationZ += AngularVelocity.Z * deltaTime;

            // CoupleX
            float accRotX = 0.001f * -AG.Z * force / Ixx;
            float deltaX = 0.5f;
            accRotX -= AngularVelocity.X * deltaX;

            AngularVelocity.X += accRotX * deltaTime;
            mRotationX += AngularVelocity.X * deltaTime;
        }
    }
}