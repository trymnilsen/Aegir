﻿using AegirLib.Keyframe;
using AegirLib.Persistence;
using AegirLib.Scene;
using AegirLib.Simulation;
using AegirLib.MathType;
using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AegirLib.Behaviour.World
{
    public class Transform : BehaviourComponent, ICustomPersistable
    {
        private Vector3 localPosition;
        private Quaternion localRotation;
        private Vector3 worldPosition;
        private Quaternion worldRotation;
        private Matrix matrix;
        private bool matrixIsDirty;

        private float roll;
        private float pitch;
        private float yaw;
        private bool rotationIsDirty;

        public bool Notify { get; set; }

        [KeyframeProperty]
        public Vector3 LocalPosition
        {
            get { return localPosition; }
            set
            {
                localPosition = value;
                matrixIsDirty = true;
            }
        }

        [KeyframeProperty]
        public Quaternion LocalRotation
        {
            get { return localRotation; }
            set
            {
                localRotation = value;
                matrixIsDirty = true;
                rotationIsDirty = true;
            }
        }

        public Vector3 WorldPosition
        {
            get
            {
                return worldPosition;
            }
        }

        public Quaternion WorldRotation
        {
            get
            {
                return worldRotation;
            }
        }

        public Matrix TransformMatrix
        {
            get
            {
                if(matrixIsDirty) { UpdateMatrix(); }
                return matrix;
            }
        }

        public float Roll
        {
            get
            {
                if(rotationIsDirty) { GenerateRPY(); }
                return roll;
            }
            set
            {
                Quaternion q = Quaternion.CreateFromYawPitchRoll(YawRadians, PitchRadians, MathHelper.ToRadians(value));
                localRotation = localRotation * q;
                localRotation.Normalize();
                rotationIsDirty = true;
                matrixIsDirty = true;
            }
        }


        public float Pitch
        {
            get
            {
                if (rotationIsDirty) { GenerateRPY(); }
                return pitch;
            }
            set
            {
                Quaternion q = Quaternion.CreateFromYawPitchRoll(YawRadians, MathHelper.ToRadians(value), RollRadians);
                localRotation = localRotation * q;
                localRotation.Normalize();
                rotationIsDirty = true;
                matrixIsDirty = true;
            }
        }

        public float Yaw
        {
            get
            {
                if(rotationIsDirty) { GenerateRPY(); }
                return yaw;
            }
            set
            {
                Quaternion q = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(value), PitchRadians, RollRadians);
                localRotation = localRotation * q;
                localRotation.Normalize();
                rotationIsDirty = true;
                matrixIsDirty = true;
            }
        }
        public float RollRadians => MathHelper.ToRadians(Roll);
        public float YawRadians => MathHelper.ToRadians(Yaw);
        public float PitchRadians => MathHelper.ToRadians(Pitch);

        public Transform(Entity parent)
            : base(parent)
        {
            localPosition = new Vector3();
            LocalRotation = new Quaternion();
            matrix = Matrix.Identity;
        }

        public void SetX(double x)
        {
            localPosition.X = (float)x;
            matrixIsDirty = true;
        }

        public void SetY(double y)
        {
            localPosition.Y = (float)y;
            matrixIsDirty = true;

        }

        public void SetZ(double z)
        {
            localPosition.Z = (float)z;
            matrixIsDirty = true;

        }

        public void Translate(Vector3 vector)
        {
            LocalPosition = LocalPosition + vector;
        }

        public void TranslateX(double amount)
        {
            LocalPosition = LocalPosition + new Vector3((float)amount, 0, 0);
        }

        public void TranslateY(double amount)
        {
            LocalPosition = LocalPosition + new Vector3(0, (float)amount, 0);
        }

        public void TranslateZ(double amount)
        {
            LocalPosition = LocalPosition + new Vector3(0, 0, (float)amount);
        }

        public void RotateHeading(double newHeading)
        {
            LocalRotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)newHeading);
        }

        public void SetOrientation(double yaw, double pitch, double roll)
        {
            LocalRotation = Quaternion.CreateFromYawPitchRoll((float)yaw, (float)pitch, (float)roll);
        }

        public override string ToString()
        {
            return "Transform";
        }

        public override void PreUpdate(SimulationTime time)
        {
            Transform parentTransform = Parent?.Parent?.Transform;
            if (parentTransform != null)
            {
                this.worldPosition = parentTransform.WorldPosition + localPosition;
                this.worldRotation = parentTransform.WorldRotation * localRotation;
            }
            else
            {
                //For root entities we do not have a parent, therefore we treat the local space as our world space
                this.worldPosition = localPosition;
                this.worldRotation = localRotation;
            }
            base.Update(time);
        }

        public override XElement Serialize()
        {
            //We need to serialize both position and rotation so lets create a wrapper to keep them
            XElement transformContainer = new XElement(GetType().Name);
            XElement positionElement = XElementSerializer.SerializeToXElement(localPosition);
            XElement rotationElement = XElementSerializer.SerializeToXElement(localRotation);

            transformContainer.Add(positionElement);
            transformContainer.Add(rotationElement);

            return transformContainer;
        }

        public override void Deserialize(XElement data)
        {
            XElement positionElement = data.Element(localPosition.GetType().Name);
            XElement rotationElement = data.Element(localRotation.GetType().Name);

            if (positionElement == null)
            {
                throw new PersistanceException("Transform element of entity does not have a position element");
            }
            if (rotationElement == null)
            {
                throw new PersistanceException("Transform element of entity does not have a rotation element");
            }

            localPosition = XElementSerializer.DeserializeFromXElement<Vector3>(positionElement);
            localRotation = XElementSerializer.DeserializeFromXElement<Quaternion>(rotationElement);
        }
        private void UpdateMatrix()
        {
            matrixIsDirty = false;
            Matrix m = Matrix.CreateFromQuaternion(worldRotation);
            m.Translation = worldPosition;

            matrix = m;
        }
        private void GenerateRPY()
        {
            rotationIsDirty = false;
            roll = MathHelper.ToDegrees((float)Quaternion.GetXAngle(localRotation));
            pitch = MathHelper.ToDegrees((float)Quaternion.GetYAngle(localRotation));
            yaw = MathHelper.ToDegrees((float)Quaternion.GetZAngle(localRotation));
        }


    }
}