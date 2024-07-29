using System.Threading.Tasks;

public class InitializeModel
{
    public static InitializeModel api;
    
    public readonly TaskCompletionSource<bool> createGridCompletion = new TaskCompletionSource<bool>();
}
