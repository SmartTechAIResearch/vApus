﻿using System;
using System.Collections.Generic;
using SLA = SpreadsheetLight.Drawing;

namespace SpreadsheetLight.Charts
{
    /// <summary>
    /// Encapsulates properties and methods for setting the side wall of 3D charts.
    /// This simulates the DocumentFormat.OpenXml.Drawing.Charts.SideWall class.
    /// </summary>
    public class SLSideWall
    {
        // From the Open XML SDK documentation:
        // "This element specifies the thickness of the walls or floor as a percentage of the largest dimension of the plot volume."
        // I have no idea what that means... and Excel doesn't allow the user to set this. Hmm...
        internal byte Thickness { get; set; }

        internal SLA.SLShapeProperties ShapeProperties;

        /// <summary>
        /// Fill properties.
        /// </summary>
        public SLA.SLFill Fill { get { return this.ShapeProperties.Fill; } }

        /// <summary>
        /// Border properties.
        /// </summary>
        public SLA.SLLinePropertiesType Border { get { return this.ShapeProperties.Outline; } }

        /// <summary>
        /// Shadow properties.
        /// </summary>
        public SLA.SLShadowEffect Shadow { get { return this.ShapeProperties.EffectList.Shadow; } }

        /// <summary>
        /// Glow properties.
        /// </summary>
        public SLA.SLGlow Glow { get { return this.ShapeProperties.EffectList.Glow; } }

        /// <summary>
        /// Soft edge properties.
        /// </summary>
        public SLA.SLSoftEdge SoftEdge { get { return this.ShapeProperties.EffectList.SoftEdge; } }

        internal SLSideWall(List<System.Drawing.Color> ThemeColors)
        {
            this.Thickness = 0;
            this.ShapeProperties = new SLA.SLShapeProperties(ThemeColors);
        }

        internal SLSideWall Clone()
        {
            SLSideWall sw = new SLSideWall(this.ShapeProperties.listThemeColors);
            sw.Thickness = this.Thickness;
            sw.ShapeProperties = this.ShapeProperties.Clone();

            return sw;
        }
    }
}