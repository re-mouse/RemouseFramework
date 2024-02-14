using Remouse.Math;
using UnityEngine;

namespace Remouse.UnityEngine.Utils
{
    public static class UnityVectorExtensions
    {
        public static Vector3 ToVector3(this Vec3 vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }
        
        public static Vector4 ToVector4(this Vec4 vector)
        {
            return new Vector4(vector.x, vector.y, vector.z, vector.w);
        }
        
        public static Quaternion ToQuaternion(this Vec4 vector)
        {
            return new Quaternion(vector.x, vector.y, vector.z, vector.w);
        }
        
        public static Vector2 ToVector2(this Vec2 vector)
        {
            return new Vector3(vector.x, vector.y);
        }
        
        public static Vector2 ToVector2XY(this Vec3 vector)
        {
            return new Vector3(vector.x, vector.y);
        }
        
        public static Vector2 ToVector2XZ(this Vec3 vector)
        {
            return new Vector3(vector.x, vector.z);
        }
        public static Vector2 ToVector2YZ(this Vec3 vector)
        {
            return new Vector3(vector.y, vector.z);
        }

        public static Vector3Int ToVector3Int(this Vec3Int vector)
        {
            return new Vector3Int(vector.x, vector.y, vector.z);
        }
        
        public static Vector2Int ToVector2Int(this Vec2Int vector)
        {
            return new Vector2Int(vector.x, vector.y);
        }
        
        public static Vector2Int ToVector2IntXY(this Vec3Int vector)
        {
            return new Vector2Int(vector.x, vector.y);
        }
        
        public static Vector2Int ToVector2IntXZ(this Vec3Int vector)
        {
            return new Vector2Int(vector.x, vector.z);
        }
        
        public static Vector2Int ToVector2IntYZ(this Vec3Int vector)
        {
            return new Vector2Int(vector.y, vector.z);
        }
        
        public static Vec2 ToVec2(this Vector2 vector)
        {
            return new Vec2(vector.x, vector.y);
        }
        
        public static Vec3 ToVec3(this Vector3 vector)
        {
            return new Vec3(vector.x, vector.y, vector.z);
        }
        
        public static Vec4 ToVec4(this Vector4 vector)
        {
            return new Vec4(vector.x, vector.y, vector.z, vector.w);
        }
        
        public static Vec4 ToVec4(this Quaternion vector)
        {
            return new Vec4(vector.x, vector.y, vector.z, vector.w);
        }
        
        public static Vec2 ToVec2XY(this Vector3 vector)
        {
            return new Vec2(vector.x, vector.y);
        }
        
        public static Vec2 ToVec2XZ(this Vector3 vector)
        {
            return new Vec2(vector.x, vector.z);
        }
        
        public static Vec2 ToVec2YZ(this Vector3 vector)
        {
            return new Vec2(vector.y, vector.z);
        }
        
        public static Vec2Int ToVec2Int(this Vector2Int vector)
        {
            return new Vec2Int(vector.x, vector.y);
        }
        
        public static Vec2Int ToVec2IntXY(this Vector3 vector)
        {
            return new Vec2Int((int)vector.x, (int)vector.y);
        }
        
        public static Vec2Int ToVec2IntXZ(this Vector3 vector)
        {
            return new Vec2Int((int)vector.x, (int)vector.z);
        }
        
        public static Vec2Int ToVec2IntYZ(this Vector3 vector)
        {
            return new Vec2Int((int)vector.y, (int)vector.z);
        }

        public static Vector2 ToVector2XY(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }
        
        public static Vector2 ToVector2XZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }
        
        public static Vector2 ToVector2YZ(this Vector3 vector)
        {
            return new Vector2(vector.y, vector.z);
        }

        public static Vec3Int ToVec3Int(this Vector3Int vector)
        {
            return new Vec3Int(vector.x, vector.y, vector.z);
        }
    }
}