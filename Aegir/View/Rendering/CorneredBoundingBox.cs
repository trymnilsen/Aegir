using Aegir.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aegir.View.Rendering
{

    public class CorneredBoundingBox
    {
        internal class BoundingBoxCorner
        {

        }
        //upper corners 
        BoundingBoxCorner x0y0z0Corner; 
        BoundingBoxCorner x1y0z0Corner; 
        BoundingBoxCorner x1y1z0Corner; 
        BoundingBoxCorner x0y1z0Corner;

        //lower corners 
        BoundingBoxCorner x0y0z1Corner;
        BoundingBoxCorner x1y0z1Corner;
        BoundingBoxCorner x1y1z1Corner;
        BoundingBoxCorner x0y1z1Corner;

        public CorneredBoundingBox()
        {

        }

    }
}
