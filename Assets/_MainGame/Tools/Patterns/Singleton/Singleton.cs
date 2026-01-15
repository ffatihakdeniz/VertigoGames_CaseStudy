namespace Patterns.Singleton
{
    public class Singleton<T> where T : class, new()
    {
        private static volatile T instance;
        private static readonly object lockObject = new object();

        /// <summary>
        /// Return an instance of the class. Initializes a new instance if the instance is null.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject) // Only one thread can enter
                    {
                        instance = new T();
                    }
                }

                return instance;
            }
        }
    }
}
