using Game;

namespace Desktop.Services
{
    /// <summary>
    /// Service responsible for remembering game state between API calls from the frontend application.
    /// </summary>
    public class StateService
    {
        public GameState GameState { get; set; }
    }
}
