using System.Threading.Tasks;
using JetBrains.Annotations;

namespace DELTation.UnityPix.Editor
{
    internal interface ICaptureTarget
    {
        [MustUseReturnValue]
        Task RenderAsync();

        int NumFrames { get; }
    }
}