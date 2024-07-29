using System.Threading.Tasks;
using Services.Utils;
using UnityEngine;

public class InitializeModel
{
    public static InitializeModel api;
    
    public readonly TaskCompletionSource<bool> createGridCompletion = new TaskCompletionSource<bool>();
}
