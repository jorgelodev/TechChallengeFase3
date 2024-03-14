namespace WorkerService.Entities
{
    public class LogEmail
    {
        public int Id { get; private set; }
        public DateTime Data { get; private set; }
        public string Para { get; private set; }
        public string Conteudo { get; private set; }
        public LogEmail(string para, string conteudo)
        {
            Para = para;
            Conteudo = conteudo;
        }
    }
}
