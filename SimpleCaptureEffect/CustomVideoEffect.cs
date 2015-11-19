using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;

namespace SimpleCaptureEffect
{
    public delegate void ProcessHandlerFunction(ProcessVideoFrameContext x, CanvasDevice y);
    public sealed class CustomVideoEffect : IBasicVideoEffect
    {
        CanvasDevice canvasDevice;
        static ProcessHandlerFunction processHandler;

        public static VideoEffectDefinition CreateDefinition(ProcessHandlerFunction x)
        {
            processHandler = x;
            return new VideoEffectDefinition(typeof(CustomVideoEffect).FullName);
        }

        public bool IsReadOnly { get { return false; } }

        public IReadOnlyList<VideoEncodingProperties> SupportedEncodingProperties { get { return new List<VideoEncodingProperties>(); } }

        public MediaMemoryTypes SupportedMemoryTypes { get { return MediaMemoryTypes.Gpu; } }

        public bool TimeIndependent { get { return false; } }

        public void Close(MediaEffectClosedReason reason)
        {
            if (canvasDevice != null) { canvasDevice.Dispose(); }
        }

        public void DiscardQueuedFrames() { }

        public void ProcessFrame(ProcessVideoFrameContext context)
        {
            processHandler(context, canvasDevice);
        }

        public void SetEncodingProperties(VideoEncodingProperties encodingProperties, IDirect3DDevice device)
        {
            canvasDevice = CanvasDevice.CreateFromDirect3D11Device(device);
        }

        public void SetProperties(IPropertySet configuration) { }
    }
}
