using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * <summary>
 *  A helper/util class that helps with geometry.
 * </summary>
 */
public static class UGeometry
{
    public class Triangle
    {
        private float m_l1,  m_l2, m_l3; // lenght
        private Vector2 m_a1,  m_a2, m_a3; // angle
        private Vector2 m_p1,  m_p2, m_p3; // points

        public Triangle(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            setPoints(p1, p2, p3);
        }

        public Vector2 getPoint(int index)
        {
            switch (index)
            {
                case 0:
                    return m_p1;
                case 1:
                    return m_p2;
                case 2:
                    return m_p3;
                default:
                    throw new IndexOutOfRangeException("UGeometry::Triangle::getPoint: \'index\' parameter is out of the 0-2 range (a triangle only has 3 sides so the max index is 2)");
            }
        }

        public void setPoints(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            m_p1 = p1;
            m_p2 = p2;
            m_p3 = p3;

            m_l1 = calculateLen(m_p1, m_p2);
            m_l2 = calculateLen(m_p2, m_p3);
            m_l3 = calculateLen(m_p3, m_p1);

            m_a1 = angleToVec2(calculateAngle(m_p1, m_p2, m_p3));
            m_a2 = angleToVec2(calculateAngle(m_p2, m_p3, m_p1));
            m_a3 = angleToVec2(calculateAngle(m_p3, m_p1, m_p2));
        }

        private bool oneEquals<T>(T a1, T a2, T c1, T c2)
        {
            return (a1.Equals(c1) && a2.Equals(c2)) || (a1.Equals(c2) && a2.Equals(c1));
        }
        
        public float getLen(Vector2 p1, Vector2 p2)
        {
            if (oneEquals(p1, p2, m_p1, m_p2))
                return m_l1;
            if (oneEquals(p1, p2, m_p2, m_p3))
                return m_l2;
            if (oneEquals(p1, p2, m_p3, m_p1))
                return m_l3;
            throw new Exception("Not valid points");
        }

        public Vector2 getAngle(Vector2 point)
        {
            if (point == m_p1)
                return m_a1;
            if (point == m_p2)
                return m_a2;
            if (point == m_p3)
                return m_a3;
            
            throw new Exception("Wrong point");
        }
        
        private static float calculateLen(Vector2 p1, Vector2 p2)
        {
            float cat1 = Math.Abs(p1.x - p2.x);
            float cat2 = Math.Abs(p1.y - p2.y);
            
            return (float)(Math.Sqrt(cat2 * cat2 + cat1 * cat1));
        }
        
        private static float calculateAngle(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            Vector2 v1 = new Vector2(p2.x - p1.x, p2.y - p1.y);
            Vector2 v2 = new Vector2(p3.x - p1.x, p3.y - p1.y);

            float dot = v1.x * v2.x + v1.y * v2.y;

            float len1 = (float)Math.Sqrt(v1.x * v1.x + v1.y * v1.y);
            float len2 = (float)Math.Sqrt(v2.x * v2.x + v2.y * v2.y);

            float cosTheta = dot / (len1 * len2);

            cosTheta = Math.Clamp(cosTheta, -1f, 1f);

            return (float)Math.Acos(cosTheta);
        }
        
    }

    public static Vector2 angleToVec2(float angle)
    {
        float angleRadians = angle * Mathf.Deg2Rad;

        return new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
    }
    
    public static float arctan(float x, uint precision = 15)
    {
        float retVal = x;
        float lastPow = x;
        uint lastNum = 1;

        for (uint i = 0; i < precision; i++)
        {
            retVal -= (lastPow *= x * x) / (lastNum += 2);
        }
        
        return retVal;
    }
    
    [Obsolete("Not implemented, will throw not implemented error")]
    public static Vector2 getAngleBetween(Vector2 origin, Vector2 goTo)
    {
        // new Triangle(origin, new Vector2(origin.x, goTo.y), goTo).getAngle(origin, goTo);
        throw new NotImplementedException();
    }
}