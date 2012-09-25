namespace Vienna.Input
{
    public interface IKeyboardHandler
    {
        bool OnKeyDown(int keycode);
        bool OnKeyUp(int keycode);
    }
}