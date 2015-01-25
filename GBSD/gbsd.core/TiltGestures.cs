using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GestureTest
{

    

    public class TiltGestures
    {
        Move flatMove = new Move(0.05, -0.68, -0.7);
        Move rightMove = new Move(0.35, 0, -0.7);
        Move leftMove = new Move(-0.38, -0.65, -0.50);
        Move upMove = new Move(0, -0.23, -0.90);
        Move downMove = new Move(0, -0.80, -0.5); 



        public Microsoft.Xna.Framework.Vector3 accelReading{get; set;}



        /* public MoveGesture(Microsoft.Xna.Framework.Vector3 accelReading)
         {
             // TODO: Complete member initialization
             this.accelReading = accelReading;
         }

         public MoveGesture(double X, double Y, double Z)
         {
             accelReading.X = (float)X;
             accelReading.Y = (float)Y;
             accelReading.Z = (float)Z;

         }*/

        public bool isRightTilt()
        {
            if (accelReading.X > rightMove.xLimit
                //&& accelReading.Y //ignore
                && accelReading.Z > rightMove.zLimit)
            {
                return true;
            }
            return false;
        }

        public bool isLeftTilt()
        {

            if (accelReading.X < leftMove.xLimit
                && accelReading.Y < leftMove.yLimit
                && accelReading.Z > leftMove.zLimit)
            {
                return true;
            }
            return false;
        }

        public bool isDownTilt()
        {

            if (//accelReading.X < leftMove.xLimit  //ignore
                accelReading.Y < downMove.yLimit
                && accelReading.Z > downMove.zLimit)
            {
                return true;
            }

            return false;

        }

        public bool isUpTilt()
        {

            if (//accelReading.X < leftMove.xLimit //ignore
                   accelReading.Y > upMove.yLimit
                    && accelReading.Z < upMove.zLimit)
            {
                return true;
            }

            return false;

        }


        public bool isNoTilt()
        {

            if (!isDownTilt() && !isUpTilt() && !isLeftTilt() && !isRightTilt())
            {
                return true;
            }
            
            return false;

        }
    }

    public class Move
    {
        public double xLimit { get; set; }
        public double yLimit { get; set; }
        public double zLimit { get; set; }

        public Move(double x, double y, double z)
        {
            xLimit = x;
            yLimit = y;
            zLimit = z;
        }

    }
}
