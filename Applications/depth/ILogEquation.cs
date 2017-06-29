namespace Depth
{
    public interface IEquation
    {
        string Name { get; }

        double Eval(double x);
        double EvalTransform(double x);
    }
}