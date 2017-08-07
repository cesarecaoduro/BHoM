﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BH.oM.Geometry
{
    public class GeometryGroup 
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public List<IBHoMGeometry> Elements = new List<IBHoMGeometry>();


        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public GeometryGroup() { }

        /***************************************************/

        public GeometryGroup(IEnumerable<IBHoMGeometry> elements)
        {
            Elements = elements.ToList();
        }

    }
}

