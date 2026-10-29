namespace Gamification.Api.Events; //общий интерфейс

public interface IGameEvent
{
    Guid PlayerId { get; } // к какому игроку относится событие
    DateTime OccurredAt { get; } // когда произошло
}