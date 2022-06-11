using TeaFramework;

namespace TeaExampleMod
{
    public class Example : TeaMod
    {
        public override void PostSetupContent() {
            base.PostSetupContent();

            // Should log "Hello, world!"
            // See the call handler example in ./ModCall/ExampleModCalLHandler.cs
            Call("hello", "world");
        }
    }
}