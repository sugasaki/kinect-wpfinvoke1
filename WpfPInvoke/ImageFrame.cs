using System;
using System.Net;
using System.Runtime.InteropServices;


namespace WpfPInvoke
{



    [StructLayout(LayoutKind.Sequential)]
    public class NUI_IMAGE_FRAME
    {
        public UInt64 liTimeStamp; //LARGE_INTEGER
        public uint dwFrameNumber;
        public NUI_IMAGE_TYPE eImageType;
        public NUI_IMAGE_RESOLUTION eResolution;
        public IntPtr pFrameTexture; //INuiFrameTextureのポインタ
        public uint dwFrameFlags;
        public IntPtr ViewArea;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct NUI_IMAGE_VIEW_AREA
    {
        public int Zoom; //eDigitalZoom_NotUsed
        public int CenterX; //lCenterX_NotUsed long(C++) -> int(C#)
        public int CenterY; //lCenterY_NotUsed long(C++) -> int(C#)
    }


    [StructLayout(LayoutKind.Explicit)]
    public struct LARGE_INTEGER
    {
        [FieldOffset(0)]
        public uint lowpart;
        [FieldOffset(4)]
        public int highpart;
        [FieldOffset(0)]
        public long QuadPart;
    }






}
