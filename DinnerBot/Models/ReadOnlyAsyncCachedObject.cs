namespace DinnerBot.Models
{
    public class ReadOnlyAsyncCachedObject<T>(Func<Task<T>> generator, TimeSpan lifetime, ILogger logger)
    {
        public DateTime LastUpdated { get; private set; } = DateTime.MinValue;
        public TimeSpan LifeTime { get; } = lifetime;

        private readonly Func<Task<T>> _generator = generator;
        private T _value = default!;

        public async Task ForceUpdateAsync()
        {
            logger.LogInformation("Обновление данных...");
            LastUpdated = DateTime.UtcNow;
            _value = await _generator.Invoke();
            logger.LogInformation("Данные обновлены");
        }

        public async Task<T> GetValue()
        {
            if ((DateTime.UtcNow - LastUpdated) > LifeTime)
                await ForceUpdateAsync();
            return _value;
        }
    }
}
