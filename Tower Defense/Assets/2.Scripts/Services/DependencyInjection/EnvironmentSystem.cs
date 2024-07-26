using UnityEngine;

namespace Services.DependencyInjection
{
    public interface IEnvironmentSystem
    {
        IEnvironmentSystem ProvideEnvironmentSystem();
    }
    
    public class EnvironmentSystem : MonoBehaviour, IEnvironmentSystem, IDependencyProvider
    {
        [Provide]
        public IEnvironmentSystem ProvideEnvironmentSystem()
        {
            return this;
        }
    }
}