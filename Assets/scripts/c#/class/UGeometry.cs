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
        private Vector2 m_p1,  m_p2, m_p3;

        public Triangle(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            m_p1 = p1;
            m_p2 = p2;
            m_p3 = p3;
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
        }
    }
    
    public static Vector2 getDirection(Vector2 origin, Vector2 goTo)
    {
        Triangle triangle = new Triangle(
            origin, 
            new Vector2(goTo.x, origin.y), 
            new Vector2(goTo.y, origin.x)
        );

        throw new NotImplementedException();
    }
}