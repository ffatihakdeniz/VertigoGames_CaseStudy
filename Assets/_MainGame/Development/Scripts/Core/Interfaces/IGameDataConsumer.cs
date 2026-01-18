using VertigoCase.Runtime.Data;
namespace VertigoCase.Runtime
{
    public interface IGameDataConsumer
    {
        void Initialize(GameDataSO gameData);
    }
}