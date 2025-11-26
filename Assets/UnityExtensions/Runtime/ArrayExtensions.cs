using System;

namespace work.ctrl3d
{
    public static class ArrayExtensions {

        public static void BlockCopy<T>(this Span<T> src, int srcOffset, Span<T> dst, int dstOffset, int count)
        {if((uint)srcOffset + count > (uint)src.Length) 
                throw new ArgumentException("Source span is too small");
            if((uint)dstOffset + count > (uint)dst.Length) 
                throw new ArgumentException("Destination span is too small");
            
            src.Slice(srcOffset, count).CopyTo(dst.Slice(dstOffset, count));
        }
        
        public static void BlockCopy<T>(this T[] src, int srcOffset, T[] dst, int dstOffset, int count)
        {
            if(src == null) throw new ArgumentNullException(nameof(src));
            if(dst == null) throw new ArgumentNullException(nameof(dst));
         
            src.AsSpan().BlockCopy(srcOffset, dst.AsSpan(), dstOffset, count);
        }
    }
}
