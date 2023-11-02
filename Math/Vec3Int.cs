// Decompiled with JetBrains decompiler
// Type: UnityEngine.Vector3Int
// Assembly: UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1390FDF6-EA82-4C60-86AD-FB60B8035622
// Assembly location: /Applications/Unity/Hub/Editor/2022.3.0f1/Unity.app/Contents/Managed/UnityEngine/UnityEngine.CoreModule.dll
// XML documentation location: /Applications/Unity/Hub/Editor/2022.3.0f1/Unity.app/Contents/Managed/UnityEngine/UnityEngine.CoreModule.xml

using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Remouse.MathLib
{
  /// <summary>
  ///   <para>Representation of 3D vectors and points using integers.</para>
  /// </summary>
  public struct Vec3Int : IEquatable<Vec3Int>, IFormattable
  {
    private int m_X;
    private int m_Y;
    private int m_Z;
    private static readonly Vec3Int s_Zero = new Vec3Int(0, 0, 0);
    private static readonly Vec3Int s_One = new Vec3Int(1, 1, 1);
    private static readonly Vec3Int s_Up = new Vec3Int(0, 1, 0);
    private static readonly Vec3Int s_Down = new Vec3Int(0, -1, 0);
    private static readonly Vec3Int s_Left = new Vec3Int(-1, 0, 0);
    private static readonly Vec3Int s_Right = new Vec3Int(1, 0, 0);
    private static readonly Vec3Int s_Forward = new Vec3Int(0, 0, 1);
    private static readonly Vec3Int s_Back = new Vec3Int(0, 0, -1);

    /// <summary>
    ///   <para>X component of the vector.</para>
    /// </summary>
    public int x
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.m_X;
      [MethodImpl(MethodImplOptions.AggressiveInlining)] set => this.m_X = value;
    }

    /// <summary>
    ///   <para>Y component of the vector.</para>
    /// </summary>
    public int y
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.m_Y;
      [MethodImpl(MethodImplOptions.AggressiveInlining)] set => this.m_Y = value;
    }

    /// <summary>
    ///   <para>Z component of the vector.</para>
    /// </summary>
    public int z
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.m_Z;
      [MethodImpl(MethodImplOptions.AggressiveInlining)] set => this.m_Z = value;
    }

    /// <summary>
    ///   <para>Initializes and returns an instance of a new Vector3Int with x and y components and sets z to zero.</para>
    /// </summary>
    /// <param name="x">The X component of the Vector3Int.</param>
    /// <param name="y">The Y component of the Vector3Int.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3Int(int x, int y)
    {
      this.m_X = x;
      this.m_Y = y;
      this.m_Z = 0;
    }

    /// <summary>
    ///   <para>Initializes and returns an instance of a new Vector3Int with x, y, z components.</para>
    /// </summary>
    /// <param name="x">The X component of the Vector3Int.</param>
    /// <param name="y">The Y component of the Vector3Int.</param>
    /// <param name="z">The Z component of the Vector3Int.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vec3Int(int x, int y, int z)
    {
      this.m_X = x;
      this.m_Y = y;
      this.m_Z = z;
    }

    /// <summary>
    ///   <para>Set x, y and z components of an existing Vector3Int.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(int x, int y, int z)
    {
      this.m_X = x;
      this.m_Y = y;
      this.m_Z = z;
    }

    public int this[int index]
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get
      {
        switch (index)
        {
          case 0:
            return this.x;
          case 1:
            return this.y;
          case 2:
            return this.z;
          default:
            throw new IndexOutOfRangeException(string.Format("Invalid Vector3Int index addressed: {0}!", (object) index));
        }
      }
      [MethodImpl(MethodImplOptions.AggressiveInlining)] set
      {
        switch (index)
        {
          case 0:
            this.x = value;
            break;
          case 1:
            this.y = value;
            break;
          case 2:
            this.z = value;
            break;
          default:
            throw new IndexOutOfRangeException(string.Format("Invalid Vector3Int index addressed: {0}!", (object) index));
        }
      }
    }

    /// <summary>
    ///   <para>Returns the length of this vector (Read Only).</para>
    /// </summary>
    public float magnitude
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (float)Math.Sqrt((float) (this.x * this.x + this.y * this.y + this.z * this.z));
    }

    /// <summary>
    ///   <para>Returns the squared length of this vector (Read Only).</para>
    /// </summary>
    public int sqrMagnitude
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.x * this.x + this.y * this.y + this.z * this.z;
    }

    /// <summary>
    ///   <para>Returns the distance between a and b.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vec3Int a, Vec3Int b) => (a - b).magnitude;

    /// <summary>
    ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3Int Min(Vec3Int lhs, Vec3Int rhs) => new Vec3Int(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));

    /// <summary>
    ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3Int Max(Vec3Int lhs, Vec3Int rhs) => new Vec3Int(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));

    /// <summary>
    ///   <para>Multiplies two vectors component-wise.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3Int Scale(Vec3Int a, Vec3Int b) => new Vec3Int(a.x * b.x, a.y * b.y, a.z * b.z);

    /// <summary>
    ///   <para>Multiplies every component of this vector by the same component of scale.</para>
    /// </summary>
    /// <param name="scale"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Scale(Vec3Int scale)
    {
      this.x *= scale.x;
      this.y *= scale.y;
      this.z *= scale.z;
    }

    /// <summary>
    ///   <para>Clamps the Vector3Int to the bounds given by min and max.</para>
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clamp(Vec3Int min, Vec3Int max)
    {
      this.x = Math.Max(min.x, this.x);
      this.x = Math.Min(max.x, this.x);
      this.y = Math.Max(min.y, this.y);
      this.y = Math.Min(max.y, this.y);
      this.z = Math.Max(min.z, this.z);
      this.z = Math.Min(max.z, this.z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vec3(Vec3Int v) => new Vec3((float) v.x, (float) v.y, (float) v.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vec2Int(Vec3Int v) => new Vec2Int(v.x, v.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3Int operator +(Vec3Int a, Vec3Int b) => new Vec3Int(a.x + b.x, a.y + b.y, a.z + b.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3Int operator -(Vec3Int a, Vec3Int b) => new Vec3Int(a.x - b.x, a.y - b.y, a.z - b.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3Int operator *(Vec3Int a, Vec3Int b) => new Vec3Int(a.x * b.x, a.y * b.y, a.z * b.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3Int operator -(Vec3Int a) => new Vec3Int(-a.x, -a.y, -a.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3Int operator *(Vec3Int a, int b) => new Vec3Int(a.x * b, a.y * b, a.z * b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3Int operator *(int a, Vec3Int b) => new Vec3Int(a * b.x, a * b.y, a * b.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vec3Int operator /(Vec3Int a, int b) => new Vec3Int(a.x / b, a.y / b, a.z / b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vec3Int lhs, Vec3Int rhs) => lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vec3Int lhs, Vec3Int rhs) => !(lhs == rhs);

    /// <summary>
    ///   <para>Returns true if the objects are equal.</para>
    /// </summary>
    /// <param name="other"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object other) => other is Vec3Int other1 && this.Equals(other1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vec3Int other) => this == other;

    /// <summary>
    ///   <para>Gets the hash code for the Vector3Int.</para>
    /// </summary>
    /// <returns>
    ///   <para>The hash code of the Vector3Int.</para>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
      int hashCode1 = this.y.GetHashCode();
      int hashCode2 = this.z.GetHashCode();
      return this.x.GetHashCode() ^ hashCode1 << 4 ^ hashCode1 >> 28 ^ hashCode2 >> 4 ^ hashCode2 << 28;
    }

    /// <summary>
    ///   <para>Returns a formatted string for this vector.</para>
    /// </summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => this.ToString((string) null, (IFormatProvider) null);

    /// <summary>
    ///   <para>Returns a formatted string for this vector.</para>
    /// </summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format) => this.ToString(format, (IFormatProvider) null);

    /// <summary>
    ///   <para>Returns a formatted string for this vector.</para>
    /// </summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (formatProvider == null)
        formatProvider = (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat;
      return string.Format("({0}, {1}, {2})", (object) this.x.ToString(format, formatProvider), (object) this.y.ToString(format, formatProvider), (object) this.z.ToString(format, formatProvider));
    }

    /// <summary>
    ///   <para>Shorthand for writing Vector3Int(0, 0, 0).</para>
    /// </summary>
    public static Vec3Int zero
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Vec3Int.s_Zero;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vector3Int(1, 1, 1).</para>
    /// </summary>
    public static Vec3Int one
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Vec3Int.s_One;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vector3Int(0, 1, 0).</para>
    /// </summary>
    public static Vec3Int up
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Vec3Int.s_Up;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vector3Int(0, -1, 0).</para>
    /// </summary>
    public static Vec3Int down
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Vec3Int.s_Down;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vector3Int(-1, 0, 0).</para>
    /// </summary>
    public static Vec3Int left
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Vec3Int.s_Left;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vector3Int(1, 0, 0).</para>
    /// </summary>
    public static Vec3Int right
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Vec3Int.s_Right;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vector3Int(0, 0, 1).</para>
    /// </summary>
    public static Vec3Int forward
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Vec3Int.s_Forward;
    }

    /// <summary>
    ///   <para>Shorthand for writing Vector3Int(0, 0, -1).</para>
    /// </summary>
    public static Vec3Int back
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Vec3Int.s_Back;
    }
  }
}
