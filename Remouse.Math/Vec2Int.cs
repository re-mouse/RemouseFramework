using System;
using System.Runtime.CompilerServices;

namespace Remouse.Math
{
    /// <summary>
  ///   <para>Representation of 2D vectors and points using integers.</para>
  /// </summary>
  [Serializable]
  public struct Vec2Int : IEquatable<Vec2Int>
  {
    private int _x;
    private int _y;
    private static readonly Vec2Int s_Zero = new Vec2Int(0, 0);
    private static readonly Vec2Int s_One = new Vec2Int(1, 1);
    private static readonly Vec2Int s_Up = new Vec2Int(0, 1);
    private static readonly Vec2Int s_Down = new Vec2Int(0, -1);
    private static readonly Vec2Int s_Left = new Vec2Int(-1, 0);
    private static readonly Vec2Int s_Right = new Vec2Int(1, 0);

    /// <summary>
    ///   <para>X component of the vector.</para>
    /// </summary>
    public int x
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => _x;
      [MethodImpl(MethodImplOptions.AggressiveInlining)] set => _x = value;
    }

    /// <summary>
    ///   <para>Y component of the vector.</para>
    /// </summary>
    public int y
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => _y;
      [MethodImpl(MethodImplOptions.AggressiveInlining)] set => _y = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec2Int(int x, int y)
    {
      _x = x;
      _y = y;
    }

    /// <summary>
    ///   <para>Set x and y components of an existing Vec2Int.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(int x, int y)
    {
      _x = x;
      _y = y;
    }

    public int this[int index]
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get
      {
        switch (index)
        {
          case 0:
            return x;
          case 1:
            return y;
          default:
            throw new IndexOutOfRangeException(string.Format("Invalid Vec2Int index addressed: {0}!", index));
        }
      }
      [MethodImpl(MethodImplOptions.AggressiveInlining)] set
      {
        switch (index)
        {
          case 0:
            x = value;
            break;
          case 1:
            y = value;
            break;
          default:
            throw new IndexOutOfRangeException(string.Format("Invalid Vec2Int index addressed: {0}!", index));
        }
      }
    }

    /// <summary>
    ///   <para>Returns the length of this vector (Read Only).</para>
    /// </summary>
    public float magnitude
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (float)System.Math.Sqrt((float) (x * x + y * y));
    }

    /// <summary>
    ///   <para>Returns the squared length of this vector (Read Only).</para>
    /// </summary>
    public int sqrMagnitude
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => x * x + y * y;
    }

    /// <summary>
    ///   <para>Returns the distance between a and b.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vec2Int a, Vec2Int b)
    {
      float num1 = a.x - b.x;
      float num2 = a.y - b.y;
      return (float) System.Math.Sqrt(num1 * (double) num1 + num2 * (double) num2);
    }

    /// <summary>
    ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int Min(Vec2Int lhs, Vec2Int rhs) => new Vec2Int(System.Math.Min(lhs.x, rhs.x), System.Math.Min(lhs.y, rhs.y));

    /// <summary>
    ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int Max(Vec2Int lhs, Vec2Int rhs) => new Vec2Int(System.Math.Max(lhs.x, rhs.x), System.Math.Max(lhs.y, rhs.y));

    /// <summary>
    ///   <para>Multiplies two vectors component-wise.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int Scale(Vec2Int a, Vec2Int b) => new Vec2Int(a.x * b.x, a.y * b.y);

    /// <summary>
    ///   <para>Multiplies every component of this vector by the same component of scale.</para>
    /// </summary>
    /// <param name="scale"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Scale(Vec2Int scale)
    {
      x *= scale.x;
      y *= scale.y;
    }

    /// <summary>
    ///   <para>Clamps the Vec2Int to the bounds given by min and max.</para>
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clamp(Vec2Int min, Vec2Int max)
    {
      x = System.Math.Max(min.x, x);
      x = System.Math.Min(max.x, x);
      y = System.Math.Max(min.y, y);
      y = System.Math.Min(max.y, y);
    }
    
    /// <summary>
    ///   <para>Converts a  Vector2 to a Vector2Int by doing a Ceiling to each value.</para>
    /// </summary>
    /// <param name="v"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int CeilToInt(Vec2 v) => new Vec2Int((int)(v.x + 0.5f), (int)(v.y + 0.5f));

    /// <summary>
    ///   <para>Converts a  Vector2 to a Vector2Int by doing a Round to each value.</para>
    /// </summary>
    /// <param name="v"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int RoundToInt(Vec2 v) => new Vec2Int((int)(v.x + 0.5f), (int)(v.y + 0.5f));

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int operator -(Vec2Int v) => new Vec2Int(-v.x, -v.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int operator +(Vec2Int a, Vec2Int b) => new Vec2Int(a.x + b.x, a.y + b.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int operator -(Vec2Int a, Vec2Int b) => new Vec2Int(a.x - b.x, a.y - b.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int operator *(Vec2Int a, Vec2Int b) => new Vec2Int(a.x * b.x, a.y * b.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int operator *(int a, Vec2Int b) => new Vec2Int(a * b.x, a * b.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int operator *(Vec2Int a, int b) => new Vec2Int(a.x * b, a.y * b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec2Int operator /(Vec2Int a, int b) => new Vec2Int(a.x / b, a.y / b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vec2Int lhs, Vec2Int rhs) => lhs.x == rhs.x && lhs.y == rhs.y;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vec2Int lhs, Vec2Int rhs) => !(lhs == rhs);

    /// <summary>
    ///   <para>Returns true if the objects are equal.</para>
    /// </summary>
    /// <param name="other"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object other) => other is Vec2Int other1 && Equals(other1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vec2Int other) => x == other.x && y == other.y;

    /// <summary>
    ///   <para>Gets the hash code for the Vec2Int.</para>
    /// </summary>
    /// <returns>
    ///   <para>The hash code of the Vec2Int.</para>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
      int num1 = x;
      int hashCode = num1.GetHashCode();
      num1 = y;
      int num2 = num1.GetHashCode() << 2;
      return hashCode ^ num2;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vec2Int(0, 0).</para>
    /// </summary>
    public static Vec2Int zero
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => s_Zero;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vec2Int(1, 1).</para>
    /// </summary>
    public static Vec2Int one
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => s_One;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vec2Int(0, 1).</para>
    /// </summary>
    public static Vec2Int up
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => s_Up;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vec2Int(0, -1).</para>
    /// </summary>
    public static Vec2Int down
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => s_Down;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vec2Int(-1, 0).</para>
    /// </summary>
    public static Vec2Int left
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => s_Left;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vec2Int(1, 0).</para>
    /// </summary>
    public static Vec2Int right
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => s_Right;
    }
  }
}