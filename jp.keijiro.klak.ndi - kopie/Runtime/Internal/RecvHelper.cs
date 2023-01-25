using IntPtr = System.IntPtr;

namespace Klak.Ndi {

// Small helper class for NDI recv interop
static class RecvHelper
{
    public static Interop.Source? FindSource(string sourceName)
    {
        foreach (var source in SharedInstance.Find.CurrentSources)
            if (source.NdiName == sourceName) return source;
        return null;
    }

    public static unsafe Interop.Recv TryCreateRecv(string sourceName)
    {
        var source = FindSource(sourceName);
        if (source == null) return null;

        var opt = new Interop.Recv.Settings
          { Source = (Interop.Source)source,
            ColorFormat = Interop.ColorFormat.Fastest,
            Bandwidth = Interop.Bandwidth.Highest };

        return Interop.Recv.Create(opt);
    }

    public static Interop.VideoFrame? TryCaptureVideoFrame(Interop.Recv recv,Interop.KVMdata kvm)
    {
            
        Interop.VideoFrame video;
        var type = recv.Capture(out video, IntPtr.Zero, IntPtr.Zero, 0);
            if (recv.GetKVM())
            {
                recv.Send_Mouse_Position(kvm.MouseX, kvm.MouseY);
                if (kvm.TouchD)
                {
                    recv.Send_Leftclick_Down();
                }
                else
                {
                    recv.Send_Leftclick_Up();
                }
            }
        if (type != Interop.FrameType.Video) return null;
        return (Interop.VideoFrame?)video;
    }
}

} // namespace Klak.Ndi
