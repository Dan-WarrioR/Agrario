using Source.Engine.Tools;
using Source.Engine.Tools.ProjectUtilities;
using Source.Game.Factories;

namespace Source.Game
{
    public class Menu
    {
        private static UIFactory UIFactory => _uiFactory ??= Dependency.Get<UIFactory>();
        private static UIFactory? _uiFactory;

        private bool _isMenuActive = true;

        public Menu()
        {

        }

        public void Begin()
        {
            var button = UIFactory.CreateButton(new(400, 200), new(500, 500), text: "AVADA KEDABRA");

            button.OnClicked += () => Debug.Log("CLICKED");
        }
    }
}