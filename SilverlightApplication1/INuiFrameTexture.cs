using System;
using System.Net;
using System.Runtime.InteropServices;



//C++

//INuiFrameTexture : public IUnknown
//{
//public:
//    virtual int STDMETHODCALLTYPE BufferLen( void) = 0;

//    virtual int STDMETHODCALLTYPE Pitch( void) = 0;

//    virtual HRESULT STDMETHODCALLTYPE LockRect( 
//        UINT Level,
//        /* [ref] */ NUI_LOCKED_RECT *pLockedRect,
//        /* [unique] */ RECT *pRect,
//        DWORD Flags) = 0;

//    virtual HRESULT STDMETHODCALLTYPE GetLevelDesc( 
//        UINT Level,
//        NUI_SURFACE_DESC *pDesc) = 0;

//    virtual HRESULT STDMETHODCALLTYPE UnlockRect( 
//        /* [in] */ UINT Level) = 0;

//};



namespace SilverlightApplication1
{
    
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("13ea17f5-ff2e-4670-9ee5-1297a6e880d1")]
    public interface INuiFrameTexture
    {
        [PreserveSig]
        int BufferLen();

        [PreserveSig]
        int Pitch();

        [PreserveSig]
        HRESULT LockRect(
            uint Level,

            ref IntPtr pLockedRect, //NUI_LOCKED_RECT

            ref IntPtr pRect,//

            uint Flags
        );

        [PreserveSig]
        HRESULT GetLevelDesc(
           uint Level,
           NUI_SURFACE_DESC pDesc
           );

        [PreserveSig]
        HRESULT UnlockRect(
           [In]
            uint Level
           );
    }



    [StructLayout(LayoutKind.Sequential)]
    public class NUI_LOCKED_RECT
    {
        public int Pitch;//
        public int size;//
        public IntPtr pBits;//
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct NUI_SURFACE_DESC
    {
        public int Width;
        public int Height;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }


}

