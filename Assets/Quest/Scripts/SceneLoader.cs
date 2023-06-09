using VContainer.Unity;

namespace SampleCode.Quest
{
    public class SceneLoader
    {
        readonly LifetimeScope _currentScope;

        LifetimeScope _instantScope;

        public SceneLoader(LifetimeScope lifetimeScope)
        {
            _currentScope = lifetimeScope;
        }
        
        public void Load()
        {
            // ... Loading some assets with any async way you like
            //
            // await Addressables.LoadAssetAsync...
            //


            MessagePipeInstaller extraInstaller = new MessagePipeInstaller();
            // Create a child scope with extra registrations via `IInstaller`
            _instantScope = _currentScope.CreateChild(extraInstaller);
        }

        public void Unload()
        {
            // Note that the scope implicitly create `LifetimeScope`.
            // Use `Dispose` to safely destroying the scope.
            _instantScope.Dispose();

            // ... Unloading some assets
        }
        
    }
}