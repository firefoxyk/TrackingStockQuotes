using CsvHelper;

namespace TrackingStockQuotes
{
    public class QuoteProcessor
    {
        //ассинхронное чтение котировок
        public static async Task<List<Quote>> ReadQuotesFromCsvAsync(string filePath)
        {
            var quotes = new List<Quote>();

            try
            {
                using var reader = new StreamReader(filePath);//читаем
                using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);


                // Создаем CancellationTokenSource и передаем его в GetRecordsAsync
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;

                // Отменяем операцию после 5 секунд
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

                try
                {
                    await foreach (var record in csv.GetRecordsAsync<Quote>(cancellationToken))
                    {
                        quotes.Add(record);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Обработка отмены операции
                    Console.WriteLine("Слишком долго идет операцияЮ, мы ее отменили");
                }
            }
            catch (Exception ex)
            {
                // если поймали ошибку при чтении csv
                Console.WriteLine($"Произошла ошибка при чтении котировок: {ex.Message}");
            }

                return quotes;
        }

        public static List<Quote> GetQuotesByTicker(List<Quote> quotes, string ticker)
        {
            var matchingQuotes = quotes.Where(q => q.Ticker == ticker).ToList();

            if (matchingQuotes.Any())
            {
                Console.WriteLine($"Найдены котировки для тикера {ticker}:");

                foreach (var quote in matchingQuotes)
                {
                    Console.WriteLine($"Ticker: {quote.Ticker}, TradeDate: {quote.TradeDate}, Open: {quote.Open}, High: {quote.High}, Low: {quote.Low}, Close: {quote.Close}, Volume: {quote.Volume}, Change: {quote.Change}, ChangePercent: {quote.ChangePercent}");
                }
            }
            else
            {
                Console.WriteLine($"Котировки для тикера {ticker} не найдены.");
            }

            return matchingQuotes;
        }

        public static async Task<double> CalculateAverageWeeklyVolumeAsync(List<Quote> quotes)
        {
            var totalVolumes = await Task.WhenAll(quotes.Select(quote => Task.FromResult(quote.Volume)));
            int sumOfVolumes = totalVolumes.Sum();

            return sumOfVolumes / 7.0;
        }
    }

}

