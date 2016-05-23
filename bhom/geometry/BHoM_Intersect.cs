﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHoM.Geometry
{
    public static class Intersect
    {
        public static List<Point> PlaneCurve(Plane p, Curve c)
        { 
            List<Point> result = new List<Point>();
            List<int> sameSide = p.SameSide(c.ControlPointVector);

            if (sameSide.Count < c.PointCount)
            {
                int i1 = 0;
                int i2 = 0;

                while (i1 < sameSide.Count - 1 && sameSide[i1 + 1] - sameSide[i1] == 1) i1++;
                for (int i = i1; i < sameSide.Count; i++)
                {
                    if (sameSide[i] - i2 == 1)
                    {
                        i1 = i2;
                        i2 = sameSide[i];
                        while (i < sameSide.Count - 1 && sameSide[i + 1] - sameSide[i] == 1) i++;
                    }
                    else if (i == sameSide.Count - 1 || sameSide[i + 1] - sameSide[i] > 1)
                    {
                        i1 = sameSide[i];
                        i2 = sameSide[i] + 1;
                        while (i < sameSide.Count - 1 && i2 < sameSide[i + 1] - 1) i2++;
                    }
                    else continue;
                    double maxT = c.Knots[i1 + c.Degree + 1];
                    double minT = c.Knots[i1 + 1];
                    result.Add(new Point(CurveParameterAtPlane(p, c, minT, maxT, c.PointAt(minT), c.PointAt(maxT))));
                }
            }
           
            return result;
        }

        public static List<Point> CurveCurve(Curve c1, Curve c2)
        {
            List<Point> result = new List<Point>();
            if (BoundingBox.InRange(c1.Bounds(), c2.Bounds()))
            {
                Point p11 = c1.ControlPoint(0);
                for (int i = 1; i < c1.PointCount; i++)
                {
                    Point p12 = c1.ControlPoint(i);
                    Point p21 = c2.ControlPoint(0);
                    for (int j = 1; j < c2.PointCount; j++)
                    {
                        Point p22 = c2.ControlPoint(j);
                        double[] intersectPoint = VectorUtils.Intersect(p11, p12 - p11, p21, p22 - p21, true);
                        if (intersectPoint != null)
                        {
                            double maxT1 = c1.Knots[i + c1.Degree];
                            double minT1 = c1.Knots[i];

                            double maxT2 = c2.Knots[j + c2.Degree];
                            double minT2 = c2.Knots[j];

                            double[] p1Max = c1.PointAt(maxT1);
                            double[] p1Min = c1.PointAt(minT1);

                            double[] p2Max = c2.PointAt(maxT2);
                            double[] p2Min = c2.PointAt(minT2);

                            double[] cP1 = p1Min;// : CurveNearestPoint(c1, intersectPoint, minT1, maxT1, p1Min, p1Max);
                            double[] cP2 = intersectPoint;// : CurveNearestPoint(c2, intersectPoint, minT2, maxT2, p2Min, p2Max);

                            int interations = 0;
                            while (interations++ < 20 && !VectorUtils.Equal(cP1, cP2, 0.001))
                            {
                                UpdateNearestEnd(c1, cP2, ref minT1, ref maxT1, ref p1Min, ref p1Max);
                                cP1 = VectorUtils.Average(p1Max, p1Min);
                                UpdateNearestEnd(c2, cP1, ref minT2, ref maxT2, ref p2Min, ref p2Max);
                                cP2 = VectorUtils.Average(p2Max, p2Min);
                                
                            }
                            //if (VectorUtils.Equal(cP1, cP2, 0.001))
                            result.Add(new Point(cP1));
                        }
                        p21 = p22;
                    }
                    p11 = p12;
                }
            }
            return result;
        }

        private static void UpdateNearestEnd(Curve c, double[] pointComparison, ref double minT, ref double maxT, ref double[] pMin, ref double[] pMax)
        {
            double distance1 = VectorUtils.LengthSq(VectorUtils.Sub(pointComparison, pMin));
            double distance2 = VectorUtils.LengthSq(VectorUtils.Sub(pointComparison, pMax));
            double distanceRatio = distance1 / distance2;
            if (distanceRatio < 1.01 && distanceRatio > 0.99)
            {
                double midQuart = (maxT - minT) / 4;
                minT = minT + midQuart;
                maxT = maxT - midQuart;
                pMin = c.PointAt(minT);
                pMax = c.PointAt(maxT);

            }
            else
            {
                double mid = (minT + maxT) / 2;
                double[] midP = c.PointAt(mid);

                if (distance1 < distance2)//(InRange(pointComparison, pMin, midP))
                {
                    maxT = mid;
                    pMax = midP;
                }
                else
                {
                    minT = mid;
                    pMin = midP;
                }
            }
        }

        private static bool InRange(double[] value, double[] min, double[] max)
        {
            for (int i = 0; i < value.Length;i++)
            {
                if (value[i] >= min[i] && value[i] <= max[i]) continue;
                if (value[i] >= max[i] && value[i] <= min[i]) continue;
                return false;
            }
            return true;
        }

        private static double[] CurveNearestPoint(Curve c, double[] point, double minT, double maxT, double[] p1, double[] p2)
        {
            
            if (VectorUtils.Equal(p1, p2, 0.1)) return p1;

            double mid = (minT + maxT) / 2;

            double[] p3 = c.PointAt(mid);

            double distance1 = VectorUtils.LengthSq(VectorUtils.Sub(point, p1));
            double distance2 = VectorUtils.LengthSq(VectorUtils.Sub(point, p2));
            if (distance1 > distance2)
            {
                return CurveNearestPoint(c, point, mid, maxT, p3, p2);
            }
            else
            {
                return CurveNearestPoint(c, point, minT, mid, p1, p3);
            }
        }

        private static double[] CurveParameterAtPlane(Plane p, Curve c, double minT, double maxT, double[] p1, double[] p2)
        {
            double mid = (minT + maxT) / 2;

            //double[] p1 = c.PointAt(minT);
            double[] p3 = c.PointAt(mid);
            if (p.InPlane(p3, 4)) return p3;
            
            if (p.IsSameSide(p1, p3))
            {
                return CurveParameterAtPlane(p, c, mid, maxT, p3, p2);
            }
            else
            {
                return CurveParameterAtPlane(p, c, minT, mid, p1, p3);
            }
        }
    }
}
