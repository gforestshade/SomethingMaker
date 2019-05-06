
namespace SukoyakaMeteor.LightNovelMaker
{

    public interface IShowable
    {
        void Show();
    }

    public interface IDrawable
    {
        void Draw(GameController gameController);
    }

    public interface IDrawable<T>
    {
        void Draw(GameController gameController, T t);
    }

    public interface IDrawable<T1, T2>
    {
        void Draw(GameController gameController, T1 t1, T2 t2);
    }

    public interface ICanvas : IShowable, IDrawable
    {
    }

    public interface ICanvas<T> : IShowable, IDrawable<T>
    {
    }

    public interface ICanvas<T1, T2> : IShowable, IDrawable<T1, T2>
    {
    }
}