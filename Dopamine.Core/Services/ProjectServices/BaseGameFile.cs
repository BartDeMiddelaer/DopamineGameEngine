namespace Dopamine.Core.Services.ProjectServices
{
    public class BaseGameFile
    {
        public override string ToString()
        {
            var naming =
                base.ToString()?.Split(".").ToList()
                ?? throw new ArgumentException("base.ToString() is null");

            return $"Project: {naming.Last()}";
        }
    }
}
