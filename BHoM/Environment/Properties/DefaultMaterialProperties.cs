﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Base;
using BH.oM.Environment.Interface;

namespace BH.oM.Environment.Properties
{
    public class DefaultMaterialProperties : BHoMObject, IMaterialProperties
    {
        public double ThermalConductivity { get; set; } = 0.0;
    }
}