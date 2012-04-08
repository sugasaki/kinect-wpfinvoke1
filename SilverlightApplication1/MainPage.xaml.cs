using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

using System.Runtime.InteropServices;


namespace SilverlightApplication1
{



    public partial class MainPage : UserControl
    {

        #region DllImport

        [DllImport("Kinect10.dll")]
        private static extern HRESULT NuiInitialize(uint dwFlags);


        [DllImport("Kinect10.dll")]
        private static extern void NuiShutdown();


        [DllImport("Kinect10.DLL")]
        private static extern HRESULT NuiImageStreamOpen(
            /* [in] */ NUI_IMAGE_TYPE eImageType,
            /* [in] */ NUI_IMAGE_RESOLUTION eResolution,
            /* [in] */ uint dwImageFrameFlags,
            /* [in] */ uint dwFrameLimit,
            /* [in] */ IntPtr hNextFrameEvent,
            /* [out] */ ref IntPtr phStreamHandle);


        [DllImport("Kinect10.DLL")]
        private static extern HRESULT NuiImageStreamGetNextFrame(IntPtr hStream,
           uint dwMillisecondsToWait, ref IntPtr ppcImageFrame);
           //uint dwMillisecondsToWait, ref NUI_IMAGE_FRAME ppcImageFrame);

        [DllImport("Kinect10.DLL")]
        private static extern HRESULT NuiImageStreamReleaseFrame(IntPtr hStream,
           IntPtr pImageFrame);

        #endregion

        private IntPtr streamHandle = IntPtr.Zero;
        private IntPtr imageFramePtr = IntPtr.Zero;

        public MainPage()
        {
            InitializeComponent();
        }

        #region ウインドウの表示と終了イベント

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            HRESULT res = NuiInitialize((uint)NUI_INITIALIZE_FLAG.USES_COLOR);
            if (res != HRESULT.S_OK)
                throw new InvalidOperationException("Failed to initialize the kinect runtime, return value:" + res.ToString());


            OpenStream(NUI_IMAGE_TYPE.NUI_IMAGE_TYPE_COLOR, NUI_IMAGE_RESOLUTION.NUI_IMAGE_RESOLUTION_640x480);


            //レンダリング時にKinectデータを取得し描画
            CompositionTarget.Rendering += compositionTarget_rendering;

        }


        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            NuiShutdown();
        }


        private void OpenStream(NUI_IMAGE_TYPE imageType, NUI_IMAGE_RESOLUTION resolution)
        {
            HRESULT res = NuiImageStreamOpen(imageType, resolution, 0, 2, IntPtr.Zero, ref streamHandle);

            if (res != HRESULT.S_OK)
                throw new Exception("Failed to open stream, return value:" + res.ToString());

        }

        #endregion


        /// <summary>
        /// レンダリング時にKinectデータを取得し描画
        /// </summary>
        private void compositionTarget_rendering(object sender, EventArgs e)
        {

            //フレームを読み取る
            HRESULT hr = NuiImageStreamGetNextFrame(streamHandle, 0, ref imageFramePtr);
            if (hr != HRESULT.S_OK)
            {
                return;
            }

            try
            {
                //imageFramePtrからNUI_IMAGE_FRAMEへマーシャリング
                var nativeImageFrame = new NUI_IMAGE_FRAME();
                Marshal.PtrToStructure(imageFramePtr, nativeImageFrame);

                //Console.WriteLine("SizeOf: {0}", Marshal.SizeOf(typeof(NUI_IMAGE_FRAME)));

                //pFrameTextureからINuiFrameTextureへマーシャリング
                var pTexture = (INuiFrameTexture)Marshal.GetObjectForIUnknown(nativeImageFrame.pFrameTexture);


                var BufferLen = pTexture.BufferLen();
                var Pitch = pTexture.Pitch();


                /*
                 * 
                //ロックしてバッファへアクセス
                var LockedRect = new NUI_LOCKED_RECT();
                //var Rect = new RECT();

                IntPtr pLockedRect = IntPtr.Zero;
                IntPtr pRect = IntPtr.Zero;
                pTexture.LockRect(0, ref pLockedRect, ref pRect, 0);

                
                Marshal.PtrToStructure(pLockedRect, LockedRect);

                */
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Message: " + ex.Message);
            }
            finally
            {
                var res = NuiImageStreamReleaseFrame(streamHandle, imageFramePtr);
                if (res != HRESULT.S_FALSE && res != HRESULT.S_OK)
                {
                    throw new InvalidOperationException("Failed to release stream, HRESULT: " + res.ToString());
                }
            }


        }



    }
}
