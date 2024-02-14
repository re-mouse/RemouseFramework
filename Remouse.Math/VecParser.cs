using System;
using System.Globalization;

namespace Remouse.Math
{
    public class VecParser
    {
        public static Vec3 ParseVec3(string vec3String)
        {
            int startIndex = 1; // Начало после открывающей скобки '('
            int endIndex = vec3String.IndexOf(',', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            float x = float.Parse(vec3String.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            startIndex = endIndex + 1;
            endIndex = vec3String.IndexOf(',', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            float y = float.Parse(vec3String.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            startIndex = endIndex + 1;
            endIndex = vec3String.IndexOf(')', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            float z = float.Parse(vec3String.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            return new Vec3(x, y, z);
        }
        
        public static Vec2 ParseVec2(string vec2String)
        {
            int startIndex = 1; // Начало после открывающей скобки '('
            int endIndex = vec2String.IndexOf(',', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            float x = float.Parse(vec2String.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            startIndex = endIndex + 1;
            endIndex = vec2String.IndexOf(')', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            float y = float.Parse(vec2String.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            return new Vec2(x, y);
        }
        
        public static Vec3Int ParseVec3Int(string vec3IntString)
        {
            int startIndex = 1; // Начало после открывающей скобки '('
            int endIndex = vec3IntString.IndexOf(',', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            int x = int.Parse(vec3IntString.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            startIndex = endIndex + 1;
            endIndex = vec3IntString.IndexOf(',', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            int y = int.Parse(vec3IntString.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            startIndex = endIndex + 1;
            endIndex = vec3IntString.IndexOf(')', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            int z = int.Parse(vec3IntString.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            return new Vec3Int(x, y, z);
        }
        
        public static Vec2Int ParseVec2Int(string vec2IntString)
        {
            int startIndex = 1; // Начало после открывающей скобки '('
            int endIndex = vec2IntString.IndexOf(',', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            int x = int.Parse(vec2IntString.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            startIndex = endIndex + 1;
            endIndex = vec2IntString.IndexOf(')', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            int y = int.Parse(vec2IntString.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            return new Vec2Int(x, y);
        }
        
        public static Vec4 ParseVec4(string vec4String)
        {
            int startIndex = 1; // Начало после открывающей скобки '('
            int endIndex = vec4String.IndexOf(',', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            float x = float.Parse(vec4String.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            startIndex = endIndex + 1;
            endIndex = vec4String.IndexOf(',', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            float y = float.Parse(vec4String.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            startIndex = endIndex + 1;
            endIndex = vec4String.IndexOf(',', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            float z = float.Parse(vec4String.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            startIndex = endIndex + 4; // Пропускаем " w: "
            endIndex = vec4String.IndexOf(')', startIndex);
            if (endIndex == -1) throw new FormatException("String is not in the correct format.");

            float w = float.Parse(vec4String.Substring(startIndex, endIndex - startIndex).Trim(), CultureInfo.InvariantCulture);

            return new Vec4(x, y, z, w);
        }
    }
}