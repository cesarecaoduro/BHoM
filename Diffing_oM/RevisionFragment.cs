/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.oM.Diffing
{
    [Description("Stores the current and previous Hash of the parent BHoMObject, as assigned after the inclusion in a Revision.")]
    public class RevisionFragment : IHashFragment, IImmutable
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Current object Hash.")]
        public string CurrentHash { get; }

        [Description("Previous object Hash, typically the one it had in its previous Revision.")]
        public string PreviousHash { get; } = null;

        /***************************************************/
        /**** Constructor                               ****/
        /***************************************************/

        public RevisionFragment(string currentHash, string previousHash = null)
        {
            CurrentHash = currentHash;
            PreviousHash = previousHash;
        }

        /***************************************************/
    }
}
