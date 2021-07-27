namespace Config
{
    public class MongoDBConnectionString : BaseConfig<string>
    {
        private MongoDBConnectionString(string value) : base(value)
        {
        }

        public static implicit operator string(MongoDBConnectionString obj)
        {
            return obj.Value;
        }

        public static implicit operator MongoDBConnectionString(string value)
        {
            return new MongoDBConnectionString(value);
        }
    }
}
