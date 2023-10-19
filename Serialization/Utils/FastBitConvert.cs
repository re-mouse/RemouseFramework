using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Remouse.Serialization
{
    public static class FastBitConvert
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct ConverterHelperDouble
        {
            [FieldOffset(0)]
            public ulong Along;

            [FieldOffset(0)]
            public double Adouble;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct ConverterHelperFloat
        {
            [FieldOffset(0)]
            public int Aint;

            [FieldOffset(0)]
            public float Afloat;
        }

        #region Getting Bytes from value
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteLittleEndian(byte[] buffer, int offset, ulong data)
        {
#if BIGENDIAN
            buffer[offset + 7] = (byte)(data);
            buffer[offset + 6] = (byte)(data >> 8);
            buffer[offset + 5] = (byte)(data >> 16);
            buffer[offset + 4] = (byte)(data >> 24);
            buffer[offset + 3] = (byte)(data >> 32);
            buffer[offset + 2] = (byte)(data >> 40);
            buffer[offset + 1] = (byte)(data >> 48);
            buffer[offset    ] = (byte)(data >> 56);
#else
            buffer[offset] = (byte)(data);
            buffer[offset + 1] = (byte)(data >> 8);
            buffer[offset + 2] = (byte)(data >> 16);
            buffer[offset + 3] = (byte)(data >> 24);
            buffer[offset + 4] = (byte)(data >> 32);
            buffer[offset + 5] = (byte)(data >> 40);
            buffer[offset + 6] = (byte)(data >> 48);
            buffer[offset + 7] = (byte)(data >> 56);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteLittleEndian(byte[] buffer, int offset, int data)
        {
#if BIGENDIAN
            buffer[offset + 3] = (byte)(data);
            buffer[offset + 2] = (byte)(data >> 8);
            buffer[offset + 1] = (byte)(data >> 16);
            buffer[offset    ] = (byte)(data >> 24);
#else
            buffer[offset] = (byte)(data);
            buffer[offset + 1] = (byte)(data >> 8);
            buffer[offset + 2] = (byte)(data >> 16);
            buffer[offset + 3] = (byte)(data >> 24);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLittleEndian(byte[] buffer, int offset, short data)
        {
#if BIGENDIAN
            buffer[offset + 1] = (byte)(data);
            buffer[offset    ] = (byte)(data >> 8);
#else
            buffer[offset] = (byte)(data);
            buffer[offset + 1] = (byte)(data >> 8);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetBytes(byte[] bytes, int startIndex, double value)
        {
            ConverterHelperDouble ch = new ConverterHelperDouble { Adouble = value };
            WriteLittleEndian(bytes, startIndex, ch.Along);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetBytes(byte[] bytes, int startIndex, float value)
        {
            ConverterHelperFloat ch = new ConverterHelperFloat { Afloat = value };
            WriteLittleEndian(bytes, startIndex, ch.Aint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetBytes(byte[] bytes, int startIndex, short value)
        {
            WriteLittleEndian(bytes, startIndex, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetBytes(byte[] bytes, int startIndex, ushort value)
        {
            WriteLittleEndian(bytes, startIndex, (short)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetBytes(byte[] bytes, int startIndex, int value)
        {
            WriteLittleEndian(bytes, startIndex, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetBytes(byte[] bytes, int startIndex, uint value)
        {
            WriteLittleEndian(bytes, startIndex, (int)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetBytes(byte[] bytes, int startIndex, long value)
        {
            WriteLittleEndian(bytes, startIndex, (ulong)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetBytes(byte[] bytes, int startIndex, ulong value)
        {
            WriteLittleEndian(bytes, startIndex, value);
        }
        
        #endregion
        
        #region Getting Value from Bytes
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadLittleEndian4Bytes(byte[] bytes, int startIndex)
        {
#if BIGENDIAN
    return (bytes[startIndex + 3])
        | (bytes[startIndex + 2] << 8)
        | (bytes[startIndex + 1] << 16)
        | (bytes[startIndex] << 24);
#else
            return (bytes[startIndex])
                   | (bytes[startIndex + 1] << 8)
                   | (bytes[startIndex + 2] << 16)
                   | (bytes[startIndex + 3] << 24);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadLittleEndian8Bytes(byte[] bytes, int startIndex)
        {
            ulong value;
#if BIGENDIAN
    value = (long)(bytes[startIndex + 7])
        | ((long)bytes[startIndex + 6] << 8)
        | ((long)bytes[startIndex + 5] << 16)
        | ((long)bytes[startIndex + 4] << 24)
        | ((long)bytes[startIndex + 3] << 32)
        | ((long)bytes[startIndex + 2] << 40)
        | ((long)bytes[startIndex + 1] << 48)
        | ((long)bytes[startIndex] << 56);
#else
            value = bytes[startIndex]
                    | ((ulong)bytes[startIndex + 1] << 8)
                    | ((ulong)bytes[startIndex + 2] << 16)
                    | ((ulong)bytes[startIndex + 3] << 24)
                    | ((ulong)bytes[startIndex + 4] << 32)
                    | ((ulong)bytes[startIndex + 5] << 40)
                    | ((ulong)bytes[startIndex + 6] << 48)
                    | ((ulong)bytes[startIndex + 7] << 56);
#endif
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadLittleEndian2Bytes(byte[] bytes, int startIndex)
        {
#if BIGENDIAN
    return (short)((bytes[startIndex + 1])
        | (bytes[startIndex] << 8));
#else
            return (short)(bytes[startIndex]
                           | (bytes[startIndex + 1] << 8));
#endif
        }

        public static int ToInt32(byte[] bytes, int startIndex)
        {
            return ReadLittleEndian4Bytes(bytes, startIndex);
        }
        
        public static ushort ToUInt16(byte[] bytes, int startIndex)
        {
            return (ushort)ReadLittleEndian2Bytes(bytes, startIndex);
        }

        public static short ToInt16(byte[] bytes, int startIndex)
        {
            return ReadLittleEndian2Bytes(bytes, startIndex);
        }

        public static uint ToUInt32(byte[] bytes, int startIndex)
        {
            return (uint)ReadLittleEndian4Bytes(bytes, startIndex);
        }

        public static float ToSingle(byte[] bytes, int startIndex)
        {
            int intVal = ReadLittleEndian4Bytes(bytes, startIndex);
            ConverterHelperFloat ch = new ConverterHelperFloat { Aint = intVal };
            return ch.Afloat;
        }

        public static long ToInt64(byte[] bytes, int startIndex)
        {
            return (long)ReadLittleEndian8Bytes(bytes, startIndex);
        }

        public static ulong ToUInt64(byte[] bytes, int startIndex)
        {
            return ReadLittleEndian8Bytes(bytes, startIndex);
        }

        public static double ToDouble(byte[] bytes, int startIndex)
        {
            ulong longVal = ReadLittleEndian8Bytes(bytes, startIndex);
            ConverterHelperDouble ch = new ConverterHelperDouble { Along = longVal };
            return ch.Adouble;
        }
        
        #endregion
    }
}