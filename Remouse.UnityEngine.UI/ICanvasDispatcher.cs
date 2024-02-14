using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Remouse.UnityEngine.UI
{
    public interface ICanvasDispatcher
    {
        UniTask<Canvas> GetAsync(CanvasType canvasType);
    }
}