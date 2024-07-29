using System.Threading.Tasks;
using Services.Utils;
using UnityEngine;

namespace Managers
{
    public class Initializer : Singleton<Initializer>
    {
        public readonly TaskCompletionSource<bool> createGridCompletion = new TaskCompletionSource<bool>();
    }
}
