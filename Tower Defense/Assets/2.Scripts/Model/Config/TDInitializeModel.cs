using System.Threading.Tasks;

public class TDInitializeModel
{
    public static TDInitializeModel api;
    
    public readonly TaskCompletionSource<bool> createGridCompletion = new TaskCompletionSource<bool>();
}
