using VContainer.Unity;

namespace VContainerSample.Scripts.MessagePipe
{
    public class TestPingPong : IStartable
    {
        readonly FooController _fooController;
        readonly BarController _barController;
        
        public TestPingPong(FooController fooController, BarController barController)
        {
            this._fooController = fooController;
            this._barController = barController;
        }

        public void Start()
        {
            _fooController.Foo();
            _barController.Bar();
        }
    }
}